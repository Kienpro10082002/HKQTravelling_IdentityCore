using HKQTravellingAuthenication.Areas.Tour.Extension;
using HKQTravellingAuthenication.Areas.Tour.Models;
using HKQTravellingAuthenication.Data;
using HKQTravellingAuthenication.Models.Tour;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace HKQTravellingAuthenication.Areas.Tour.Controllers
{
    [Area("Tour")]
    [Route("tour-manager")]
    public class ToursAdministratorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ToursAdministratorController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var dbTours = await _context.tours.Include(t => t.discounts).Include(t => t.endLocations).Include(t => t.startLocations).ToListAsync();
            return View(dbTours);
        }

        [HttpGet]
        [Route("details/{id}")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.tours == null)
            {
                return NotFound();
            }

            var tour = await _context.tours.Include(t => t.discounts)
                .Include(t => t.startLocations)
                .Include(t => t.endLocations)
                .FirstOrDefaultAsync(t => t.TourId == id);
            var tourImages = await _context.tourImages.Where(t => t.TourId == id).ToListAsync();
            var tourDays = await _context.tourDays.Where(t => t.TourId == id).ToListAsync();
            if (tour == null)
            {
                return NotFound();
            }
            else if (tourImages == null)
            {
                return NotFound();
            }
            else if (tourDays == null)
            {
                return NotFound();
            }
            else
            {
                var tourExtraViewModel = new TourExtraViewModel()
                {
                    Tours = tour,
                    TourImagesList = tourImages,
                    TourDaysList = tourDays
                };
                return View(tourExtraViewModel);
            }
        }

        // GET: Tour/ToursAdministrator/edit/5
        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.tours == null)
            {
                return NotFound();
            }

            var tours = await _context.tours.Include(t => t.discounts)
                .Include(t => t.startLocations)
                .Include(t => t.endLocations)
                .FirstOrDefaultAsync(t => t.TourId == id);
            if (tours == null)
            {
                return NotFound();
            }

            ViewData["DiscountId"] = new SelectList(_context.discounts, "DiscountId", "DiscountId", tours.DiscountId);
            ViewData["EndLocationId"] = new SelectList(_context.endLocations, "EndLocationId", "EndLocationName", tours.EndLocationId);
            ViewData["StartLocationId"] = new SelectList(_context.startLocations, "StartLocationId", "StartLocationName", tours.StartLocationId);
            return View(tours);
        }
    }
}
