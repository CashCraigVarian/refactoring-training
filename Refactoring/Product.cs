using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    [Serializable]
    public class Product
    {
        [JsonProperty("Name")]
        public string Name;
        [JsonProperty("Price")]
        public double Price;
        [JsonProperty("Quantity")]
        public int Quantity;

        public bool IsInStock(int quantity)
        {
            return this.Quantity <= quantity;
        }

        public bool HasEnoughBalance(double balance, int quantity)
        {
            return balance - this.Price * quantity < 0;
        }
    }
}
