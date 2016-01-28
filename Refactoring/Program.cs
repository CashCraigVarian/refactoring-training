using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Refactoring
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<User> users = LoadUsers();

            List<Product> products = LoadProducts();

            Tusc.Start(users, products);
        }

        private static List<Product> LoadProducts()
        {
            return JsonConvert.DeserializeObject<List<Product>>(File.ReadAllText(@"Data\Products.json"));
        }

        private static List<User> LoadUsers()
        {
            return JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(@"Data\Users.json"));
        }
    }
}
