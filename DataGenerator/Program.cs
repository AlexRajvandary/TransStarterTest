using DataGenerator.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DataGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Car Sales Data Generator ===");

            while (true)
            {
                try
                {
                    int ordersCount = ReadInt("Enter number of orders to generate (or 0 to exit): ", min: 0);
                    if (ordersCount == 0) break;

                    int years = ReadInt("Enter number of years to generate data for (1-10) (or 0 to exit): ", min: 0, max: 10);
                    if (years == 0) break;

                    var solutionDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
                                               .Parent?.Parent?.Parent?.Parent?.FullName; // подстраиваем под структуру bin/Debug/netX

                    if (solutionDir == null)
                        throw new Exception("Не удалось определить путь к решению");

                    var dbFolder = Path.Combine(solutionDir, "Database");

                    if (!Directory.Exists(dbFolder))
                        Directory.CreateDirectory(dbFolder);

                    var dbPath = Path.Combine(dbFolder, "sales.db");

                    var options = new DbContextOptionsBuilder<AppDbContext>()
                        .UseSqlite($"Data Source={dbPath}")
                        .Options;

                    using (var context = new AppDbContext(options))
                    {
                        context.Database.EnsureCreated();
                        var generator = new RandomDataService(context);
                        generator.Generate(ordersCount, years);
                    }

                    Console.WriteLine($"Successfully generated {ordersCount} orders for the last {years} years.\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}. Try again.\n");
                }
            }

            Console.WriteLine("Exiting program...");
        }

        static int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (input == null) continue;

                if (input.Trim().ToLower() == "exit") Environment.Exit(0);

                if (int.TryParse(input, out int value))
                {
                    if (value >= min && value <= max)
                        return value;
                }

                Console.WriteLine($"Invalid input. Please enter a number between {min} and {max} or 'exit' to quit.");
            }
        }
    }
}
