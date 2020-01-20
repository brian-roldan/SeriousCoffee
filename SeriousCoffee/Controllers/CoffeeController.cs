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
    public class CoffeeController : ControllerBase
    {

        private readonly ICoffeeContext CoffeeContext;

        public CoffeeController(ICoffeeContext CoffeeContext)
        {
            this.CoffeeContext = CoffeeContext;
        }

        // GET: api/Coffee
        [HttpGet]
        public IEnumerable<Coffee> Get()
        {
            return CoffeeContext.CoffeeOptions;
        }
        
    }
}
