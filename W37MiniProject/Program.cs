// Product list management system
// Level 3
// Add proper error handling for invalid inputs 
// Prevent entry of invalid prices
// Allow users to continue adding products after showing the list
//Use LINQ


using System;
using System.Collections.Generic;
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

        //Method to add products to the list dynamically
        public void AddProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            }

            _products.Add(product);
        }

        // Method to calculate the total value of all products
        public decimal CalculateTotal()
        {
            return _products.Sum(p => p.Price);
        }

        // Method to display products in a formatted table, sorted by lowest to highest price,
        //  and show the total amount
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
            

            // Sorted from lowest to highest price
            var sortedProducts = _products.OrderBy(p => p.Price);

            foreach (var item in sortedProducts)
            {
                Console.WriteLine($"{item.Category,-20} {item.Name,-25} {item.Price,12:N2}");
            }

            decimal totalAmount = CalculateTotal();

            // Aligned under the Name column
            Console.WriteLine($"\n{"",-20} {"Total Amount:",-25} {totalAmount,12:N2}");
            Console.WriteLine(new string('-', 60));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ProductManager manager = new ProductManager();

            //Clear the console at the start of the program
            Console.Clear();

            // Main loop to enter product details and display the list
            while (true)
            {
                Console.WriteLine("\nTo enter a new product - follow the steps | To quit - enter: 'Q'\n");

                bool addedAnyThisRound = false;

                while (true)
                {
                    Console.Write("Enter a Category: ");
                    string? category = Console.ReadLine()?.Trim();

                    if (string.Equals(category, "q", StringComparison.OrdinalIgnoreCase))
                    {
                        // If 'q' was entered immediately without adding new items, exit the whole program
                        if (!addedAnyThisRound)
                        {
                            return;
                        }

                        // Otherwise, break to show the updated table
                        break;
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

                // Show the current product list and total amount after each round of entries
                manager.ShowProducts();
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
                               decimal.TryParse(normalizedInput, System.Globalization.NumberStyles.Number,
                                                System.Globalization.CultureInfo.InvariantCulture, out price);

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