using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static SeriousCoffee.Startup;

namespace SeriousCoffee.Models
{
    public class CoffeeOrder
    {

        public CoffeeOrder(String Id, DateTime OrderDateTime, String CoffeeName)
        {
            this.Id = Id;
            this.OrderDateTime = OrderDateTime;
            this.CoffeeName = CoffeeName;
        }
        
        public String Id { get; set; }
        public DateTime OrderDateTime { get; set; }
        public String CoffeeName { get; set; }

        public string OrderDateTimeString => OrderDateTime.ToString("dd/MM/yyyy HH:mm");


    }
}
