﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    [Serializable]
    public class User
    {
        [JsonProperty("Username")]
        public string Name;
        [JsonProperty("Password")]
        public string Password;
        [JsonProperty("Balance")]
        public double Balance;

        public void ShowBalance()
        {
            Console.WriteLine();
            Console.WriteLine("Your balance is " + this.Balance.ToString("C"));
        }
    }   
}
