using Microsoft.EntityFrameworkCore;
using Sprint_2_Activity_1.Data;
using Sprint_2_Activity_1.Models;

namespace Sprint_2_Activity_1.Services
{
    public class AppointmentService : BaseService
    {
        public AppointmentService(VeterinaryDbContext context) : base(context)
        {
        }

        public async Task ShowMenu()
        {
            bool continuar = true;
            while (continuar)
            {
                ClearScreen();
                ShowTitle("Gestión de Atenciones Médicas");

                Console.WriteLine("1. Registrar atención médica");
                Console.WriteLine("2. Listar atenciones médicas");
                Console.WriteLine("3. Editar atención médica");
                Console.WriteLine("4. Eliminar atención médica");
                Console.WriteLine("5. Historial médico de mascota");
                Console.WriteLine("6. Volver al menú principal");
                Console.WriteLine();

                int opcion = ReadInt("Selecciona una opción", false);

                switch (opcion)
                {
                    case 1:
                        await RegistrarAtencion();
                        break;
                    case 2:
                        await ListarAtenciones();
                        break;
                    case 3:
                        await EditarAtencion();
                        break;
                    case 4:
                        await EliminarAtencion();
                        break;
                    case 5:
                        await MostrarHistorialMedico();
                        break;
                    case 6:
                        continuar = false;
                        break;
                    default:
                        ShowMessage("Opción no válida", true);
                        break;
                }
            }
        }

        private async Task RegistrarAtencion()
        {
            ClearScreen();
            ShowTitle("Registrar Atención Médica");

            try
            {
                // Mostrar mascotas disponibles
                var mascotas = await _context.Pets
                    .Include(m => m.Client)
                    .ToListAsync();
                if (!mascotas.Any())
                {
                    ShowMessage("No hay mascotas registradas. Debes registrar una mascota primero.", true);
                    return;
                }

                Console.WriteLine("Mascotas disponibles:");
                foreach (var mascota in mascotas)
                {
                    Console.WriteLine($"{mascota.Id}. {mascota.Name} ({mascota.Species}) - Dueño: {mascota.Client.FirstName} {mascota.Client.LastName}");
                }
                Console.WriteLine();

                int mascotaId = ReadInt("ID de la mascota");
                var mascotaSeleccionada = await _context.Pets.FindAsync(mascotaId);
                if (mascotaSeleccionada == null)
                {
                    ShowMessage("Mascota no encontrada", true);
                    return;
                }

                // Mostrar veterinarios disponibles
                var veterinarios = await _context.Veterinarians
                    .Where(v => v.IsActive)
                    .ToListAsync();
                if (!veterinarios.Any())
                {
                    ShowMessage("No hay veterinarios activos registrados.", true);
                    return;
                }

                Console.WriteLine("\nVeterinarios disponibles:");
                foreach (var veterinario in veterinarios)
                {
                    Console.WriteLine($"{veterinario.Id}. Dr. {veterinario.FirstName} {veterinario.LastName} - {veterinario.Specialty}");
                }
                Console.WriteLine();

                int veterinarioId = ReadInt("ID del veterinario");
                var veterinarioSeleccionado = await _context.Veterinarians.FindAsync(veterinarioId);
                if (veterinarioSeleccionado == null)
                {
                    ShowMessage("Veterinario no encontrado", true);
                    return;
                }

                DateTime fechaAtencion = ReadDate("Fecha de atención");
                string diagnostico = ReadString("Diagnóstico");
                string tratamiento = ReadString("Tratamiento", false);
                string observaciones = ReadString("Observaciones", false);
                decimal costo = ReadDecimal("Costo de la atención");

                var atencion = new Appointment(fechaAtencion, diagnostico, tratamiento, observaciones, costo, mascotaId, veterinarioId);
                _context.Appointments.Add(atencion);
                await _context.SaveChangesAsync();

                ShowMessage($"Atención médica registrada exitosamente para {mascotaSeleccionada.Name}.");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al registrar atención médica: {ex.Message}", true);
            }
        }

        private async Task ListarAtenciones()
        {
            ClearScreen();
            ShowTitle("Lista de Atenciones Médicas");

            try
            {
                var atenciones = await _context.Appointments
                    .Include(a => a.Pet)
                        .ThenInclude(m => m.Client)
                    .Include(a => a.Veterinarian)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToListAsync();

                if (!atenciones.Any())
                {
                    Console.WriteLine("No hay atenciones médicas registradas.");
                }
                else
                {
                    Console.WriteLine($"{"ID",-5} {"Fecha",-12} {"Mascota",-15} {"Dueño",-20} {"Veterinario",-25} {"Costo",-10}");
                    Console.WriteLine(new string('-', 95));

                    foreach (var atencion in atenciones)
                    {
                        Console.WriteLine($"{atencion.Id,-5} {atencion.AppointmentDate:dd/MM/yyyy,-12} {atencion.Pet.Name,-15} {atencion.Pet.Client.FirstName + " " + atencion.Pet.Client.LastName,-20} {atencion.Veterinarian.FirstName + " " + atencion.Veterinarian.LastName,-25} ${atencion.Cost,-9:F2}");
                    }
                }

                Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al listar atenciones médicas: {ex.Message}", true);
            }
        }

        private async Task EditarAtencion()
        {
            ClearScreen();
            ShowTitle("Editar Atención Médica");

            try
            {
                int id = ReadInt("ID de la atención a editar");
                var atencion = await _context.Appointments
                    .Include(a => a.Pet)
                    .Include(a => a.Veterinarian)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (atencion == null)
                {
                    ShowMessage("Atención no encontrada", true);
                    return;
                }

                Console.WriteLine($"\nAtención actual: {atencion}");
                Console.WriteLine();

                DateTime fechaAtencion = ReadDate($"Nueva fecha de atención (actual: {atencion.AppointmentDate:dd/MM/yyyy})", false);
                if (fechaAtencion != default)
                    atencion.AppointmentDate = fechaAtencion;

                string diagnostico = ReadString($"Nuevo diagnóstico (actual: {atencion.Diagnosis})", false);
                if (!string.IsNullOrWhiteSpace(diagnostico))
                    atencion.Diagnosis = diagnostico;

                string tratamiento = ReadString($"Nuevo tratamiento (actual: {atencion.Treatment})", false);
                if (!string.IsNullOrWhiteSpace(tratamiento))
                    atencion.Treatment = tratamiento;

                string observaciones = ReadString($"Nuevas observaciones (actual: {atencion.Observations})", false);
                if (!string.IsNullOrWhiteSpace(observaciones))
                    atencion.Observations = observaciones;

                decimal costo = ReadDecimal($"Nuevo costo (actual: {atencion.Cost:F2})", false);
                if (costo > 0)
                    atencion.Cost = costo;

                await _context.SaveChangesAsync();
                ShowMessage($"Atención médica actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al editar atención médica: {ex.Message}", true);
            }
        }

        private async Task EliminarAtencion()
        {
            ClearScreen();
            ShowTitle("Eliminar Atención Médica");

            try
            {
                int id = ReadInt("ID de la atención a eliminar");
                var atencion = await _context.Appointments
                    .Include(a => a.Pet)
                    .Include(a => a.Veterinarian)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (atencion == null)
                {
                    ShowMessage("Atención no encontrada", true);
                    return;
                }

                Console.WriteLine($"\nAtención a eliminar: {atencion}");
                Console.WriteLine($"Mascota: {atencion.Pet.Name}");
                Console.WriteLine($"Veterinario: Dr. {atencion.Veterinarian.FirstName} {atencion.Veterinarian.LastName}");

                Console.Write("¿Confirmar eliminación? (s/n): ");
                string confirmacion = Console.ReadLine()?.ToLower() ?? "";

                if (confirmacion == "s" || confirmacion == "si")
                {
                    _context.Appointments.Remove(atencion);
                    await _context.SaveChangesAsync();
                    ShowMessage($"Atención médica eliminada exitosamente.");
                }
                else
                {
                    ShowMessage("Eliminación cancelada.");
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al eliminar atención médica: {ex.Message}", true);
            }
        }

        private async Task MostrarHistorialMedico()
        {
            ClearScreen();
            ShowTitle("Historial Médico de Mascota");

            try
            {
                // Mostrar mascotas disponibles
                var mascotas = await _context.Pets
                    .Include(m => m.Client)
                    .ToListAsync();
                if (!mascotas.Any())
                {
                    ShowMessage("No hay mascotas registradas.", true);
                    return;
                }

                Console.WriteLine("Mascotas disponibles:");
                foreach (var mascota in mascotas)
                {
                    Console.WriteLine($"{mascota.Id}. {mascota.Name} ({mascota.Species}) - Dueño: {mascota.Client.FirstName} {mascota.Client.LastName}");
                }
                Console.WriteLine();

                int mascotaId = ReadInt("ID de la mascota");
                var mascotaHistorial = await _context.Pets
                    .Include(m => m.Client)
                    .Include(m => m.Appointments)
                        .ThenInclude(a => a.Veterinarian)
                    .FirstOrDefaultAsync(m => m.Id == mascotaId);

                if (mascotaHistorial == null)
                {
                    ShowMessage("Mascota no encontrada", true);
                    return;
                }

                ClearScreen();
                ShowTitle($"Historial Médico de {mascotaHistorial.Name}");

                Console.WriteLine($"Mascota: {mascotaHistorial.Name}");
                Console.WriteLine($"Especie: {mascotaHistorial.Species} - Raza: {mascotaHistorial.Breed}");
                Console.WriteLine($"Edad: {mascotaHistorial.CalculateAge()} años");
                Console.WriteLine($"Dueño: {mascotaHistorial.Client.FirstName} {mascotaHistorial.Client.LastName}");
                Console.WriteLine();

                var atenciones = mascotaHistorial.Appointments.OrderByDescending(a => a.AppointmentDate).ToList();

                if (!atenciones.Any())
                {
                    Console.WriteLine("No hay atenciones médicas registradas para esta mascota.");
                }
                else
                {
                    Console.WriteLine($"Total de atenciones: {atenciones.Count()}");
                    Console.WriteLine();

                    foreach (var atencion in atenciones)
                    {
                        Console.WriteLine($"Fecha: {atencion.AppointmentDate:dd/MM/yyyy}");
                        Console.WriteLine($"Veterinario: Dr. {atencion.Veterinarian.FirstName} {atencion.Veterinarian.LastName} ({atencion.Veterinarian.Specialty})");
                        Console.WriteLine($"Diagnóstico: {atencion.Diagnosis}");
                        if (!string.IsNullOrWhiteSpace(atencion.Treatment))
                            Console.WriteLine($"Tratamiento: {atencion.Treatment}");
                        if (!string.IsNullOrWhiteSpace(atencion.Observations))
                            Console.WriteLine($"Observaciones: {atencion.Observations}");
                        Console.WriteLine($"Costo: ${atencion.Cost:F2}");
                        Console.WriteLine(new string('-', 50));
                    }
                }

                Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al mostrar historial médico: {ex.Message}", true);
            }
        }

        public async Task<List<Appointment>> GetAllAppointments()
        {
            return await _context.Appointments
                .Include(a => a.Pet)
                    .ThenInclude(m => m.Client)
                .Include(a => a.Veterinarian)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<Appointment?> GetAppointmentById(int id)
        {
            return await _context.Appointments
                .Include(a => a.Pet)
                    .ThenInclude(m => m.Client)
                .Include(a => a.Veterinarian)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Appointment>> GetAppointmentsByPet(int petId)
        {
            return await _context.Appointments
                .Include(a => a.Pet)
                    .ThenInclude(m => m.Client)
                .Include(a => a.Veterinarian)
                .Where(a => a.PetId == petId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }
    }
}
