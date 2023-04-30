using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoolEvents.Data;
using CoolEvents.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.Extensions.Logging;

namespace CoolEvents.Controllers
{
    
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EventsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
              return _context.Events != null ? 
                          View(await _context.Events.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Events'  is null.");
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            var usersWithEvent = from u in _context.ApplicationUsers
                                 where u.UserEvents.Any(ue => ue.EventId == @event.Id)
                                 select u;
            DetailsEventModel detailsEventModel = new DetailsEventModel
            {
                Id = @event.Id,
                Date = @event.Date,
                Name = @event.Name,
                Description = @event.Description,
                FilePath = @event.FilePath,
                ApplicationUsers = usersWithEvent.ToList()
        };
            if (@event == null)
            {
                return NotFound();
            }

            return View(detailsEventModel);
        }

        // GET: Events/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventModel eventModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    string username = User.Identity.Name;
                    ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName.Equals(username));
                    string photoName = UploadPhoto("Events", eventModel.Image, _webHostEnvironment);

                    Event @event = new Event()
                    {
                        Name = eventModel.Name,
                        Description = eventModel.Description,
                        FilePath = photoName,
                        Date = eventModel.Date
                    };
                    
                    _context.Events.Add(@event);
                    _context.SaveChanges();
                    TempData["successMessage"] = "Event created successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Model data is not valid";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var events = await _context.Events.FindAsync(id);
            if (events == null)
            {
                return NotFound();
            }
            EditEventModel eventModel = new EditEventModel()
            {
                Id = events.Id,
                Name = events.Name,
                Description = events.Description,
                FilePath = events.FilePath,
                Date = events.Date
            };
            return View(eventModel);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,FilePath,Date")] EditEventModel editEventModel)
        {
            if (id != editEventModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Event events = await _context.Events.FindAsync(id);
                    events.Name = editEventModel.Name;
                    events.Description = editEventModel.Description;
                    events.Date = editEventModel.Date;
                    _context.Update(events);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(editEventModel.Id))
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
            return View(editEventModel);
        }

        // GET: Events/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Events == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Events'  is null.");
            }
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                DeleteImage("Events", @event.FilePath, _context, _webHostEnvironment);
                _context.Events.Remove(@event);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
          return (_context.Events?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public static string UploadPhoto(string imageType, IFormFile formFile, IWebHostEnvironment webHostEnvironment)
        {
            string uniqueFileName = null;

            if (formFile != null)
            {
                string photosFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images", imageType);
                uniqueFileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
                string photoPathAndName = Path.Combine(photosFolder, uniqueFileName);

                Directory.CreateDirectory(photosFolder);
                using FileStream fileStream = new(photoPathAndName, FileMode.Create);

                formFile.CopyTo(fileStream);
            }

            return uniqueFileName;
        }
        public static async Task<bool> DeleteImage(string imageType, string imageUrl, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            string profilePhotoFileName = Path.Combine(webHostEnvironment.WebRootPath, "Images", imageType, imageUrl);


            if (System.IO.File.Exists(profilePhotoFileName))
            {
                System.IO.File.Delete(profilePhotoFileName);
                return true;
            }

            return false;
        }
    }
}
