using Microsoft.EntityFrameworkCore;
using Veterinaria.Data;
using Veterinaria.Models;

namespace Veterinaria;

public static class Program
{
    private static readonly VeterinariaContext Db = new VeterinariaContext();

    public static void Main()
    {
        try
        {
            Db.Database.EnsureCreated();
            Console.WriteLine("¡Bienvenido a Veterinaria San Miguel!");
            Console.WriteLine("Base de datos conectada correctamente.");
            EjecutarMenuPrincipal();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al conectar con la base de datos: {ex.Message}");
        }
    }

    private static void EjecutarMenuPrincipal()
    {
        while (true)
        {
            Console.WriteLine("\n==== Veterinaria San Miguel ====");
            Console.WriteLine("1. Gestión de Clientes");
            Console.WriteLine("2. Gestión de Mascotas");
            Console.WriteLine("3. Gestión de Veterinarios");
            Console.WriteLine("4. Gestión de Atenciones Médicas");
            Console.WriteLine("5. Historial Médico por Mascota");
            Console.WriteLine("6. Consultas Avanzadas");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opción: ");

            var opcion = Console.ReadLine();
            switch (opcion)
            {
                case "1": MenuClientes(); break;
                case "2": MenuMascotas(); break;
                case "3": MenuVeterinarios(); break;
                case "4": MenuAtenciones(); break;
                case "5": MostrarHistorialMedico(); break;
                case "6": MenuConsultasAvanzadas(); break;
                case "0": 
                    Console.WriteLine("¡Hasta luego!");
                    return;
                default: 
                    Console.WriteLine("Opción no válida. Presione Enter para continuar...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    // --- Clientes ---
    private static void MenuClientes()
    {
        while (true)
        {
            Console.WriteLine("\n-- Clientes --");
            Console.WriteLine("1. Registrar");
            Console.WriteLine("2. Listar");
            Console.WriteLine("3. Editar");
            Console.WriteLine("4. Eliminar");
            Console.WriteLine("0. Volver");
            Console.Write("Opción: ");
            var op = Console.ReadLine();
            switch (op)
            {
                case "1": RegistrarCliente(); break;
                case "2": ListarClientes(); break;
                case "3": EditarCliente(); break;
                case "4": EliminarCliente(); break;
                case "0": return;
                default: 
                    Console.WriteLine("Opción no válida. Presione Enter para continuar...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private static void RegistrarCliente()
    {
        try
        {
            Console.Write("Nombres: "); 
            var nombres = Console.ReadLine() ?? string.Empty;
            Console.Write("Apellidos: "); 
            var apellidos = Console.ReadLine() ?? string.Empty;
            Console.Write("Email: "); 
            var email = Console.ReadLine();
            Console.Write("Teléfono: "); 
            var telefono = Console.ReadLine();

            var cliente = new Cliente 
            { 
                Nombres = nombres, 
                Apellidos = apellidos, 
                Email = email, 
                Telefono = telefono 
            };

            Db.Clientes.Add(cliente);
            Db.SaveChanges();
            Console.WriteLine("Cliente registrado exitosamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al registrar el cliente: " + ex.Message);
        }
        Console.WriteLine("Presione Enter para continuar...");
        Console.ReadLine();
    }

    private static void ListarClientes()
    {
        try
        {
            var clientes = Db.Clientes.AsNoTracking().OrderBy(c => c.Apellidos).ToList();
            if (clientes.Any())
            {
                Console.WriteLine("\n--- Lista de Clientes ---");
                foreach (var c in clientes)
                    Console.WriteLine($"{c.Id}. {c.Apellidos}, {c.Nombres} - {c.Email} - {c.Telefono}");
            }
            else
            {
                Console.WriteLine("No hay clientes registrados.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al listar clientes: " + ex.Message);
        }
        Console.WriteLine("\nPresione Enter para continuar...");
        Console.ReadLine();
    }

    private static void EditarCliente()
    {
        try
        {
            ListarClientes();
            Console.Write("ID del cliente a editar: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var cliente = Db.Clientes.Find(id);
            if (cliente == null)
            {
                Console.WriteLine("Cliente no encontrado.");
                return;
            }

            Console.Write($"Nombres actual: {cliente.Nombres}. Nuevo: ");
            var nuevosNombres = Console.ReadLine();
            if (!string.IsNullOrEmpty(nuevosNombres))
                cliente.Nombres = nuevosNombres;

            Console.Write($"Apellidos actual: {cliente.Apellidos}. Nuevo: ");
            var nuevosApellidos = Console.ReadLine();
            if (!string.IsNullOrEmpty(nuevosApellidos))
                cliente.Apellidos = nuevosApellidos;

            Console.Write($"Email actual: {cliente.Email}. Nuevo: ");
            var nuevoEmail = Console.ReadLine();
            if (!string.IsNullOrEmpty(nuevoEmail))
                cliente.Email = nuevoEmail;

            Console.Write($"Teléfono actual: {cliente.Telefono}. Nuevo: ");
            var nuevoTelefono = Console.ReadLine();
            if (!string.IsNullOrEmpty(nuevoTelefono))
                cliente.Telefono = nuevoTelefono;

            Db.SaveChanges();
            Console.WriteLine("Cliente actualizado exitosamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al editar cliente: " + ex.Message);
        }
        Console.WriteLine("Presione Enter para continuar...");
        Console.ReadLine();
    }

    private static void EliminarCliente()
    {
        try
        {
            ListarClientes();
            Console.Write("ID del cliente a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var cliente = Db.Clientes.Find(id);
            if (cliente == null)
            {
                Console.WriteLine("Cliente no encontrado.");
                return;
            }

            Console.Write($"¿Está seguro de eliminar a {cliente.Nombres} {cliente.Apellidos}? (s/n): ");
            var confirmacion = Console.ReadLine();
            if (confirmacion?.ToLower() == "s")
            {
                Db.Clientes.Remove(cliente);
                Db.SaveChanges();
                Console.WriteLine("Cliente eliminado exitosamente.");
            }
            else
            {
                Console.WriteLine("Operación cancelada.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al eliminar cliente: " + ex.Message);
        }
        Console.WriteLine("Presione Enter para continuar...");
        Console.ReadLine();
    }

    // --- Mascotas ---
    private static void MenuMascotas()
    {
        while (true)
        {
            Console.WriteLine("\n-- Mascotas --");
            Console.WriteLine("1. Registrar");
            Console.WriteLine("2. Listar");
            Console.WriteLine("3. Editar");
            Console.WriteLine("4. Eliminar");
            Console.WriteLine("0. Volver");
            Console.Write("Opción: ");
            var op = Console.ReadLine();
            switch (op)
            {
                case "1": RegistrarMascota(); break;
                case "2": ListarMascotas(); break;
                case "3": EditarMascota(); break;
                case "4": EliminarMascota(); break;
                case "0": return;
                default: 
                    Console.WriteLine("Opción no válida. Presione Enter para continuar...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private static void RegistrarMascota()
    {
        try
        {
            ListarClientes();
            Console.Write("ID del cliente: ");
            if (!int.TryParse(Console.ReadLine(), out var clienteId))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var cliente = Db.Clientes.Find(clienteId);
            if (cliente == null)
            {
                Console.WriteLine("Cliente no encontrado.");
                return;
            }

            Console.Write("Nombre de la mascota: ");
            var nombre = Console.ReadLine() ?? string.Empty;
            
            Console.WriteLine("Especie:");
            Console.WriteLine("1. Perro");
            Console.WriteLine("2. Gato");
            Console.WriteLine("3. Ave");
            Console.WriteLine("4. Roedor");
            Console.WriteLine("5. Reptil");
            Console.WriteLine("99. Otro");
            Console.Write("Seleccione una opción: ");
            if (!int.TryParse(Console.ReadLine(), out var especieId))
            {
                Console.WriteLine("Opción inválida.");
                return;
            }
            var especie = (Especie)especieId;
            
            Console.Write("Raza: ");
            var raza = Console.ReadLine() ?? string.Empty;
            Console.Write("Fecha de nacimiento (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out var fechaNacimiento))
            {
                Console.WriteLine("Fecha inválida.");
                return;
            }

            var mascota = new Mascota
            {
                Nombre = nombre,
                Especie = especie,
                Raza = raza,
                FechaNacimiento = fechaNacimiento,
                ClienteId = clienteId
            };

            Db.Mascotas.Add(mascota);
            Db.SaveChanges();
            Console.WriteLine("Mascota registrada exitosamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al registrar la mascota: " + ex.Message);
        }
        Console.WriteLine("Presione Enter para continuar...");
        Console.ReadLine();
    }

    private static void ListarMascotas()
    {
        try
        {
            var mascotas = Db.Mascotas
                .AsNoTracking()
                .Include(m => m.Cliente)
                .OrderBy(m => m.Nombre)
                .ToList();

            if (mascotas.Any())
            {
                Console.WriteLine("\n--- Lista de Mascotas ---");
                foreach (var m in mascotas)
                {
                    var edad = m.FechaNacimiento.HasValue ? DateTime.Now.Year - m.FechaNacimiento.Value.Year : 0;
                    Console.WriteLine($"{m.Id}. {m.Nombre} ({m.Especie} - {m.Raza}) - {edad} años - Dueño: {m.Cliente?.Nombres} {m.Cliente?.Apellidos}");
                }
            }
            else
            {
                Console.WriteLine("No hay mascotas registradas.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al listar mascotas: " + ex.Message);
        }
        Console.WriteLine("\nPresione Enter para continuar...");
        Console.ReadLine();
    }

    private static void EditarMascota()
    {
        try
        {
            ListarMascotas();
            Console.Write("ID de la mascota a editar: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var mascota = Db.Mascotas.Find(id);
            if (mascota == null)
            {
                Console.WriteLine("Mascota no encontrada.");
                return;
            }

            Console.Write($"Nombre actual: {mascota.Nombre}. Nuevo: ");
            var nuevoNombre = Console.ReadLine();
            if (!string.IsNullOrEmpty(nuevoNombre))
                mascota.Nombre = nuevoNombre;

            Console.WriteLine($"Especie actual: {mascota.Especie}");
            Console.WriteLine("Nueva especie:");
            Console.WriteLine("1. Perro");
            Console.WriteLine("2. Gato");
            Console.WriteLine("3. Ave");
            Console.WriteLine("4. Roedor");
            Console.WriteLine("5. Reptil");
            Console.WriteLine("99. Otro");
            Console.Write("Seleccione una opción (Enter para mantener actual): ");
            var nuevaEspecieInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(nuevaEspecieInput) && int.TryParse(nuevaEspecieInput, out var nuevaEspecieId))
                mascota.Especie = (Especie)nuevaEspecieId;

            Console.Write($"Raza actual: {mascota.Raza}. Nuevo: ");
            var nuevaRaza = Console.ReadLine();
            if (!string.IsNullOrEmpty(nuevaRaza))
                mascota.Raza = nuevaRaza;

            Db.SaveChanges();
            Console.WriteLine("Mascota actualizada exitosamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al editar mascota: " + ex.Message);
        }
        Console.WriteLine("Presione Enter para continuar...");
        Console.ReadLine();
    }

    private static void EliminarMascota()
    {
        try
        {
            ListarMascotas();
            Console.Write("ID de la mascota a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var mascota = Db.Mascotas.Find(id);
            if (mascota == null)
            {
                Console.WriteLine("Mascota no encontrada.");
                return;
            }

            Console.Write($"¿Está seguro de eliminar a {mascota.Nombre}? (s/n): ");
            var confirmacion = Console.ReadLine();
            if (confirmacion?.ToLower() == "s")
            {
                Db.Mascotas.Remove(mascota);
                Db.SaveChanges();
                Console.WriteLine("Mascota eliminada exitosamente.");
            }
            else
            {
                Console.WriteLine("Operación cancelada.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al eliminar mascota: " + ex.Message);
        }
        Console.WriteLine("Presione Enter para continuar...");
        Console.ReadLine();
    }

    // --- Veterinarios ---
    private static void MenuVeterinarios()
    {
        while (true)
        {
            Console.WriteLine("\n-- Veterinarios --");
            Console.WriteLine("1. Registrar");
            Console.WriteLine("2. Listar");
            Console.WriteLine("3. Editar");
            Console.WriteLine("4. Eliminar");
            Console.WriteLine("0. Volver");
            Console.Write("Opción: ");
            var op = Console.ReadLine();
            switch (op)
            {
                case "1": RegistrarVeterinario(); break;
                case "2": ListarVeterinarios(); break;
                case "3": EditarVeterinario(); break;
                case "4": EliminarVeterinario(); break;
                case "0": return;
                default: 
                    Console.WriteLine("Opción no válida. Presione Enter para continuar...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private static void RegistrarVeterinario()
    {
        try
        {
            Console.Write("Nombres: ");
            var nombres = Console.ReadLine() ?? string.Empty;
            Console.Write("Apellidos: ");
            var apellidos = Console.ReadLine() ?? string.Empty;
            Console.Write("Especialidad: ");
            var especialidad = Console.ReadLine();

            var veterinario = new Veterinario
            {
                Nombres = nombres,
                Apellidos = apellidos,
                Especialidad = especialidad
            };

            Db.Veterinarios.Add(veterinario);
            Db.SaveChanges();
            Console.WriteLine("Veterinario registrado exitosamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al registrar el veterinario: " + ex.Message);
        }
        Console.WriteLine("Presione Enter para continuar...");
        Console.ReadLine();
    }

    private static void ListarVeterinarios()
    {
        try
        {
            var veterinarios = Db.Veterinarios.AsNoTracking().OrderBy(v => v.Apellidos).ToList();
            if (veterinarios.Any())
            {
                Console.WriteLine("\n--- Lista de Veterinarios ---");
                foreach (var v in veterinarios)
                    Console.WriteLine($"{v.Id}. {v.Apellidos}, {v.Nombres} - {v.Especialidad}");
            }
            else
            {
                Console.WriteLine("No hay veterinarios registrados.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al listar veterinarios: " + ex.Message);
        }
        Console.WriteLine("\nPresione Enter para continuar...");
        Console.ReadLine();
    }

    private static void EditarVeterinario()
    {
        try
        {
            ListarVeterinarios();
            Console.Write("ID del veterinario a editar: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var veterinario = Db.Veterinarios.Find(id);
            if (veterinario == null)
            {
                Console.WriteLine("Veterinario no encontrado.");
                return;
            }

            Console.Write($"Nombres actual: {veterinario.Nombres}. Nuevo: ");
            var nuevosNombres = Console.ReadLine();
            if (!string.IsNullOrEmpty(nuevosNombres))
                veterinario.Nombres = nuevosNombres;

            Console.Write($"Apellidos actual: {veterinario.Apellidos}. Nuevo: ");
            var nuevosApellidos = Console.ReadLine();
            if (!string.IsNullOrEmpty(nuevosApellidos))
                veterinario.Apellidos = nuevosApellidos;

            Console.Write($"Especialidad actual: {veterinario.Especialidad}. Nuevo: ");
            var nuevaEspecialidad = Console.ReadLine();
            if (!string.IsNullOrEmpty(nuevaEspecialidad))
                veterinario.Especialidad = nuevaEspecialidad;

            Db.SaveChanges();
            Console.WriteLine("Veterinario actualizado exitosamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al editar veterinario: " + ex.Message);
        }
        Console.WriteLine("Presione Enter para continuar...");
        Console.ReadLine();
    }

    private static void EliminarVeterinario()
    {
        try
        {
            ListarVeterinarios();
            Console.Write("ID del veterinario a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var veterinario = Db.Veterinarios.Find(id);
            if (veterinario == null)
            {
                Console.WriteLine("Veterinario no encontrado.");
                return;
            }

            Console.Write($"¿Está seguro de eliminar a {veterinario.Nombres} {veterinario.Apellidos}? (s/n): ");
            var confirmacion = Console.ReadLine();
            if (confirmacion?.ToLower() == "s")
            {
                Db.Veterinarios.Remove(veterinario);
                Db.SaveChanges();
                Console.WriteLine("Veterinario eliminado exitosamente.");
            }
            else
            {
                Console.WriteLine("Operación cancelada.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al eliminar veterinario: " + ex.Message);
        }
        Console.WriteLine("Presione Enter para continuar...");
        Console.ReadLine();
    }

    // --- Atenciones ---
    private static void MenuAtenciones()
    {
        while (true)
        {
            Console.WriteLine("\n-- Atenciones Médicas --");
            Console.WriteLine("1. Registrar");
            Console.WriteLine("2. Listar");
            Console.WriteLine("3. Editar");
            Console.WriteLine("4. Eliminar");
            Console.WriteLine("0. Volver");
            Console.Write("Opción: ");
            var op = Console.ReadLine();
            switch (op)
            {
                case "1": RegistrarAtencion(); break;
                case "2": ListarAtenciones(); break;
                case "3": EditarAtencion(); break;
                case "4": EliminarAtencion(); break;
                case "0": return;
                default: 
                    Console.WriteLine("Opción no válida. Presione Enter para continuar...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private static void RegistrarAtencion()
    {
        try
        {
            ListarMascotas();
            Console.Write("ID de la mascota: ");
            if (!int.TryParse(Console.ReadLine(), out var mascotaId))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var mascota = Db.Mascotas.Find(mascotaId);
            if (mascota == null)
            {
                Console.WriteLine("Mascota no encontrada.");
                return;
            }

            ListarVeterinarios();
            Console.Write("ID del veterinario: ");
            if (!int.TryParse(Console.ReadLine(), out var veterinarioId))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var veterinario = Db.Veterinarios.Find(veterinarioId);
            if (veterinario == null)
            {
                Console.WriteLine("Veterinario no encontrado.");
                return;
            }

            Console.Write("Fecha de la atención (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out var fecha))
            {
                Console.WriteLine("Fecha inválida.");
                return;
            }

            Console.Write("Diagnóstico: ");
            var diagnostico = Console.ReadLine() ?? string.Empty;

            var atencion = new Atencion
            {
                Fecha = fecha,
                Diagnostico = diagnostico,
                MascotaId = mascotaId,
                VeterinarioId = veterinarioId
            };

            Db.Atenciones.Add(atencion);
            Db.SaveChanges();
            Console.WriteLine("Atención médica registrada exitosamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al registrar la atención: " + ex.Message);
        }
        Console.WriteLine("Presione Enter para continuar...");
        Console.ReadLine();
    }

    private static void ListarAtenciones()
    {
        try
        {
            var atenciones = Db.Atenciones
                .AsNoTracking()
                .Include(a => a.Mascota)
                .Include(a => a.Veterinario)
                .OrderByDescending(a => a.Fecha)
                .ToList();

            if (atenciones.Any())
            {
                Console.WriteLine("\n--- Lista de Atenciones Médicas ---");
                foreach (var a in atenciones)
                {
                    Console.WriteLine($"{a.Id}. {a.Fecha:yyyy-MM-dd} - {a.Mascota?.Nombre} - Dr. {a.Veterinario?.Apellidos}");
                    Console.WriteLine($"   Diagnóstico: {a.Diagnostico}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No hay atenciones médicas registradas.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al listar atenciones: " + ex.Message);
        }
        Console.WriteLine("Presione Enter para continuar...");
        Console.ReadLine();
    }

    private static void EditarAtencion()
    {
        try
        {
            ListarAtenciones();
            Console.Write("ID de la atención a editar: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var atencion = Db.Atenciones.Find(id);
            if (atencion == null)
            {
                Console.WriteLine("Atención no encontrada.");
                return;
            }

            Console.Write($"Diagnóstico actual: {atencion.Diagnostico}. Nuevo: ");
            var nuevoDiagnostico = Console.ReadLine();
            if (!string.IsNullOrEmpty(nuevoDiagnostico))
                atencion.Diagnostico = nuevoDiagnostico;

            Db.SaveChanges();
            Console.WriteLine("Atención actualizada exitosamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al editar atención: " + ex.Message);
        }
        Console.WriteLine("Presione Enter para continuar...");
        Console.ReadLine();
    }

    private static void EliminarAtencion()
    {
        try
        {
            ListarAtenciones();
            Console.Write("ID de la atención a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var atencion = Db.Atenciones.Find(id);
            if (atencion == null)
            {
                Console.WriteLine("Atención no encontrada.");
                return;
            }

            Console.Write($"¿Está seguro de eliminar la atención del {atencion.Fecha:yyyy-MM-dd}? (s/n): ");
            var confirmacion = Console.ReadLine();
            if (confirmacion?.ToLower() == "s")
            {
                Db.Atenciones.Remove(atencion);
                Db.SaveChanges();
                Console.WriteLine("Atención eliminada exitosamente.");
            }
            else
            {
                Console.WriteLine("Operación cancelada.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al eliminar atención: " + ex.Message);
        }
        Console.WriteLine("Presione Enter para continuar...");
        Console.ReadLine();
    }

    // --- Historial Médico ---
    private static void MostrarHistorialMedico()
    {
        try
        {
            ListarMascotas();
            Console.Write("ID de la mascota para ver historial: ");
            if (!int.TryParse(Console.ReadLine(), out var mascotaId))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var mascota = Db.Mascotas
                .AsNoTracking()
                .Include(m => m.Cliente)
                .FirstOrDefault(m => m.Id == mascotaId);

            if (mascota == null)
            {
                Console.WriteLine("Mascota no encontrada.");
                return;
            }

            var atenciones = Db.Atenciones
                .AsNoTracking()
                .Include(a => a.Veterinario)
                .Where(a => a.MascotaId == mascotaId)
                .OrderByDescending(a => a.Fecha)
                .ToList();

            Console.WriteLine($"\n--- Historial Médico de {mascota.Nombre} ---");
            Console.WriteLine($"Dueño: {mascota.Cliente?.Nombres} {mascota.Cliente?.Apellidos}");
            Console.WriteLine($"Especie: {mascota.Especie} - Raza: {mascota.Raza}");

            if (atenciones.Any())
            {
                Console.WriteLine("\nAtenciones médicas:");
                foreach (var a in atenciones)
                {
                    Console.WriteLine($"\nFecha: {a.Fecha:yyyy-MM-dd}");
                    Console.WriteLine($"Veterinario: Dr. {a.Veterinario?.Apellidos}");
                    Console.WriteLine($"Diagnóstico: {a.Diagnostico}");
                }
            }
            else
            {
                Console.WriteLine("No hay atenciones médicas registradas para esta mascota.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al mostrar historial médico: " + ex.Message);
        }
        Console.WriteLine("\nPresione Enter para continuar...");
        Console.ReadLine();
    }

    // --- Consultas Avanzadas ---
    private static void MenuConsultasAvanzadas()
    {
        while (true)
        {
            Console.WriteLine("\n-- Consultas Avanzadas --");
            Console.WriteLine("1. Mascotas de un cliente");
            Console.WriteLine("2. Veterinario con más atenciones");
            Console.WriteLine("3. Especie más atendida");
            Console.WriteLine("4. Cliente con más mascotas");
            Console.WriteLine("0. Volver");
            Console.Write("Opción: ");
            var op = Console.ReadLine();
            switch (op)
            {
                case "1": ConsultarMascotasPorCliente(); break;
                case "2": ConsultarVeterinarioMasAtendido(); break;
                case "3": ConsultarEspecieMasAtendida(); break;
                case "4": ConsultarClienteConMasMascotas(); break;
                case "0": return;
                default: 
                    Console.WriteLine("Opción no válida. Presione Enter para continuar...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private static void ConsultarMascotasPorCliente()
    {
        try
        {
            ListarClientes();
            Console.Write("ID del cliente: ");
            if (!int.TryParse(Console.ReadLine(), out var clienteId))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var mascotas = Db.Mascotas
                .AsNoTracking()
                .Where(m => m.ClienteId == clienteId)
                .ToList();

            if (mascotas.Any())
            {
                Console.WriteLine($"\n--- Mascotas del cliente ID {clienteId} ---");
                foreach (var m in mascotas)
                {
                    var edad = m.FechaNacimiento.HasValue ? DateTime.Now.Year - m.FechaNacimiento.Value.Year : 0;
                    Console.WriteLine($"{m.Nombre} ({m.Especie} - {m.Raza}) - {edad} años");
                }
            }
            else
            {
                Console.WriteLine("El cliente no tiene mascotas registradas.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error en la consulta: " + ex.Message);
        }
        Console.WriteLine("\nPresione Enter para continuar...");
        Console.ReadLine();
    }

    private static void ConsultarVeterinarioMasAtendido()
    {
        try
        {
            var veterinarioMasAtendido = Db.Atenciones
                .AsNoTracking()
                .Include(a => a.Veterinario)
                .GroupBy(a => a.VeterinarioId)
                .Select(g => new
                {
                    VeterinarioId = g.Key,
                    CantidadAtenciones = g.Count(),
                    Veterinario = g.First().Veterinario
                })
                .OrderByDescending(x => x.CantidadAtenciones)
                .FirstOrDefault();

            if (veterinarioMasAtendido != null)
            {
                Console.WriteLine($"\n--- Veterinario con más atenciones ---");
                Console.WriteLine($"Dr. {veterinarioMasAtendido.Veterinario?.Apellidos} {veterinarioMasAtendido.Veterinario?.Nombres}");
                Console.WriteLine($"Especialidad: {veterinarioMasAtendido.Veterinario?.Especialidad}");
                Console.WriteLine($"Total de atenciones: {veterinarioMasAtendido.CantidadAtenciones}");
            }
            else
            {
                Console.WriteLine("No hay atenciones registradas.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error en la consulta: " + ex.Message);
        }
        Console.WriteLine("\nPresione Enter para continuar...");
        Console.ReadLine();
    }

    private static void ConsultarEspecieMasAtendida()
    {
        try
        {
            var especieMasAtendida = Db.Atenciones
                .AsNoTracking()
                .Include(a => a.Mascota)
                .GroupBy(a => a.Mascota!.Especie)
                .Select(g => new
                {
                    Especie = g.Key,
                    CantidadAtenciones = g.Count()
                })
                .OrderByDescending(x => x.CantidadAtenciones)
                .FirstOrDefault();

            if (especieMasAtendida != null)
            {
                Console.WriteLine($"\n--- Especie más atendida ---");
                Console.WriteLine($"Especie: {especieMasAtendida.Especie}");
                Console.WriteLine($"Total de atenciones: {especieMasAtendida.CantidadAtenciones}");
            }
            else
            {
                Console.WriteLine("No hay atenciones registradas.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error en la consulta: " + ex.Message);
        }
        Console.WriteLine("\nPresione Enter para continuar...");
        Console.ReadLine();
    }

    private static void ConsultarClienteConMasMascotas()
    {
        try
        {
            var clienteConMasMascotas = Db.Mascotas
                .AsNoTracking()
                .Include(m => m.Cliente)
                .GroupBy(m => m.ClienteId)
                .Select(g => new
                {
                    ClienteId = g.Key,
                    CantidadMascotas = g.Count(),
                    Cliente = g.First().Cliente
                })
                .OrderByDescending(x => x.CantidadMascotas)
                .FirstOrDefault();

            if (clienteConMasMascotas != null)
            {
                Console.WriteLine($"\n--- Cliente con más mascotas ---");
                Console.WriteLine($"{clienteConMasMascotas.Cliente?.Apellidos}, {clienteConMasMascotas.Cliente?.Nombres}");
                Console.WriteLine($"Email: {clienteConMasMascotas.Cliente?.Email}");
                Console.WriteLine($"Total de mascotas: {clienteConMasMascotas.CantidadMascotas}");
            }
            else
            {
                Console.WriteLine("No hay mascotas registradas.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error en la consulta: " + ex.Message);
        }
        Console.WriteLine("\nPresione Enter para continuar...");
        Console.ReadLine();
    }
}