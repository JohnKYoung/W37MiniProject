// Product list management system
// Level 4
// - Allow users to search for products by name or category
// - Highlight the searched product or category in the displayed table


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ProductManagementSystem
{
    public class Product
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(string category, string name, decimal price)
        {
            Category = category;
            Name = name;
            Price = price;
        }
    }

    public class ProductManager
    {
        private readonly List<Product> _products = new List<Product>();

        public void AddProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");

            _products.Add(product);
        }

        public decimal CalculateTotal()
        {
            return _products.Sum(p => p.Price);
        }

        public void ShowProducts(string? filter = null)
        {
            // 1. If searching, check for matching products first
            if (!string.IsNullOrWhiteSpace(filter))
            {
                bool hasMatches = _products.Any(p =>
                    p.Name.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                    p.Category.Contains(filter, StringComparison.OrdinalIgnoreCase));

                if (!hasMatches)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nNo products were found.");
                    Console.ResetColor();
                    return;
                }
            }
            // 2. If not searching and inventory is completely empty
            else if (!_products.Any())
            {
                Console.WriteLine("\nNo products were entered.");
                return;
            }

            Console.WriteLine(new string('-', 60));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{"Category",-20} {"Name",-25} {"Price",12}");
            Console.ResetColor();

            // LINQ: Sort by price ascending
            var sortedProducts = _products.OrderBy(p => p.Price);

            foreach (var item in sortedProducts)
            {
                bool isMatch = !string.IsNullOrWhiteSpace(filter) &&
                               (item.Name.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                                item.Category.Contains(filter, StringComparison.OrdinalIgnoreCase));

                if (isMatch)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                }

                Console.WriteLine($"{item.Category,-20} {item.Name,-25} {item.Price,12:N2}");

                if (isMatch)
                {
                    Console.ResetColor();
                }
            }

            // LINQ: Calculate grand total
            decimal totalAmount = CalculateTotal();

            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"{"",-20} {"Total Amount:",-25} {totalAmount,12:N2}");
            Console.WriteLine(new string('-', 60));
        }
    }

    class Program
    {
static void Main(string[] args)
        {
            ProductManager manager = new ProductManager();
            Console.Clear();

            while (true)
            {
                Console.WriteLine("\nTo enter a new product - follow the steps | To search - enter: 'S' | To quit - enter: 'Q'\n");

                bool addedAnyThisRound = false;
                bool exitedSearch = false;

                while (true)
                {
                    Console.Write("Enter a Category: ");
                    string? category = Console.ReadLine()?.Trim();

                    // Quit logic
                    if (string.Equals(category, "q", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!addedAnyThisRound)
                        {
                            return; // Direct quit when requested right after table display
                        }
                        break; // Break input round and show updated table
                    }

                    // Search logic
                    if (string.Equals(category, "s", StringComparison.OrdinalIgnoreCase))
                    {
                        RunSearchLoop(manager);
                        exitedSearch = true;
                        break; // Break inner loop to return to outermost prompt
                    }

                    if (string.IsNullOrWhiteSpace(category))
                    {
                        DisplayError("Category cannot be empty. Please try again.\n");
                        continue;
                    }

                    string name = ReadNonEmptyString("Enter Product Name: ");
                    decimal price = ReadValidPrice("Enter Product Price: ");

                    manager.AddProduct(new Product(category, name, price));
                    addedAnyThisRound = true;

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Product added successfully!\n");
                    Console.ResetColor();
                }

                // If we exited search mode, loop back immediately to display the main prompt
                if (exitedSearch)
                {
                    continue;
                }

                // Otherwise, display full sorted inventory and grand total
                manager.ShowProducts();
            }
        }

        // Handles recurring search queries until the user exits search mode
        private static void RunSearchLoop(ProductManager manager)
        {
            Console.WriteLine("\n[SEARCH MODE - Type 'Q' to return to product entry]");

            while (true)
            {
                Console.Write("Enter search term (Product Name or Category): ");
                string? term = Console.ReadLine()?.Trim();

                if (string.Equals(term, "q", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Exiting search mode...\n");
                    break;
                }

                if (string.IsNullOrWhiteSpace(term))
                {
                    DisplayError("Search term cannot be empty. Please enter a value or 'Q' to quit.");
                    continue;
                }

                manager.ShowProducts(term);
                Console.WriteLine();
            }
        }

        private static string ReadNonEmptyString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }

                DisplayError("Input cannot be empty. Please try again.");
            }
        }

        private static decimal ReadValidPrice(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    DisplayError("Price cannot be empty. Please try again.");
                    continue;
                }

                string normalizedInput = input.Replace(',', '.');

                bool isValid = decimal.TryParse(input, out decimal price) ||
                               decimal.TryParse(normalizedInput, NumberStyles.Number,
                                                CultureInfo.InvariantCulture, out price);

                if (!isValid)
                {
                    DisplayError("Invalid price format. Enter numeric values only (e.g., 29.99).");
                    continue;
                }

                if (price < 0)
                {
                    DisplayError("Price cannot be negative. Please enter a value >= 0.00.");
                    continue;
                }

                if (price > 1_000_000_000m)
                {
                    DisplayError("Price is too large. Please enter a realistic amount.");
                    continue;
                }

                return price;
            }
        }

        private static void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}