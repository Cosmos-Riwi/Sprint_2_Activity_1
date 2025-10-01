using Microsoft.EntityFrameworkCore;
using Sprint_2_Activity_1.Data;
using Sprint_2_Activity_1.Models;

namespace Sprint_2_Activity_1.Services
{
    public class QueryService : BaseService
    {
        public QueryService(VeterinaryDbContext context) : base(context)
        {
        }

        public async Task ShowMenu()
        {
            bool continuar = true;
            while (continuar)
            {
                ClearScreen();
                ShowTitle("Consultas Avanzadas");

                Console.WriteLine("1. Mascotas de un cliente");
                Console.WriteLine("2. Veterinario con más atenciones");
                Console.WriteLine("3. Especie de mascota más atendida");
                Console.WriteLine("4. Cliente con más mascotas");
                Console.WriteLine("5. Volver al menú principal");
                Console.WriteLine();

                int opcion = ReadInt("Selecciona una opción", false);

                switch (opcion)
                {
                    case 1:
                        await ConsultarMascotasPorCliente();
                        break;
                    case 2:
                        await ConsultarVeterinarioConMasAtenciones();
                        break;
                    case 3:
                        await ConsultarEspecieMasAtendida();
                        break;
                    case 4:
                        await ConsultarClienteConMasMascotas();
                        break;
                    case 5:
                        continuar = false;
                        break;
                    default:
                        ShowMessage("Opción no válida", true);
                        break;
                }
            }
        }

        private async Task ConsultarMascotasPorCliente()
        {
            ClearScreen();
            ShowTitle("Mascotas de un Cliente");

            try
            {
                // Mostrar clientes disponibles
                var clientes = await _context.Clients.ToListAsync();
                if (!clientes.Any())
                {
                    ShowMessage("No hay clientes registrados.", true);
                    return;
                }

                Console.WriteLine("Clientes disponibles:");
                foreach (var cliente in clientes)
                {
                    Console.WriteLine($"{cliente.Id}. {cliente.FirstName} {cliente.LastName}");
                }
                Console.WriteLine();

                int clienteId = ReadInt("ID del cliente");
                var clienteSeleccionado = await _context.Clients.FindAsync(clienteId);
                if (clienteSeleccionado == null)
                {
                    ShowMessage("Cliente no encontrado", true);
                    return;
                }

                // Consulta LINQ para obtener mascotas del cliente
                var mascotas = await _context.Pets
                    .Include(m => m.Appointments)
                    .Where(m => m.ClientId == clienteId)
                    .OrderBy(m => m.Name)
                    .ToListAsync();

                ClearScreen();
                ShowTitle($"Mascotas de {clienteSeleccionado.FirstName} {clienteSeleccionado.LastName}");

                if (!mascotas.Any())
                {
                    Console.WriteLine("Este cliente no tiene mascotas registradas.");
                }
                else
                {
                    Console.WriteLine($"Total de mascotas: {mascotas.Count}");
                    Console.WriteLine();

                    foreach (var mascota in mascotas)
                    {
                        Console.WriteLine($"Nombre: {mascota.Name}");
                        Console.WriteLine($"Especie: {mascota.Species} - Raza: {mascota.Breed}");
                        Console.WriteLine($"Edad: {mascota.CalculateAge()} años");
                        Console.WriteLine($"Sexo: {mascota.Gender}");
                        if (!string.IsNullOrWhiteSpace(mascota.Color))
                            Console.WriteLine($"Color: {mascota.Color}");
                        Console.WriteLine($"Atenciones médicas: {mascota.Appointments.Count}");
                        Console.WriteLine(new string('-', 40));
                    }
                }

                Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                ShowMessage($"Error en la consulta: {ex.Message}", true);
            }
        }

        private async Task ConsultarVeterinarioConMasAtenciones()
        {
            ClearScreen();
            ShowTitle("Veterinario con Más Atenciones");

            try
            {
                // Consulta LINQ para encontrar el veterinario con más atenciones
                var veterinarioConMasAtenciones = await _context.Veterinarians
                    .Include(v => v.Appointments)
                    .Where(v => v.IsActive)
                    .OrderByDescending(v => v.Appointments.Count)
                    .FirstOrDefaultAsync();

                if (veterinarioConMasAtenciones == null)
                {
                    Console.WriteLine("No hay veterinarios registrados.");
                }
                else
                {
                    Console.WriteLine($"Veterinario con más atenciones realizadas:");
                    Console.WriteLine();
                    Console.WriteLine($"Nombre: Dr. {veterinarioConMasAtenciones.FirstName} {veterinarioConMasAtenciones.LastName}");
                    Console.WriteLine($"Especialidad: {veterinarioConMasAtenciones.Specialty}");
                    Console.WriteLine($"Número de licencia: {veterinarioConMasAtenciones.LicenseNumber}");
                    Console.WriteLine($"Total de atenciones: {veterinarioConMasAtenciones.Appointments.Count}");
                    Console.WriteLine($"Fecha de contratación: {veterinarioConMasAtenciones.HireDate:dd/MM/yyyy}");

                    // Mostrar estadísticas adicionales
                    var totalAtenciones = await _context.Appointments.CountAsync();
                    if (totalAtenciones > 0)
                    {
                        double porcentaje = (double)veterinarioConMasAtenciones.Appointments.Count / totalAtenciones * 100;
                        Console.WriteLine($"Porcentaje del total de atenciones: {porcentaje:F1}%");
                    }
                }

                Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                ShowMessage($"Error en la consulta: {ex.Message}", true);
            }
        }

        private async Task ConsultarEspecieMasAtendida()
        {
            ClearScreen();
            ShowTitle("Especie de Mascota Más Atendida");

            try
            {
                // Consulta LINQ para encontrar la especie más atendida
                var especiesMasAtendidas = await _context.Pets
                    .Include(m => m.Appointments)
                    .GroupBy(m => m.Species)
                    .Select(g => new
                    {
                        Especie = g.Key,
                        TotalAtenciones = g.Sum(m => m.Appointments.Count),
                        TotalMascotas = g.Count()
                    })
                    .OrderByDescending(x => x.TotalAtenciones)
                    .ToListAsync();

                if (!especiesMasAtendidas.Any())
                {
                    Console.WriteLine("No hay mascotas registradas.");
                }
                else
                {
                    var especieMasAtendida = especiesMasAtendidas.First();
                    
                    Console.WriteLine($"Especie más atendida: {especieMasAtendida.Especie}");
                    Console.WriteLine($"Total de atenciones: {especieMasAtendida.TotalAtenciones}");
                    Console.WriteLine($"Total de mascotas de esta especie: {especieMasAtendida.TotalMascotas}");
                    
                    if (especieMasAtendida.TotalMascotas > 0)
                    {
                        double promedioAtenciones = (double)especieMasAtendida.TotalAtenciones / especieMasAtendida.TotalMascotas;
                        Console.WriteLine($"Promedio de atenciones por mascota: {promedioAtenciones:F1}");
                    }

                    Console.WriteLine();
                    Console.WriteLine("Ranking de especies por atenciones:");
                    Console.WriteLine(new string('-', 50));

                    foreach (var especie in especiesMasAtendidas)
                    {
                        Console.WriteLine($"{especie.Especie,-15} | Atenciones: {especie.TotalAtenciones,-5} | Mascotas: {especie.TotalMascotas}");
                    }
                }

                Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                ShowMessage($"Error en la consulta: {ex.Message}", true);
            }
        }

        private async Task ConsultarClienteConMasMascotas()
        {
            ClearScreen();
            ShowTitle("Cliente con Más Mascotas");

            try
            {
                // Consulta LINQ para encontrar el cliente con más mascotas
                var clientesConMasMascotas = await _context.Clients
                    .Include(c => c.Pets)
                    .Select(c => new
                    {
                        Cliente = c,
                        TotalMascotas = c.Pets.Count,
                        TotalAtenciones = c.Pets.Sum(m => m.Appointments.Count)
                    })
                    .OrderByDescending(x => x.TotalMascotas)
                    .ToListAsync();

                if (!clientesConMasMascotas.Any())
                {
                    Console.WriteLine("No hay clientes registrados.");
                }
                else
                {
                    var clienteConMasMascotas = clientesConMasMascotas.First();
                    var cliente = clienteConMasMascotas.Cliente;

                    Console.WriteLine($"Cliente con más mascotas:");
                    Console.WriteLine();
                    Console.WriteLine($"Nombre: {cliente.FirstName} {cliente.LastName}");
                    Console.WriteLine($"Teléfono: {cliente.Phone}");
                    Console.WriteLine($"Email: {cliente.Email}");
                    Console.WriteLine($"Dirección: {cliente.Address}");
                    Console.WriteLine($"Total de mascotas: {clienteConMasMascotas.TotalMascotas}");
                    Console.WriteLine($"Total de atenciones médicas: {clienteConMasMascotas.TotalAtenciones}");
                    Console.WriteLine($"Fecha de registro: {cliente.RegistrationDate:dd/MM/yyyy}");

                    Console.WriteLine();
                    Console.WriteLine("Mascotas del cliente:");
                    Console.WriteLine(new string('-', 40));

                    foreach (var mascota in cliente.Pets)
                    {
                        Console.WriteLine($"• {mascota.Name} ({mascota.Species} - {mascota.Breed}) - {mascota.CalculateAge()} años");
                    }

                    Console.WriteLine();
                    Console.WriteLine("Top 5 clientes con más mascotas:");
                    Console.WriteLine(new string('-', 50));

                    var top5 = clientesConMasMascotas.Take(5);
                    foreach (var item in top5)
                    {
                        Console.WriteLine($"{item.Cliente.FirstName} {item.Cliente.LastName,-20} | Mascotas: {item.TotalMascotas,-3} | Atenciones: {item.TotalAtenciones}");
                    }
                }

                Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                ShowMessage($"Error en la consulta: {ex.Message}", true);
            }
        }
    }
}
