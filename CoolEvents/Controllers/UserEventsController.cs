using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoolEvents.Data;
using CoolEvents.Models;

namespace CoolEvents.Controllers
{
    public class UserEventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserEventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserEvents
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserEvents.Include(u => u.Event).Include(u => u.User);
            return View(await applicationDbContext.ToListAsync());
        }

       

        // GET: UserEvents/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Description");
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: UserEvents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,EventId")] UserEvent userEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", userEvent.EventId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", userEvent.UserId);
            return View(userEvent);
        }

        
    }
}
