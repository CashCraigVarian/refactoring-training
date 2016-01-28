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
        public static void Start(List<User> users, List<Product> products)
        {
            ShowWelcome();

            // Login
            Login:
            bool loggedIn = false; // Is logged in?

            // Prompt for user input
            Console.WriteLine();
            Console.WriteLine("Enter Username:");
            string name = Console.ReadLine();

            // Validate Username
            bool valUsr = false; // Is valid user?
            if (!string.IsNullOrEmpty(name))
            {
                for (int i = 0; i < 5; i++)
                {
                    User user = users[i];
                    // Check that name matches
                    if (user.Name == name)
                    {
                        valUsr = true;
                    }
                }

                // if valid user
                if (valUsr)
                {
                    // Prompt for user input
                    Console.WriteLine("Enter Password:");
                    string pwd = Console.ReadLine();

                    // Validate Password
                    bool valPwd = false; // Is valid password?
                    for (int i = 0; i < 5; i++)
                    {
                        User user = users[i];

                        // Check that name and password match
                        if (user.Name == name && user.Password == pwd)
                        {
                            valPwd = true;
                        }
                    }

                    // if valid password
                    if (valPwd == true)
                    {
                        loggedIn = true;

                        ShowMessage(ConsoleColor.Green,"Login successful! Welcome " + name + "!");
                        
                        // Show remaining balance
                        double balance = 0;
                        for (int i = 0; i < 5; i++)
                        {
                            User user = users[i];

                            // Check that name and password match
                            if (user.Name == name && user.Password == pwd)
                            {
                                balance = user.Balance;

                                user.ShowBalance();
                            }
                        }

                        // Show product list
                        while (true)
                        {
                            // Prompt for user input
                            Console.WriteLine();
                            Console.WriteLine("What would you like to buy?");
                            for (int i = 0; i < 7; i++)
                            {
                                Product prod = products[i];
                                Console.WriteLine(i + 1 + ": " + prod.Name + " (" + prod.Price.ToString("C") + ")");
                            }
                            Console.WriteLine(products.Count + 1 + ": Exit");

                            // Prompt for user input
                            Console.WriteLine("Enter a number:");
                            string answer = Console.ReadLine();
                            int num = Convert.ToInt32(answer);
                            num = num - 1; /* Subtract 1 from number
                            num = num + 1 // Add 1 to number */

                            // Check if user entered number that equals product count
                            if (num == 7)
                            {
                                // Update balance
                                foreach (var user in users)
                                {
                                    // Check that name and password match
                                    if (user.Name == name && user.Password == pwd)
                                    {
                                        user.Balance = balance;
                                    }
                                }

                                WriteNewBalance(users);

                                WriteNewQuantities(products);

                                ShowClosingPrompt();
                                return;
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("You want to buy: " + products[num].Name);
                                Console.WriteLine("Your balance is " + balance.ToString("C"));

                                int quantity = PromptForQuantity();

                                // Check if balance - quantity * price is less than 0
                                if (balance - products[num].Price * quantity < 0)
                                {
                                    ShowMessage(ConsoleColor.Red, "You do not have enough money to buy that.");
                                    continue;
                                }

                                // Check if quantity is less than quantity
                                if (products[num].Quantity <= quantity)
                                {
                                    ShowMessage(ConsoleColor.Red, "Sorry, " + products[num].Name + " is out of stock");
                                    continue;
                                }

                                // Check if quantity is greater than zero
                                if (quantity > 0)
                                {
                                    // Balance = Balance - Price * Quantity
                                    balance = balance - products[num].Price * quantity;

                                    // Quanity = Quantity - Quantity
                                    products[num].Quantity = products[num].Quantity - quantity;

                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("You bought " + quantity + " " + products[num].Name);
                                    Console.WriteLine("Your new balance is " + balance.ToString("C"));
                                    Console.ResetColor();
                                }
                                else
                                {
                                    ShowMessage(ConsoleColor.Yellow, "Purchase cancelled");
                                }
                            }
                        }
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

            ShowClosingPrompt();
        }

        private static int PromptForQuantity()
        {
            Console.WriteLine("Enter amount to purchase:");
            string answer = Console.ReadLine();
            return Convert.ToInt32(answer);
        }

        private static void ShowClosingPrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press Enter key to exit");
            Console.ReadLine();
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
