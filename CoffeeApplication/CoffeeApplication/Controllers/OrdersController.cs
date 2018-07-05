using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeApplication.Data;
using CoffeeApplication.Models;

namespace CoffeeApplication.Controllers
{
    public class OrdersController : Controller
    {
        private readonly CoffeeContext _context;

        public OrdersController(CoffeeContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CustomerSortParm"] = String.IsNullOrEmpty(sortOrder) ? "cust_desc" : "";
            ViewData["TimeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "time_desc" : "";
            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var orders = from o in _context.Orders
                         select o;

            if (!String.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(s => s.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    orders = orders.OrderBy(o => o.Name);
                    break;
                case "cust_desc":
                    orders = orders.OrderBy(o => o.Customer);
                    break;
                case "time_desc":
                    orders = orders.OrderByDescending(o => o.TimeOrdered);
                    break;
                default:
                    orders = orders.OrderBy(o => o.TimeOrdered);
                    break;
            }

            ViewBag.TotalDoubleAmericano = orders.Where(c => c.Name == "Double Americano").Count();
            ViewBag.TotalSweetLatte = orders.Where(c => c.Name == "Sweet Latte").Count();
            ViewBag.TotalFlatWhite = orders.Where(c => c.Name == "Flat White").Count();

            int pageSize = 10;
            return View(await PaginatedList<Order>.CreateAsync(orders.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Customer,TimeOrdered")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }
    }
}
