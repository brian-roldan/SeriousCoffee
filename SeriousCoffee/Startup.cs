using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeriousCoffee.Models;

namespace SeriousCoffee
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var IngredientCoffeeBeans = new Ingredient(0, "Coffee Beans", 15);
            var IngredientMilk = new Ingredient(1, "Milk", 15);
            var IngredientSugar = new Ingredient(2, "Sugar", 15);

            //initial stocks
            var Stocks = new Dictionary<Ingredient, int>
            {
                { IngredientCoffeeBeans, 3 * IngredientCoffeeBeans.UnitsPerContainer },
                { IngredientMilk, 3 * IngredientMilk.UnitsPerContainer },
                { IngredientSugar, 3 * IngredientSugar.UnitsPerContainer }
            };

            var Ingredients = new List<Ingredient> { IngredientCoffeeBeans, IngredientMilk, IngredientSugar };

            //Coffee Options
            var CoffeeDoubleAmericano = new Coffee(0, "Double Americano", 
                new Dictionary<Ingredient, int>
                {
                    { IngredientCoffeeBeans, 3 }
                })
            ;
            var CoffeeSweetLatte = new Coffee(1, "Sweet Latte",
                new Dictionary<Ingredient, int>
                {
                    { IngredientCoffeeBeans, 2 },
                    { IngredientSugar, 5 },
                    { IngredientMilk, 3 },
                })
            ;
            var CoffeeFlatWhite = new Coffee(2, "Flat White",
                new Dictionary<Ingredient, int>
                {
                    { IngredientCoffeeBeans, 2 },
                    { IngredientSugar, 1 },
                    { IngredientMilk, 4 },
                })
            ;

            var CoffeeOptions = new List<Coffee> { CoffeeDoubleAmericano, CoffeeSweetLatte, CoffeeFlatWhite };

            var CoffeeContext = new CoffeeContext(Stocks, Ingredients, CoffeeOptions, new List<CoffeeOrder>());

            services.AddSingleton<ICoffeeContext>(CoffeeContext);
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
                    
           
        }
    }
}
