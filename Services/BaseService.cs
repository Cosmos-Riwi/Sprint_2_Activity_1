using Microsoft.EntityFrameworkCore;
using Sprint_2_Activity_1.Data;

namespace Sprint_2_Activity_1.Services
{
    public abstract class BaseService
    {
        protected readonly VeterinaryDbContext _context;

        protected BaseService(VeterinaryDbContext context)
        {
            _context = context;
        }

        protected void ShowMessage(string message, bool isError = false)
        {
            Console.ForegroundColor = isError ? ConsoleColor.Red : ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine("\nPresiona cualquier tecla para continuar...");
            Console.ReadKey();
        }

        protected void ClearScreen()
        {
            Console.Clear();
        }

        protected void ShowTitle(string title)
        {
            Console.WriteLine(title);
            Console.WriteLine(new string('-', title.Length));
            Console.WriteLine();
        }

        protected string ReadString(string message, bool isRequired = true)
        {
            string? value;
            do
            {
                Console.Write($"{message}: ");
                value = Console.ReadLine();
                if (isRequired && string.IsNullOrWhiteSpace(value))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Este campo es obligatorio.");
                    Console.ResetColor();
                }
            } while (isRequired && string.IsNullOrWhiteSpace(value));

            return value ?? string.Empty;
        }

        protected int ReadInt(string message, bool isRequired = true)
        {
            int value = 0;
            string? input;
            do
            {
                Console.Write($"{message}: ");
                input = Console.ReadLine();
                if (isRequired && string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Este campo es obligatorio.");
                    Console.ResetColor();
                    continue;
                }
                if (!int.TryParse(input, out value))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Por favor ingresa un número válido.");
                    Console.ResetColor();
                }
            } while (isRequired && (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out value)));

            return value;
        }

        protected decimal ReadDecimal(string message, bool isRequired = true)
        {
            decimal value = 0;
            string? input;
            do
            {
                Console.Write($"{message}: ");
                input = Console.ReadLine();
                if (isRequired && string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Este campo es obligatorio.");
                    Console.ResetColor();
                    continue;
                }
                if (!decimal.TryParse(input, out value))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Por favor ingresa un número decimal válido.");
                    Console.ResetColor();
                }
            } while (isRequired && (string.IsNullOrWhiteSpace(input) || !decimal.TryParse(input, out value)));

            return value;
        }

        protected DateTime ReadDate(string message, bool isRequired = true)
        {
            DateTime value = DateTime.MinValue;
            string? input;
            do
            {
                Console.Write($"{message} (dd/mm/yyyy): ");
                input = Console.ReadLine();
                if (isRequired && string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Este campo es obligatorio.");
                    Console.ResetColor();
                    continue;
                }
                if (!DateTime.TryParseExact(input, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out value))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Por favor ingresa una fecha válida en formato dd/mm/yyyy.");
                    Console.ResetColor();
                }
            } while (isRequired && (string.IsNullOrWhiteSpace(input) || !DateTime.TryParseExact(input, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out value)));

            return value;
        }
    }
}
