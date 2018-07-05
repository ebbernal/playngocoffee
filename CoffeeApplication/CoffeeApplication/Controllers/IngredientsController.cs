using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeApplication.Data;
using CoffeeApplication.Models;
using System.Data.SqlClient;

namespace CoffeeApplication.Controllers
{
    public class IngredientsController : Controller
    {
        private readonly CoffeeContext _context;

        public IngredientsController(CoffeeContext context)
        {
            _context = context;
        }

        // GET: Ingredients
        public async Task<IActionResult> Index()
        {
            var coffeebean = (from c in _context.Ingredients where c.Name == "CoffeeBean" select c.Unit).FirstOrDefault();
            var sugar = (from c in _context.Ingredients where c.Name == "Sugar" select c.Unit).FirstOrDefault();
            var milk = (from c in _context.Ingredients where c.Name == "Milk" select c.Unit).FirstOrDefault();

            var beanbag = (from c in _context.Ingredients where c.Name == "CoffeeBean" select c.Container).FirstOrDefault();
            var sugarpack = (from c in _context.Ingredients where c.Name == "Sugar" select c.Container).FirstOrDefault();
            var milkcarton = (from c in _context.Ingredients where c.Name == "Milk" select c.Container).FirstOrDefault();

            ViewBag.CoffeeBean = coffeebean;
            ViewBag.Sugar = sugar;
            ViewBag.Milk = milk;

            ViewBag.BeanBag = beanbag;
            ViewBag.SugarPack = sugarpack;
            ViewBag.MilkCarton = milkcarton;

            return View(await _context.Ingredients.ToListAsync());
        }

        // GET: Ingredients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredient = await _context.Ingredients
                .SingleOrDefaultAsync(m => m.ID == id);
            if (ingredient == null)
            {
                return NotFound();
            }

            return View(ingredient);
        }

        // GET: Ingredients/Edit/5
        public async Task<IActionResult> Edit(int? id, int Container)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var ingredient = await _context.Ingredients.SingleOrDefaultAsync(m => m.ID == id);
            if (ingredient == null)
            {
                return NotFound();
            }
            return View(ingredient);
        }

        // POST: Ingredients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Container,Unit")] Ingredient ingredient, int Container)
        {
            var cont = (from c in _context.Ingredients where c.ID == id select c.Container).FirstOrDefault();
            var unit = (from c in _context.Ingredients where c.ID == id select c.Unit).FirstOrDefault();

            if (id != ingredient.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Database.ExecuteSqlCommand("UPDATE dbo.Ingredient SET Container = @container WHERE ID = @contID",
               new SqlParameter("@container", cont + Container),
               new SqlParameter("@contID", id));
                    _context.Database.ExecuteSqlCommand("UPDATE dbo.Ingredient SET Unit = @unit WHERE ID = @unitID",
                        new SqlParameter("@unit", unit + Container*15),
                        new SqlParameter("@unitID", id));
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredientExists(ingredient.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ingredient);
        }

        private bool IngredientExists(int id)
        {
            return _context.Ingredients.Any(e => e.ID == id);
        }
    }
}
