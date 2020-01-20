using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace SeriousCoffee.Models
{
    public class Ingredient : IEquatable<Ingredient>
    {

        public Ingredient(int Id, String Name, int UnitsPerContainer)
        {
            this.Id = Id;
            this.Name = Name;
            this.UnitsPerContainer = UnitsPerContainer;
        }

        public int Id { get; set; }
        public String Name { get; set; }
        public int UnitsPerContainer { get; set; }

        public override int GetHashCode()
        {
            return Id;
        }

        public bool Equals(Ingredient other)
        {
            return other != null && other.Id == this.Id;
        }

        public override bool Equals(object other)
        {
            return Equals(other as Ingredient);
        }
    }
}
