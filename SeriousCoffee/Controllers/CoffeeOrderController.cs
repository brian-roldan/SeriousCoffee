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
    public class CoffeeOrderController : ControllerBase
    {

        private readonly ICoffeeContext CoffeeContext;

        public CoffeeOrderController(ICoffeeContext CoffeeContext)
        {
            this.CoffeeContext = CoffeeContext;
        }

        // GET: api/CoffeeOrder
        [HttpGet]
        public IEnumerable<CoffeeOrder> Get()
        {
            return CoffeeContext.CoffeeOrders;
        }

        // POST: api/CoffeeOrder
        [HttpPost]
        public void Post(Coffee Coffee)
        {
            CoffeeContext.OrderCoffee(Coffee.Id);
        }
    }
}
