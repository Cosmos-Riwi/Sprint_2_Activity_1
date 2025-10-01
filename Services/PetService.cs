using Microsoft.EntityFrameworkCore;
using Sprint_2_Activity_1.Data;
using Sprint_2_Activity_1.Models;

namespace Sprint_2_Activity_1.Services
{
    public class PetService : BaseService
    {
        public PetService(VeterinaryDbContext context) : base(context)
        {
        }

        public async Task ShowMenu()
        {
            bool continuar = true;
            while (continuar)
            {
                ClearScreen();
                ShowTitle("Gestión de Mascotas");

                Console.WriteLine("1. Registrar mascota");
                Console.WriteLine("2. Listar mascotas");
                Console.WriteLine("3. Editar mascota");
                Console.WriteLine("4. Eliminar mascota");
                Console.WriteLine("5. Volver al menú principal");
                Console.WriteLine();

                int opcion = ReadInt("Selecciona una opción", false);

                switch (opcion)
                {
                    case 1:
                        await RegistrarMascota();
                        break;
                    case 2:
                        await ListarMascotas();
                        break;
                    case 3:
                        await EditarMascota();
                        break;
                    case 4:
                        await EliminarMascota();
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

        private async Task RegistrarMascota()
        {
            ClearScreen();
            ShowTitle("Registrar Mascota");

            try
            {
                // Mostrar clientes disponibles
                var clientes = await _context.Clients.ToListAsync();
                if (!clientes.Any())
                {
                    ShowMessage("No hay clientes registrados. Debes registrar un cliente primero.", true);
                    return;
                }

                Console.WriteLine("Clientes disponibles:");
                foreach (var cliente in clientes)
                {
                    Console.WriteLine($"{cliente.Id}. {cliente.FirstName} {cliente.LastName}");
                }
                Console.WriteLine();

                int clienteId = ReadInt("ID del cliente dueño");
                var clienteSeleccionado = await _context.Clients.FindAsync(clienteId);
                if (clienteSeleccionado == null)
                {
                    ShowMessage("Cliente no encontrado", true);
                    return;
                }

                string nombre = ReadString("Nombre de la mascota");
                string especie = ReadString("Especie");
                string raza = ReadString("Raza");
                DateTime fechaNacimiento = ReadDate("Fecha de nacimiento");
                string sexo = ReadString("Sexo (Macho/Hembra)");
                string color = ReadString("Color", false);
                string observaciones = ReadString("Observaciones", false);

                var mascota = new Pet(nombre, especie, raza, fechaNacimiento, sexo, color, observaciones, clienteId);
                _context.Pets.Add(mascota);
                await _context.SaveChangesAsync();

                ShowMessage($"Mascota {nombre} registrada exitosamente para {clienteSeleccionado.FirstName} {clienteSeleccionado.LastName}.");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al registrar mascota: {ex.Message}", true);
            }
        }

        private async Task ListarMascotas()
        {
            ClearScreen();
            ShowTitle("Lista de Mascotas");

            try
            {
                var mascotas = await _context.Pets
                    .Include(m => m.Client)
                    .Include(m => m.Appointments)
                    .OrderBy(m => m.Name)
                    .ToListAsync();

                if (!mascotas.Any())
                {
                    Console.WriteLine("No hay mascotas registradas.");
                }
                else
                {
                    Console.WriteLine($"{"ID",-5} {"Nombre",-15} {"Especie",-10} {"Raza",-15} {"Edad",-5} {"Dueño",-20} {"Atenciones",-10}");
                    Console.WriteLine(new string('-', 90));

                    foreach (var mascota in mascotas)
                    {
                        Console.WriteLine($"{mascota.Id,-5} {mascota.Name,-15} {mascota.Species,-10} {mascota.Breed,-15} {mascota.CalculateAge(),-5} {mascota.Client.FirstName + " " + mascota.Client.LastName,-20} {mascota.Appointments.Count,-10}");
                    }
                }

                Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al listar mascotas: {ex.Message}", true);
            }
        }

        private async Task EditarMascota()
        {
            ClearScreen();
            ShowTitle("Editar Mascota");

            try
            {
                int id = ReadInt("ID de la mascota a editar");
                var mascota = await _context.Pets
                    .Include(m => m.Client)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (mascota == null)
                {
                    ShowMessage("Mascota no encontrada", true);
                    return;
                }

                Console.WriteLine($"\nMascota actual: {mascota}");
                Console.WriteLine();

                string nombre = ReadString($"Nuevo nombre (actual: {mascota.Name})", false);
                if (!string.IsNullOrWhiteSpace(nombre))
                    mascota.Name = nombre;

                string especie = ReadString($"Nueva especie (actual: {mascota.Species})", false);
                if (!string.IsNullOrWhiteSpace(especie))
                    mascota.Species = especie;

                string raza = ReadString($"Nueva raza (actual: {mascota.Breed})", false);
                if (!string.IsNullOrWhiteSpace(raza))
                    mascota.Breed = raza;

                string sexo = ReadString($"Nuevo sexo (actual: {mascota.Gender})", false);
                if (!string.IsNullOrWhiteSpace(sexo))
                    mascota.Gender = sexo;

                string color = ReadString($"Nuevo color (actual: {mascota.Color})", false);
                if (!string.IsNullOrWhiteSpace(color))
                    mascota.Color = color;

                string observaciones = ReadString($"Nuevas observaciones (actual: {mascota.Observations})", false);
                if (!string.IsNullOrWhiteSpace(observaciones))
                    mascota.Observations = observaciones;

                await _context.SaveChangesAsync();
                ShowMessage($"Mascota {mascota.Name} actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al editar mascota: {ex.Message}", true);
            }
        }

        private async Task EliminarMascota()
        {
            ClearScreen();
            ShowTitle("Eliminar Mascota");

            try
            {
                int id = ReadInt("ID de la mascota a eliminar");
                var mascota = await _context.Pets
                    .Include(m => m.Client)
                    .Include(m => m.Appointments)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (mascota == null)
                {
                    ShowMessage("Mascota no encontrada", true);
                    return;
                }

                Console.WriteLine($"\nMascota a eliminar: {mascota}");
                Console.WriteLine($"Atenciones asociadas: {mascota.Appointments.Count}");

                if (mascota.Appointments.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("ADVERTENCIA: Esta mascota tiene atenciones médicas asociadas. ¿Estás seguro de que quieres eliminarla?");
                    Console.ResetColor();
                }

                Console.Write("¿Confirmar eliminación? (s/n): ");
                string confirmacion = Console.ReadLine()?.ToLower() ?? "";

                if (confirmacion == "s" || confirmacion == "si")
                {
                    _context.Pets.Remove(mascota);
                    await _context.SaveChangesAsync();
                    ShowMessage($"Mascota {mascota.Name} eliminada exitosamente.");
                }
                else
                {
                    ShowMessage("Eliminación cancelada.");
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al eliminar mascota: {ex.Message}", true);
            }
        }

        public async Task<List<Pet>> GetAllPets()
        {
            return await _context.Pets
                .Include(m => m.Client)
                .Include(m => m.Appointments)
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        public async Task<Pet?> GetPetById(int id)
        {
            return await _context.Pets
                .Include(m => m.Client)
                .Include(m => m.Appointments)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Pet>> GetPetsByClient(int clientId)
        {
            return await _context.Pets
                .Include(m => m.Client)
                .Include(m => m.Appointments)
                .Where(m => m.ClientId == clientId)
                .OrderBy(m => m.Name)
                .ToListAsync();
        }
    }
}
