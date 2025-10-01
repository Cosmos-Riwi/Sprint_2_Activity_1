using Microsoft.EntityFrameworkCore;
using Sprint_2_Activity_1.Data;
using Sprint_2_Activity_1.Models;

namespace Sprint_2_Activity_1.Services
{
    public class MedicalHistoryService : BaseService
    {
        public MedicalHistoryService(VeterinaryDbContext context) : base(context)
        {
        }

        public async Task ShowMenu()
        {
            bool continuar = true;
            while (continuar)
            {
                ClearScreen();
                ShowTitle("Gestión de Historial Médico");

                Console.WriteLine("1. Crear historial médico");
                Console.WriteLine("2. Ver historial médico de mascota");
                Console.WriteLine("3. Actualizar historial médico");
                Console.WriteLine("4. Volver al menú principal");
                Console.WriteLine();

                int opcion = ReadInt("Selecciona una opción", false);

                switch (opcion)
                {
                    case 1:
                        await CrearHistorialMedico();
                        break;
                    case 2:
                        await VerHistorialMedico();
                        break;
                    case 3:
                        await ActualizarHistorialMedico();
                        break;
                    case 4:
                        continuar = false;
                        break;
                    default:
                        ShowMessage("Opción inválida", true);
                        break;
                }
            }
        }

        private async Task CrearHistorialMedico()
        {
            ClearScreen();
            ShowTitle("Crear Historial Médico");

            try
            {
                // Mostrar mascotas disponibles
                var mascotas = await _context.Pets
                    .Include(m => m.Client)
                    .Where(m => m.History == null) // Solo mascotas sin historial
                    .ToListAsync();

                if (!mascotas.Any())
                {
                    ShowMessage("No hay mascotas sin historial médico registrado.", true);
                    return;
                }

                Console.WriteLine("Mascotas sin historial médico:");
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

                if (mascotaSeleccionada.History != null)
                {
                    ShowMessage("Esta mascota ya tiene un historial médico registrado.", true);
                    return;
                }

                string descripcion = ReadString("Descripción del historial médico");
                string alergias = ReadString("Alergias conocidas", false);
                string condicionesCronicas = ReadString("Condiciones crónicas", false);
                string vacunaciones = ReadString("Vacunaciones", false);

                var historial = new MedicalHistory(descripcion, alergias, condicionesCronicas, mascotaId);
                _context.MedicalHistories.Add(historial);
                await _context.SaveChangesAsync();

                ShowMessage($"Historial médico creado exitosamente para {mascotaSeleccionada.Name}.");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al crear historial médico: {ex.Message}", true);
            }
        }

        private async Task VerHistorialMedico()
        {
            ClearScreen();
            ShowTitle("Ver Historial Médico");

            try
            {
                // Mostrar mascotas disponibles
                var mascotas = await _context.Pets
                    .Include(m => m.Client)
                    .Include(m => m.History)
                    .ToListAsync();

                if (!mascotas.Any())
                {
                    ShowMessage("No hay mascotas registradas.", true);
                    return;
                }

                Console.WriteLine("Mascotas disponibles:");
                foreach (var mascota in mascotas)
                {
                    string tieneHistorial = mascota.History != null ? "✓" : "✗";
                    Console.WriteLine($"{mascota.Id}. {mascota.Name} ({mascota.Species}) - Dueño: {mascota.Client.FirstName} {mascota.Client.LastName} - Historial: {tieneHistorial}");
                }
                Console.WriteLine();

                int mascotaId = ReadInt("ID de la mascota");
                var mascotaHistorial = await _context.Pets
                    .Include(m => m.Client)
                    .Include(m => m.History)
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

                if (mascotaHistorial.History == null)
                {
                    Console.WriteLine("Esta mascota no tiene historial médico registrado.");
                }
                else
                {
                    var historial = mascotaHistorial.History;
                    Console.WriteLine($"Descripción: {historial.Description}");
                    if (!string.IsNullOrWhiteSpace(historial.Allergies))
                        Console.WriteLine($"Alergias: {historial.Allergies}");
                    if (!string.IsNullOrWhiteSpace(historial.ChronicConditions))
                        Console.WriteLine($"Condiciones crónicas: {historial.ChronicConditions}");
                    if (!string.IsNullOrWhiteSpace(historial.Vaccinations))
                        Console.WriteLine($"Vacunaciones: {historial.Vaccinations}");
                    Console.WriteLine($"Última actualización: {historial.LastUpdated:dd/MM/yyyy HH:mm}");
                }

                Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al ver historial médico: {ex.Message}", true);
            }
        }

        private async Task ActualizarHistorialMedico()
        {
            ClearScreen();
            ShowTitle("Actualizar Historial Médico");

            try
            {
                // Mostrar mascotas con historial
                var mascotas = await _context.Pets
                    .Include(m => m.Client)
                    .Include(m => m.History)
                    .Where(m => m.History != null)
                    .ToListAsync();

                if (!mascotas.Any())
                {
                    ShowMessage("No hay mascotas con historial médico registrado.", true);
                    return;
                }

                Console.WriteLine("Mascotas con historial médico:");
                foreach (var mascota in mascotas)
                {
                    Console.WriteLine($"{mascota.Id}. {mascota.Name} ({mascota.Species}) - Dueño: {mascota.Client.FirstName} {mascota.Client.LastName}");
                }
                Console.WriteLine();

                int mascotaId = ReadInt("ID de la mascota");
                var mascotaHistorial = await _context.Pets
                    .Include(m => m.History)
                    .FirstOrDefaultAsync(m => m.Id == mascotaId);

                if (mascotaHistorial == null || mascotaHistorial.History == null)
                {
                    ShowMessage("Mascota no encontrada o sin historial médico", true);
                    return;
                }

                var historial = mascotaHistorial.History;
                Console.WriteLine($"\nHistorial actual: {historial}");
                Console.WriteLine();

                string descripcion = ReadString($"Nueva descripción (actual: {historial.Description})", false);
                if (!string.IsNullOrWhiteSpace(descripcion))
                    historial.Description = descripcion;

                string alergias = ReadString($"Nuevas alergias (actual: {historial.Allergies})", false);
                if (!string.IsNullOrWhiteSpace(alergias))
                    historial.Allergies = alergias;

                string condicionesCronicas = ReadString($"Nuevas condiciones crónicas (actual: {historial.ChronicConditions})", false);
                if (!string.IsNullOrWhiteSpace(condicionesCronicas))
                    historial.ChronicConditions = condicionesCronicas;

                string vacunaciones = ReadString($"Nuevas vacunaciones (actual: {historial.Vaccinations})", false);
                if (!string.IsNullOrWhiteSpace(vacunaciones))
                    historial.Vaccinations = vacunaciones;

                historial.LastUpdated = DateTime.Now;

                await _context.SaveChangesAsync();
                ShowMessage($"Historial médico actualizado exitosamente para {mascotaHistorial.Name}.");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al actualizar historial médico: {ex.Message}", true);
            }
        }

        public async Task<List<MedicalHistory>> GetAllMedicalHistories()
        {
            return await _context.MedicalHistories
                .Include(m => m.Pet)
                    .ThenInclude(p => p.Client)
                .OrderBy(m => m.Pet.Name)
                .ToListAsync();
        }

        public async Task<MedicalHistory?> GetMedicalHistoryByPetId(int petId)
        {
            return await _context.MedicalHistories
                .Include(m => m.Pet)
                    .ThenInclude(p => p.Client)
                .FirstOrDefaultAsync(m => m.PetId == petId);
        }
    }
}
