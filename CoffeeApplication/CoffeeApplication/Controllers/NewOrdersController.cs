using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoffeeApplication.Data;
using CoffeeApplication.Models;
using CoffeeApplication.Extensions.Alerts;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace CoffeeApplication.Controllers
{
    public class NewOrdersController : Controller
    {
        private readonly CoffeeContext _context;

        public NewOrdersController(CoffeeContext context)
        {
            _context = context;
        }

        // GET: NewOrders
        public IActionResult Index()
        {
            return View();
        }

        // POST: NewOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Customer,TimeOrdered")] Order order, string Name)
        {
            var coffeebean = (from c in _context.Ingredients where c.Name == "CoffeeBean" select c.Unit).FirstOrDefault();
            var sugar = (from c in _context.Ingredients where c.Name == "Sugar" select c.Unit).FirstOrDefault();
            var milk = (from c in _context.Ingredients where c.Name == "Milk" select c.Unit).FirstOrDefault();

            var bagcoffee = (from c in _context.Ingredients where c.Name == "CoffeeBean" select c.Container).FirstOrDefault();
            var packsugar = (from c in _context.Ingredients where c.Name == "Sugar" select c.Container).FirstOrDefault();
            var cartonmilk = (from c in _context.Ingredients where c.Name == "Milk" select c.Container).FirstOrDefault();

            switch (Name)
            {
                case "Double Americano":
                    if (coffeebean >=3)
                    {
                        coffeebean = coffeebean - 3;

                        _context.Database.ExecuteSqlCommand("UPDATE dbo.Ingredient SET Unit = " + coffeebean + " WHERE ID = 1");
                        _context.SaveChanges();
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index)).WithWarning("Order has failed due to insufficient unit of ingredient");
                    }
                    break;
                case "Sweet Latte":
                    if (coffeebean >= 2 & sugar >= 5 & milk >= 3)
                    {
                        coffeebean = coffeebean - 2;
                        sugar = sugar - 5;
                        milk = milk - 3;

                        _context.Database.ExecuteSqlCommand("UPDATE dbo.Ingredient SET Unit = " + coffeebean + " WHERE ID = 1");
                        _context.Database.ExecuteSqlCommand("UPDATE dbo.Ingredient SET Unit = " + sugar + " WHERE ID = 2");
                        _context.Database.ExecuteSqlCommand("UPDATE dbo.Ingredient SET Unit = " + milk + " WHERE ID = 3");
                        _context.SaveChanges();
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index)).WithWarning("Order has failed due to insufficient unit of ingredient");
                    }
                    break;
                case "Flat White":
                    if (coffeebean >= 2 & sugar >= 1 & milk >= 4)
                    {
                        coffeebean = coffeebean - 2;
                        sugar = sugar - 1;
                        milk = milk - 4;

                        _context.Database.ExecuteSqlCommand("UPDATE dbo.Ingredient SET Unit = " + coffeebean + " WHERE ID = 1");
                        _context.Database.ExecuteSqlCommand("UPDATE dbo.Ingredient SET Unit = " + sugar + " WHERE ID = 2");
                        _context.Database.ExecuteSqlCommand("UPDATE dbo.Ingredient SET Unit = " + milk + " WHERE ID = 3");
                        _context.SaveChanges();
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index)).WithWarning("Order has failed due to insufficient unit of ingredient");
                    }
                    break;
                default:
                    break;
            }

            var beanhigh = bagcoffee * 15;
            var beanlow= (bagcoffee - 1) * 15;

            var sugarhigh = packsugar * 15;
            var sugarlow = (packsugar - 1) * 15;

            var milkhigh = cartonmilk * 15;
            var milklow = (cartonmilk - 1) * 15;

            if (!(coffeebean > beanlow && coffeebean <= beanhigh))
            {
                bagcoffee--;
                _context.Database.ExecuteSqlCommand("UPDATE dbo.Ingredient SET Container = " + bagcoffee + " WHERE ID = 1");
                _context.SaveChanges();
            }

            if (!(sugar > sugarlow && sugar <= sugarhigh))
            {
                packsugar--;
                _context.Database.ExecuteSqlCommand("UPDATE dbo.Ingredient SET Container = " + packsugar + " WHERE ID = 2");
                _context.SaveChanges();
            }

            if (!(milk > milklow && milk <= milkhigh))
            {
                cartonmilk--;
                _context.Database.ExecuteSqlCommand("UPDATE dbo.Ingredient SET Container = " + cartonmilk + " WHERE ID = 3");
                _context.SaveChanges();
            }

            if (ModelState.IsValid)
            {                
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)).WithSuccess("Order has been successfully submitted");
            }
            return View(order);
        }
    }
}
