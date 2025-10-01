using Microsoft.EntityFrameworkCore;
using Sprint_2_Activity_1.Data;
using Sprint_2_Activity_1.Services;

namespace Sprint_2_Activity_1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Configure Entity Framework
            using var context = new VeterinaryDbContext(new DbContextOptionsBuilder<VeterinaryDbContext>().Options);

            // Create database if it doesn't exist
            try
            {
                await context.Database.EnsureCreatedAsync();
                Console.WriteLine("Base de datos inicializada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error al inicializar la base de datos: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Excepción interna: {ex.InnerException.Message}");
                }
                Console.ResetColor();
                Console.WriteLine("\nPasos para solucionar:");
                Console.WriteLine("1. Verificar que el servidor MySQL esté ejecutándose");
                Console.WriteLine("2. Verificar las credenciales de conexión");
                Console.WriteLine("3. Asegurar que la base de datos 'veterinaria_san_miguel_jeronimo' exista");
                Console.WriteLine("4. Verificar que el usuario 'root' tenga los permisos adecuados");
                Console.WriteLine("\nPresiona cualquier tecla para salir...");
                try
                {
                    Console.ReadKey();
                }
                catch
                {
                    // Ignore if console input is redirected
                }
                return;
            }

            // Initialize services
            var clientService = new ClientService(context);
            var petService = new PetService(context);
            var medicalHistoryService = new MedicalHistoryService(context);
            var veterinarianService = new VeterinarianService(context);
            var appointmentService = new AppointmentService(context);
            var queryService = new QueryService(context);

            // Show welcome message
            ShowWelcome();

            // Main menu
            bool keepRunning = true;
            while (keepRunning)
            {
                try
                {
                    ShowMainMenu();
                    int option = ReadOption();

                    switch (option)
                    {
                        case 1:
                            await clientService.ShowMenu();
                            break;
                        case 2:
                            await petService.ShowMenu();
                            break;
                        case 3:
                            await medicalHistoryService.ShowMenu();
                            break;
                        case 4:
                            await veterinarianService.ShowMenu();
                            break;
                        case 5:
                            await appointmentService.ShowMenu();
                            break;
                        case 6:
                            await queryService.ShowMenu();
                            break;
                        case 7:
                            keepRunning = false;
                            ShowGoodbye();
                            break;
                        default:
                            ShowMessage("Opción inválida. Por favor selecciona una opción del 1 al 7.", true);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage($"Error inesperado: {ex.Message}", true);
                }
            }
        }

        static void ShowWelcome()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("SAN MIGUEL VETERINARY SYSTEM");
            Console.WriteLine("----------------------------");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("Bienvenido al sistema de gestión veterinaria!");
            Console.WriteLine("Este sistema te permitirá gestionar clientes, mascotas,");
            Console.WriteLine("veterinarios y citas médicas de manera eficiente.");
            Console.WriteLine();
            Console.WriteLine("Presiona cualquier tecla para continuar...");
            try
            {
                Console.ReadKey();
            }
            catch
            {
                // Ignore if console input is redirected
            }
        }

        static void ShowMainMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("MENU PRINCIPAL");
            Console.WriteLine("--------------");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("1. Gestionar Clientes");
            Console.WriteLine("2. Gestionar Mascotas");
            Console.WriteLine("3. Gestionar Historial Médico");
            Console.WriteLine("4. Gestionar Veterinarios");
            Console.WriteLine("5. Gestionar Citas Médicas");
            Console.WriteLine("6. Consultas Avanzadas");
            Console.WriteLine("7. Salir");
            Console.WriteLine();
        }

        static int ReadOption()
        {
            int option;
            do
            {
                Console.Write("Selecciona una opción (1-7): ");
                string? input = Console.ReadLine();
                if (!int.TryParse(input, out option) || option < 1 || option > 7)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Por favor ingresa un número válido entre 1 y 7.");
                    Console.ResetColor();
                }
            } while (option < 1 || option > 7);

            return option;
        }

        static void ShowMessage(string message, bool isError = false)
        {
            Console.ForegroundColor = isError ? ConsoleColor.Red : ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine("\nPresiona cualquier tecla para continuar...");
            Console.ReadKey();
        }

        static void ShowGoodbye()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Gracias por usar el sistema!");
            Console.WriteLine("----------------------------");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("Sistema Veterinario San Miguel");
            Console.WriteLine("Desarrollado con C# y Entity Framework Core");
            Console.WriteLine();
            Console.WriteLine("Presiona cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}
