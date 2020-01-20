using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeriousCoffee.Models;

namespace SeriousCoffee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly ICoffeeContext CoffeeContext;

        public IngredientController(ICoffeeContext CoffeeContext)
        {
            this.CoffeeContext = CoffeeContext;
        }

        // GET: api/Ingredient
        [HttpGet]
        public IEnumerable<Ingredient> Get()
        {
            return CoffeeContext.Ingredients;
        }
        
    }
}
