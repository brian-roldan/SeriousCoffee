using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriousCoffee.Controllers.Commands
{
    public class AddStockCommand
    {

        public AddStockCommand() { }

        public int IngredientId { get; set; }
        public int ContainerCount { get; set; }

    }
}
