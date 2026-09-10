// Level 2
// Add a class for ProductManager
// Sort products from lowest to highest price
// Use methods for AddProduct(), ShowProducts() and CalculateTotal()


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ProductManagementSystem
{
    // Class representing a product
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

    // Class ProductManager - handles the collection, sorting, display, and calculations for products
    public class ProductManager
    {
        private readonly List<Product> _products = new List<Product>();
        private readonly CultureInfo _culture = CultureInfo.InvariantCulture;

        //Method to add products to the list dynamically
        public void AddProduct(Product product)
        {
            _products.Add(product);
        }    
    
        public decimal CalculateTotal()
        {
            return _products.Sum(p => p.Price);
        }

        //Method to display products in a formatted table, sorted by lowest to highest price, 
        // and show the total amount
        public void ShowProducts()
        {
 
            if (_products.Count == 0)
            {
                Console.WriteLine("No products were entered.");
                return;
            }

            Console.WriteLine(new string('-', 60));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{"Category",-20} {"Name",-25} {"Price",12}");
            Console.ResetColor();


            // Sort from lowest to highest price
            var sortedProducts = _products.OrderBy(p => p.Price);

            foreach (var item in sortedProducts)
            {
                Console.WriteLine($"{item.Category,-20} {item.Name,-25} {item.Price,12:N2}");
            }

            decimal totalAmount = CalculateTotal();
            Console.WriteLine(new string('-', 60));
            // Aligned under the Name column
            Console.WriteLine($"{"",-20} {"Total Amount:",-25} {totalAmount,12:N2}");
        }
    }
    class Program
    {
   
        static void Main(string[] args)
        {
            ProductManager manager = new ProductManager();

            Console.Clear();
            Console.WriteLine("To enter a new product - follow the steps | To quit - enter: 'Q'\n");

            while (true)
            {
                // 1. Product Category
                string category = ReadInput("Enter a Category: ", out bool quitCategory);
                if (quitCategory) break;

                // 2. Product Name
                string name = ReadInput("Enter Product Name: ", out bool quitName);
                if (quitName) break;

                // 3. Product Price
                decimal price = ReadValidPrice("Enter Product Price: ");

                // Add product via the manager
                manager.AddProduct(new Product(category, name, price));

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Product added successfully!\n");
                Console.ResetColor();
            }

            // Display all entered products
            manager.ShowProducts();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static string ReadInput(string prompt, out bool isQuit)
        {
            isQuit = false;
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim();

                if (string.Equals(input, "q", StringComparison.OrdinalIgnoreCase))
                {
                    isQuit = true;
                    return string.Empty;
                }

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Input cannot be empty. Please try again.");
                Console.ResetColor();
            }
        }

        private static decimal ReadValidPrice(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim();

                // Ensure correct parsing of decimal numbers
                if (decimal.TryParse(input, out decimal price) && price >= 0)
                {
                    return price;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid price. Please enter a valid positive number.");
                Console.ResetColor();
            }
        }
    }
}
