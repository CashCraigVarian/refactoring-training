using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class Tusc
    {
        const int EXIT = 7; // menu option to exit application

        public static void Start(List<User> users, List<Product> products)
        {
            ShowWelcome();

            Login:

            String name = PromptInput("Enter Username:");

            bool validUser = false;
            if (!string.IsNullOrEmpty(name))
            {
                validUser = ValidateUserName(users, name);

                if (validUser)
                {
                    string password = PromptInput("Enter Password:");

                    bool validPassword = false;
                    validPassword = ValidateUserPassword(users, name, password);

                    if (validPassword == true)
                    {
                        AccessTusc(users, products, name, password);

                        return;
                    }
                    else
                    {
                        ShowMessage(ConsoleColor.Red, "You entered an invalid password.");

                        goto Login;
                    }
                }
                else
                {
                    ShowMessage(ConsoleColor.Red, "You entered an invalid user.");

                    goto Login;
                }
            }

            PromptInput("Press Enter key to exit");
        }

        private static void AccessTusc(List<User> users, List<Product> products, String name, string password)
        {
            ShowMessage(ConsoleColor.Green, "Login successful! Welcome " + name + "!");

            double balance = ShowRemainingBalance(users, name, password);

            while (true)
            {
                ShowProductList(products);

                int num = Convert.ToInt32(PromptInput("Enter a number:")) - 1;

                if (num == EXIT)
                {
                    ExitTusc(users, products, name, password, balance);

                    return;
                }
                else
                {
                    balance = PurchaseProduct(products[num], balance);
                }
            }
        }

        private static void ExitTusc(List<User> users, List<Product> products, String name, string password, double balance)
        {
            foreach (var user in users)
            {
                if (user.IsValid(name, password))
                {
                    user.Balance = balance;
                }
            }

            WriteNewBalance(users);

            WriteNewQuantities(products);

            PromptInput("Press Enter key to exit");
        }

        private static bool ValidateUserPassword(List<User> users, String name, string password)
        {
            for (int i = 0; i < 5; i++)
            {
                User user = users[i];

                if (user.IsValid(name, password))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool ValidateUserName(List<User> users, String name)
        {
            for (int i = 0; i < 5; i++)
            {
                User user = users[i];

                if (user.IsValid(name))
                {
                    return true;
                }
            }
            return false;
        }

        private static double ShowRemainingBalance(List<User> users, String name, string password)
        {
            double balance = 0;
            for (int i = 0; i < 5; i++)
            {
                User user = users[i];

                if (user.IsValid(name, password))
                {
                    balance = user.Balance;

                    user.ShowBalance();
                }
            }
            return balance;
        }

        private static double PurchaseProduct(Product product, double balance)
        {
            Console.WriteLine();
            Console.WriteLine("You want to buy: " + product.Name);
            Console.WriteLine("Your balance is " + balance.ToString("C"));

            int quantity = Convert.ToInt32(PromptInput("Enter amount to purchase:"));

            balance = ValidatePurchase(product, balance, quantity);

            return balance;
        }

        private static double ValidatePurchase(Product product, double balance, int quantity)
        {
            if (product.HasEnoughBalance(balance, quantity))
            {
                ShowMessage(ConsoleColor.Red, "You do not have enough money to buy that.");
            }

            else if (product.IsInStock(quantity))
            {
                ShowMessage(ConsoleColor.Red, "Sorry, " + product.Name + " is out of stock");
            }

            else if (quantity <= 0)
            {
                ShowMessage(ConsoleColor.Yellow, "Purchase cancelled");
            }

            else
            {
                balance = ProcessPurchase(product, balance, quantity);               
            }

            return balance;
        }

        private static double ProcessPurchase(Product product, double balance, int quantity)
        {
            balance = balance - product.Price * quantity;

            product.Quantity = product.Quantity - quantity;

            ShowReceipt(product, balance, quantity);

            return balance;
        }

        private static void ShowReceipt(Product product, double balance, int quantity)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("You bought " + quantity + " " + product.Name);
            Console.WriteLine("Your new balance is " + balance.ToString("C"));
            Console.ResetColor();
        }

        private static void ShowProductList(List<Product> products)
        {
            Console.WriteLine();
            Console.WriteLine("What would you like to buy?");

            for (int i = 0; i < 7; i++)
            {
                Product product = products[i];
                Console.WriteLine(i + 1 + ": " + product.Name + " (" + product.Price.ToString("C") + ")");
            }

            Console.WriteLine(products.Count + 1 + ": Exit");
        }

        private static string PromptInput(string message)
        {
            Console.WriteLine();
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        private static void ShowMessage(ConsoleColor color, string message)
        {
            Console.Clear();
            Console.ForegroundColor = color;
            Console.WriteLine();
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void WriteNewQuantities(List<Product> products)
        {
            string json2 = JsonConvert.SerializeObject(products, Formatting.Indented);
            File.WriteAllText(@"Data\Products.json", json2);
        }

        private static void WriteNewBalance(List<User> users)
        {
            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(@"Data\Users.json", json);
        }

        private static void ShowWelcome()
        {
            Console.WriteLine("Welcome to TUSC");
            Console.WriteLine("---------------");
        }
    }
}
