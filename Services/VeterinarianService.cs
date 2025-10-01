using Microsoft.EntityFrameworkCore;
using Sprint_2_Activity_1.Data;
using Sprint_2_Activity_1.Models;

namespace Sprint_2_Activity_1.Services
{
    public class VeterinarianService : BaseService
    {
        public VeterinarianService(VeterinaryDbContext context) : base(context)
        {
        }

        public async Task ShowMenu()
        {
            bool continuar = true;
            while (continuar)
            {
                ClearScreen();
                ShowTitle("Gestión de Veterinarios");

                Console.WriteLine("1. Registrar veterinario");
                Console.WriteLine("2. Listar veterinarios");
                Console.WriteLine("3. Editar veterinario");
                Console.WriteLine("4. Eliminar veterinario");
                Console.WriteLine("5. Volver al menú principal");
                Console.WriteLine();

                int opcion = ReadInt("Selecciona una opción", false);

                switch (opcion)
                {
                    case 1:
                        await RegistrarVeterinario();
                        break;
                    case 2:
                        await ListarVeterinarios();
                        break;
                    case 3:
                        await EditarVeterinario();
                        break;
                    case 4:
                        await EliminarVeterinario();
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

        private async Task RegistrarVeterinario()
        {
            ClearScreen();
            ShowTitle("Registrar Veterinario");

            try
            {
                string nombre = ReadString("Nombre");
                string apellido = ReadString("Apellido");
                string telefono = ReadString("Teléfono");
                string email = ReadString("Email");
                string especialidad = ReadString("Especialidad");
                string numeroLicencia = ReadString("Número de licencia");

                var veterinario = new Veterinarian(nombre, apellido, telefono, email, especialidad, numeroLicencia);
                _context.Veterinarians.Add(veterinario);
                await _context.SaveChangesAsync();

                ShowMessage($"Veterinario Dr. {nombre} {apellido} registrado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al registrar veterinario: {ex.Message}", true);
            }
        }

        private async Task ListarVeterinarios()
        {
            ClearScreen();
            ShowTitle("Lista de Veterinarios");

            try
            {
                var veterinarios = await _context.Veterinarians
                    .Include(v => v.Appointments)
                    .OrderBy(v => v.LastName)
                    .ThenBy(v => v.FirstName)
                    .ToListAsync();

                if (!veterinarios.Any())
                {
                    Console.WriteLine("No hay veterinarios registrados.");
                }
                else
                {
                    Console.WriteLine($"{"ID",-5} {"Nombre",-25} {"Especialidad",-20} {"Licencia",-15} {"Atenciones",-10} {"Estado",-10}");
                    Console.WriteLine(new string('-', 90));

                    foreach (var veterinario in veterinarios)
                    {
                        string estado = veterinario.IsActive ? "Activo" : "Inactivo";
                        Console.WriteLine($"{veterinario.Id,-5} {veterinario.FirstName + " " + veterinario.LastName,-25} {veterinario.Specialty,-20} {veterinario.LicenseNumber,-15} {veterinario.Appointments.Count,-10} {estado,-10}");
                    }
                }

                Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al listar veterinarios: {ex.Message}", true);
            }
        }

        private async Task EditarVeterinario()
        {
            ClearScreen();
            ShowTitle("Editar Veterinario");

            try
            {
                int id = ReadInt("ID del veterinario a editar");
                var veterinario = await _context.Veterinarians.FindAsync(id);

                if (veterinario == null)
                {
                    ShowMessage("Veterinario no encontrado", true);
                    return;
                }

                Console.WriteLine($"\nVeterinario actual: {veterinario}");
                Console.WriteLine();

                string nombre = ReadString($"Nuevo nombre (actual: {veterinario.FirstName})", false);
                if (!string.IsNullOrWhiteSpace(nombre))
                    veterinario.FirstName = nombre;

                string apellido = ReadString($"Nuevo apellido (actual: {veterinario.LastName})", false);
                if (!string.IsNullOrWhiteSpace(apellido))
                    veterinario.LastName = apellido;

                string telefono = ReadString($"Nuevo teléfono (actual: {veterinario.Phone})", false);
                if (!string.IsNullOrWhiteSpace(telefono))
                    veterinario.Phone = telefono;

                string email = ReadString($"Nuevo email (actual: {veterinario.Email})", false);
                if (!string.IsNullOrWhiteSpace(email))
                    veterinario.Email = email;

                string especialidad = ReadString($"Nueva especialidad (actual: {veterinario.Specialty})", false);
                if (!string.IsNullOrWhiteSpace(especialidad))
                    veterinario.Specialty = especialidad;

                string numeroLicencia = ReadString($"Nuevo número de licencia (actual: {veterinario.LicenseNumber})", false);
                if (!string.IsNullOrWhiteSpace(numeroLicencia))
                    veterinario.LicenseNumber = numeroLicencia;

                Console.Write($"¿Activo? (s/n) (actual: {(veterinario.IsActive ? "sí" : "no")}): ");
                string activoInput = Console.ReadLine()?.ToLower() ?? "";
                if (!string.IsNullOrWhiteSpace(activoInput))
                {
                    veterinario.IsActive = activoInput == "s" || activoInput == "si";
                }

                await _context.SaveChangesAsync();
                ShowMessage($"Veterinario Dr. {veterinario.FirstName} {veterinario.LastName} actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al editar veterinario: {ex.Message}", true);
            }
        }

        private async Task EliminarVeterinario()
        {
            ClearScreen();
            ShowTitle("Eliminar Veterinario");

            try
            {
                int id = ReadInt("ID del veterinario a eliminar");
                var veterinario = await _context.Veterinarians
                    .Include(v => v.Appointments)
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (veterinario == null)
                {
                    ShowMessage("Veterinario no encontrado", true);
                    return;
                }

                Console.WriteLine($"\nVeterinario a eliminar: {veterinario}");
                Console.WriteLine($"Atenciones realizadas: {veterinario.Appointments.Count}");

                if (veterinario.Appointments.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("ADVERTENCIA: Este veterinario tiene atenciones médicas asociadas. ¿Estás seguro de que quieres eliminarlo?");
                    Console.ResetColor();
                }

                Console.Write("¿Confirmar eliminación? (s/n): ");
                string confirmacion = Console.ReadLine()?.ToLower() ?? "";

                if (confirmacion == "s" || confirmacion == "si")
                {
                    _context.Veterinarians.Remove(veterinario);
                    await _context.SaveChangesAsync();
                    ShowMessage($"Veterinario Dr. {veterinario.FirstName} {veterinario.LastName} eliminado exitosamente.");
                }
                else
                {
                    ShowMessage("Eliminación cancelada.");
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al eliminar veterinario: {ex.Message}", true);
            }
        }

        public async Task<List<Veterinarian>> GetAllVeterinarians()
        {
            return await _context.Veterinarians
                .Include(v => v.Appointments)
                .Where(v => v.IsActive)
                .OrderBy(v => v.LastName)
                .ThenBy(v => v.FirstName)
                .ToListAsync();
        }

        public async Task<Veterinarian?> GetVeterinarianById(int id)
        {
            return await _context.Veterinarians
                .Include(v => v.Appointments)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
    }
}
