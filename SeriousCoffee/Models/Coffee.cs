using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeriousCoffee.Models
{
    public class Coffee : IEquatable<Coffee>
    {
        public Coffee()
        {
            
        }

        public Coffee(int Id, String Name, IDictionary<Ingredient, int> Ingredients)
        {
            this.Id = Id;
            this.Name = Name;
            this.Ingredients = Ingredients;
        }

        public int Id { get; set; }
        public String Name { get; set; }
        [JsonIgnore]
        public IDictionary<Ingredient, int> Ingredients { get; set; }

        public override int GetHashCode()
        {
            return Id;
        }

        public bool Equals(Coffee other)
        {
            return other != null && other.Id == this.Id;
        }

        public override bool Equals(object other)
        {
            return Equals(other as Coffee);
        }
    }
}
