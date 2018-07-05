using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeApplication.Models;

namespace CoffeeApplication.Data
{
    public class DbInitializer
    {
        public static void Initialize(CoffeeContext context)
        {
            context.Database.EnsureCreated();

            if (context.Ingredients.Any())
            {
                return;   // DB has been seeded
            }

            var ingredient = new Ingredient[]
            {
            new Ingredient{Name="CoffeeBean",Container=3, Unit=45},
            new Ingredient{Name="Sugar",Container=3, Unit=45},
            new Ingredient{Name="Milk",Container=3, Unit=45},
            };
            foreach (Ingredient i in ingredient)
            {
                context.Ingredients.Add(i);
            }
            context.SaveChanges();
        }
    }
}
