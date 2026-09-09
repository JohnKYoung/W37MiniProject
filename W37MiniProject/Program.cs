// Level 1 
// Store products in a list
// Add products dynamically
// Present all added products

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ProductManagementSystem
{
    // Model representing a product
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

    class Program
    {
        static void Main(string[] args)
        {
            List<Product> products = new List<Product>();

            //Console.WriteLine("=======================================");
            //Console.WriteLine("    PRODUCT LIST MANAGEMENT SYSTEM     ");
            //Console.WriteLine("=======================================\n");
            Console.WriteLine("To enter a new product - follow the steps | To quit - enter: 'Q'\n");

            while (true)
            {
                // 1. Product Category
                string category = ReadNonEmptyString("Enter a category: ");

                if (category.Equals("q", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                // 2. Product Name
                string name = ReadNonEmptyString("Enter Product Name: ");

                // 3. Product Price
                decimal price = ReadValidPrice("Enter Product Price: ");

                // Dynamically add to the list
                products.Add(new Product(category, name, price));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Product added successfully!\n");
                Console.ResetColor();
            }

            // Present all added products
            DisplayProducts(products);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        // Helper to ensure strings are not empty or whitespace
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

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Input cannot be empty. Please try again.");
                Console.ResetColor();
            }
        }

        // Helper to parse valid decimal values
        private static decimal ReadValidPrice(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim();

                if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price) && price >= 0)
                {
                    return price;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid price. Please enter a positive numerical value.");
                Console.ResetColor();
            }
        }

        // Formats and displays the product list
        private static void DisplayProducts(List<Product> products)
        {
        
            if (products.Count == 0)
            {
                Console.WriteLine("No products were entered.");
                return;
            }

            // Table header
            Console.WriteLine(new string('-', 61));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{"Category",-20}   {"Name",-25}   {"Price",-5}");
            Console.ResetColor();
            

            // Product rows sorted alphabetically by category, then by price
            var sortedProducts = products.OrderBy(p => p.Category).ThenBy(p => p.Price);


            foreach (var item in sortedProducts)
            {
                Console.WriteLine($"{item.Category,-20}   {item.Name,-25}   {item.Price,10:C2}");
            }

            // Total summary
            decimal totalAmount = products.Sum(p => p.Price);
            Console.WriteLine($"\n{"",-23}{"Total Amount:",-25}   {totalAmount,10:C2}");
            Console.WriteLine(new string('-', 61));
        }
    }
}
