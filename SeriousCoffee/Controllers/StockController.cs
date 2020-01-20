using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeriousCoffee.Controllers.Commands;
using SeriousCoffee.Models;

namespace SeriousCoffee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ICoffeeContext CoffeeContext;

        public StockController(ICoffeeContext CoffeeContext)
        {
            this.CoffeeContext = CoffeeContext;
        }

        // GET: api/Stock/5
        [HttpGet("{id}", Name = "Get")]
        public double Get(int id)
        {
            return Math.Round(CoffeeContext.GetStocks(id), 2);
        }

        // POST: api/Stock
        [HttpPost]
        public void Post(AddStockCommand AddStockCommand)
        {
            CoffeeContext.AddStock(AddStockCommand);
        }
    }
}
