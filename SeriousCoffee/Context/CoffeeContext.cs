using SeriousCoffee.Controllers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriousCoffee.Models
{

    public interface ICoffeeContext
    {

        CoffeeOrder OrderCoffee(int CoffeeId);

        public IList<Coffee> CoffeeOptions { get; }
        public IList<CoffeeOrder> CoffeeOrders { get; }
        public IList<Ingredient> Ingredients { get; }
        public IDictionary<Ingredient, int> Stocks { get; }
        double GetStocks(int id);
        void AddStock(AddStockCommand AddStockCommand);
    }

    public class CoffeeContext : ICoffeeContext
    {

        public CoffeeContext(IDictionary<Ingredient, int> Stocks, IList<Ingredient> Ingredients, IList<Coffee> CoffeeOptions, IList<CoffeeOrder> CoffeeOrders)
        {
            this.Stocks = Stocks;
            this.Ingredients = Ingredients;
            this.CoffeeOptions = CoffeeOptions;
            this.CoffeeOrders = CoffeeOrders;
        }

        public IDictionary<Ingredient, int> Stocks { get; set; }
        public IList<Ingredient> Ingredients { get; set; }
        public IList<Coffee> CoffeeOptions { get; set; }
        public IList<CoffeeOrder> CoffeeOrders { get; set; }

        public CoffeeOrder OrderCoffee(int CoffeeId)
        {
            var Coffee = FindCoffee(CoffeeId);

            foreach (KeyValuePair<Ingredient, int> IngredientsNeeded in Coffee.Ingredients)
            {
                var IngredientStocksNumber = Stocks[IngredientsNeeded.Key];
                if (IngredientStocksNumber < IngredientsNeeded.Value) throw new Exception("Cannot make this coffee, not enough " + IngredientsNeeded.Key.Name);
            }

            foreach (KeyValuePair<Ingredient, int> IngredientsNeeded in Coffee.Ingredients)
            {
                Stocks[IngredientsNeeded.Key] = Stocks[IngredientsNeeded.Key] - IngredientsNeeded.Value;
            }

            var CoffeeOrder = new CoffeeOrder(Guid.NewGuid().ToString(), DateTime.Now, Coffee.Name);
            CoffeeOrders.Add(CoffeeOrder);
            return CoffeeOrder;
        }

        public double GetStocks(int IngredientId)
        {

            foreach (KeyValuePair<Ingredient, int> Stock in Stocks)
            {
                if (Stock.Key.Id == IngredientId)
                    return Stock.Value / Stock.Key.UnitsPerContainer + (Stock.Value % Stock.Key.UnitsPerContainer == 0 ? 0 : ((double)(Stock.Value % Stock.Key.UnitsPerContainer) / Stock.Key.UnitsPerContainer));
            }

            return 0;
        }

        public void AddStock(AddStockCommand AddStockCommand)
        {
            foreach (KeyValuePair<Ingredient, int> Stock in Stocks)
            {
                if (Stock.Key.Id == AddStockCommand.IngredientId)
                {
                    Stocks[Stock.Key] += AddStockCommand.ContainerCount * Stock.Key.UnitsPerContainer;
                    return;
                }
            }
            Ingredient Ingredient = FindIngredient(AddStockCommand.IngredientId);
            Stocks[Ingredient] = AddStockCommand.ContainerCount * Ingredient.UnitsPerContainer;
        }

        private Coffee FindCoffee(int Id)
        {
            foreach (Coffee CoffeeItem in CoffeeOptions)
                if (CoffeeItem.Id == Id) return CoffeeItem;
            throw new Exception("Coffee not found with Id" + Id) ;
        }
        private Ingredient FindIngredient(int Id)
        {
            foreach (Ingredient IngredientItem in Ingredients)
                if (IngredientItem.Id == Id) return IngredientItem;
            throw new Exception("Ingredient not found with Id" + Id);
        }
    }
}
