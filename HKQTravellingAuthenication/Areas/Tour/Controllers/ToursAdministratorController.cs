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
            var dbTours = await _context.tours.Include(t => t.discounts).Include(t => t.endLocations).Include(t => t.startLocations).Include(t => t.tourTypes).ToListAsync();
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

            var tours = await _context.tours.Include(t => t.discounts)
                .Include(t => t.startLocations)
                .Include(t => t.endLocations)
                .Include(t => t.tourTypes)
                .FirstOrDefaultAsync(t => t.TourId == id);
            var tourImages = await _context.tourImages.Where(t => t.TourId == id).ToListAsync();
            if (tours == null)
            {
                return NotFound();
            }
            else
            {
                var tourExtraViewModel = new TourExtraViewModel()
                {
                    Tours = tours,
                    TourImagesList = tourImages
                };
                return View(tourExtraViewModel);
            }
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            ViewData["DiscountId"] = new SelectList(_context.discounts, "DiscountId", "DiscountName");
            ViewData["EndLocationId"] = new SelectList(_context.endLocations, "EndLocationId", "EndLocationName");
            ViewData["StartLocationId"] = new SelectList(_context.startLocations, "StartLocationId", "StartLocationName");
            ViewData["TourTypeId"] = new SelectList(_context.tourTypes, "TourTypeId", "TourTypeName");
            return View();
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            string tourName = collection["TourName"].ToString();
            string price = collection["Price"].ToString();
            string startDate = collection["StartDate"].ToString();
            string endDate = collection["EndDate"].ToString();
            string discountId = collection["DiscountId"].ToString();
            string startLocationId = collection["StartLocationId"].ToString();
            string endLocationId = collection["EndLocationId"].ToString();
            string tourTypeId = collection["TourTypeId"].ToString();
            string remaining = collection["Remaining"].ToString();
            string priceInclude = collection["PriceInlude"].ToString();
            string priceNotInclude = collection["PriceNotInclude"].ToString();
            string surcharge = collection["Surcharge"].ToString();
            string cancelChange = collection["CancelChange"].ToString();
            string note = collection["Note"].ToString();
            var tours = new Tours();
            if (ModelState.IsValid)
            {
                tours = new Tours()
                {
                    TourName = tourName,
                    Price = int.Parse(price),
                    Status = 1,
                    StartDate = Convert.ToDateTime(startDate),
                    EndDate = Convert.ToDateTime(endDate),
                    StartLocationId = int.Parse(startLocationId),
                    EndLocationId = int.Parse(endLocationId),
                    DiscountId = int.Parse(discountId),
                    TourTypeId = int.Parse(tourTypeId),
                    Remaining = int.Parse(remaining),
                    PriceInclude = priceInclude,
                    PriceNotInclude = priceNotInclude,
                    Surcharge = surcharge,
                    CancelChange = cancelChange,
                    Note = note
                };
                _context.Add(tours);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiscountId"] = new SelectList(_context.discounts, "DiscountId", "DiscountId", tours.DiscountId);
            ViewData["EndLocationId"] = new SelectList(_context.endLocations, "EndLocationId", "EndLocationName", tours.EndLocationId);
            ViewData["StartLocationId"] = new SelectList(_context.startLocations, "StartLocationId", "StartLocationName", tours.StartLocationId);
            ViewData["TourTypeId"] = new SelectList(_context.tourTypes, "TourTypeId", "TourTypeName");
            return View(tours);
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
                .Include(t => t.tourTypes)
                .FirstOrDefaultAsync(t => t.TourId == id);
            var tourImages = await _context.tourImages.Where(t => t.TourId == id).ToListAsync();
            if (tours == null)
            {
                return NotFound();
            }
            else
            {
                var tourExtraViewModel = new TourExtraViewModel()
                {
                    Tours = tours,
                    TourImagesList = tourImages
                };
                ViewData["DiscountId"] = new SelectList(_context.discounts, "DiscountId", "DiscountName", tourExtraViewModel.Tours.DiscountId);
                ViewData["EndLocationId"] = new SelectList(_context.endLocations, "EndLocationId", "EndLocationName", tourExtraViewModel.Tours.EndLocationId);
                ViewData["StartLocationId"] = new SelectList(_context.startLocations, "StartLocationId", "StartLocationName", tourExtraViewModel.Tours.StartLocationId);
                ViewData["TourTypeId"] = new SelectList(_context.tourTypes, "TourTypeId", "TourTypeName", tourExtraViewModel.Tours.TourTypeId);
                return View(tourExtraViewModel);
            }
        }

        // GET: Admin/ToursAdministrator/delete/5
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.tours == null)
            {
                return NotFound();
            }

            var tours = await _context.tours
                .Include(t => t.discounts)
                .Include(t => t.endLocations)
                .Include(t => t.startLocations)
                .Include(t => t.tourTypes)
                .FirstOrDefaultAsync(m => m.TourId == id);
            if (tours == null)
            {
                return NotFound();
            }

            return View(tours);
        }

        // POST: Admin/ToursAdministrator/delete/5
        [HttpPost]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.tours == null)
            {
                return Problem("Entity set 'ApplicationDBContext.tours'  is null.");
            }
            var tours = await _context.tours.FindAsync(id);
            if (tours != null)
            {
                _context.tours.Remove(tours);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToursExists(long id)
        {
            return (_context.tours?.Any(e => e.TourId == id)).GetValueOrDefault();
        }
    }
}
