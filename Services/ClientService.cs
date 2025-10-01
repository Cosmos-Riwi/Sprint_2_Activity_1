using Microsoft.EntityFrameworkCore;
using Sprint_2_Activity_1.Data;
using Sprint_2_Activity_1.Models;

namespace Sprint_2_Activity_1.Services
{
    public class ClientService : BaseService
    {
        public ClientService(VeterinaryDbContext context) : base(context)
        {
        }

        public async Task ShowMenu()
        {
            bool continuar = true;
            while (continuar)
            {
                ClearScreen();
                ShowTitle("Gestión de Clientes");

                Console.WriteLine("1. Registrar cliente");
                Console.WriteLine("2. Listar clientes");
                Console.WriteLine("3. Editar cliente");
                Console.WriteLine("4. Eliminar cliente");
                Console.WriteLine("5. Volver al menú principal");
                Console.WriteLine();

                int opcion = ReadInt("Selecciona una opción", false);

                switch (opcion)
                {
                    case 1:
                        await RegistrarCliente();
                        break;
                    case 2:
                        await ListarClientes();
                        break;
                    case 3:
                        await EditarCliente();
                        break;
                    case 4:
                        await EliminarCliente();
                        break;
                    case 5:
                        continuar = false;
                        break;
                    default:
                        ShowMessage("Opción inválida", true);
                        break;
                }
            }
        }

        private async Task RegistrarCliente()
        {
            ClearScreen();
            ShowTitle("Registrar Cliente");

            try
            {
                string nombre = ReadString("Nombre");
                string apellido = ReadString("Apellido");
                string telefono = ReadString("Teléfono");
                string email = ReadString("Email");
                string direccion = ReadString("Dirección");

                var cliente = new Client(nombre, apellido, telefono, email, direccion);
                _context.Clients.Add(cliente);
                await _context.SaveChangesAsync();

                ShowMessage($"Cliente {nombre} {apellido} registrado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al registrar cliente: {ex.Message}", true);
            }
        }

        private async Task ListarClientes()
        {
            ClearScreen();
            ShowTitle("Lista de Clientes");

            try
            {
                var clientes = await _context.Clients
                    .Include(c => c.Pets)
                    .OrderBy(c => c.LastName)
                    .ThenBy(c => c.FirstName)
                    .ToListAsync();

                if (!clientes.Any())
                {
                    Console.WriteLine("No hay clientes registrados.");
                }
                else
                {
                    Console.WriteLine($"{"ID",-5} {"Nombre",-20} {"Teléfono",-15} {"Email",-25} {"Mascotas",-10}");
                    Console.WriteLine(new string('-', 80));

                    foreach (var cliente in clientes)
                    {
                        Console.WriteLine($"{cliente.Id,-5} {cliente.FirstName + " " + cliente.LastName,-20} {cliente.Phone,-15} {cliente.Email,-25} {cliente.Pets.Count,-10}");
                    }
                }

                Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al listar clientes: {ex.Message}", true);
            }
        }

        private async Task EditarCliente()
        {
            ClearScreen();
            ShowTitle("Editar Cliente");

            try
            {
                int id = ReadInt("ID del cliente a editar");
                var cliente = await _context.Clients.FindAsync(id);

                if (cliente == null)
                {
                    ShowMessage("Cliente no encontrado", true);
                    return;
                }

                Console.WriteLine($"\nCliente actual: {cliente}");
                Console.WriteLine();

                string nombre = ReadString($"Nuevo nombre (actual: {cliente.FirstName})", false);
                if (!string.IsNullOrWhiteSpace(nombre))
                    cliente.FirstName = nombre;

                string apellido = ReadString($"Nuevo apellido (actual: {cliente.LastName})", false);
                if (!string.IsNullOrWhiteSpace(apellido))
                    cliente.LastName = apellido;

                string telefono = ReadString($"Nuevo teléfono (actual: {cliente.Phone})", false);
                if (!string.IsNullOrWhiteSpace(telefono))
                    cliente.Phone = telefono;

                string email = ReadString($"Nuevo email (actual: {cliente.Email})", false);
                if (!string.IsNullOrWhiteSpace(email))
                    cliente.Email = email;

                string direccion = ReadString($"Nueva dirección (actual: {cliente.Address})", false);
                if (!string.IsNullOrWhiteSpace(direccion))
                    cliente.Address = direccion;

                await _context.SaveChangesAsync();
                ShowMessage($"Cliente {cliente.FirstName} {cliente.LastName} actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al editar cliente: {ex.Message}", true);
            }
        }

        private async Task EliminarCliente()
        {
            ClearScreen();
            ShowTitle("Eliminar Cliente");

            try
            {
                int id = ReadInt("ID del cliente a eliminar");
                var cliente = await _context.Clients
                    .Include(c => c.Pets)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (cliente == null)
                {
                    ShowMessage("Cliente no encontrado", true);
                    return;
                }

                Console.WriteLine($"\nCliente a eliminar: {cliente}");
                Console.WriteLine($"Mascotas asociadas: {cliente.Pets.Count}");

                if (cliente.Pets.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("ADVERTENCIA: Este cliente tiene mascotas asociadas. ¿Estás seguro de que quieres eliminarlo?");
                    Console.ResetColor();
                }

                Console.Write("¿Confirmar eliminación? (s/n): ");
                string confirmacion = Console.ReadLine()?.ToLower() ?? "";

                if (confirmacion == "s" || confirmacion == "si")
                {
                    _context.Clients.Remove(cliente);
                    await _context.SaveChangesAsync();
                    ShowMessage($"Cliente {cliente.FirstName} {cliente.LastName} eliminado exitosamente.");
                }
                else
                {
                    ShowMessage("Eliminación cancelada.");
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al eliminar cliente: {ex.Message}", true);
            }
        }

        public async Task<List<Client>> GetAllClients()
        {
            return await _context.Clients
                .Include(c => c.Pets)
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToListAsync();
        }

        public async Task<Client?> GetClientById(int id)
        {
            return await _context.Clients
                .Include(c => c.Pets)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
