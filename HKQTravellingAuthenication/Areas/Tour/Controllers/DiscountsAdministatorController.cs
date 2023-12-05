using HKQTravellingAuthenication.Areas.Tour.Extension;
using HKQTravellingAuthenication.Data;
using HKQTravellingAuthenication.Models.Tour;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HKQTravellingAuthenication.Areas.Tour.Controllers
{
    [Area("Tour")]
    [Route("discount-tour-manager")]
    public class DiscountsAdministatorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiscountsAdministatorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tour/discount-tour-manager/index
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            return _context.discounts != null ?
                        View(await _context.discounts.ToListAsync()) :
                        Problem("Entity set 'ApplicationDBContext.discounts'  is null.");
        }

        // GET: Tour/discount-tour-manager/details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _context.discounts == null)
            {
                return NotFound();
            }

            var discounts = await _context.discounts
                .FirstOrDefaultAsync(m => m.DiscountId == id);
            if (discounts == null)
            {
                return NotFound();
            }

            return View(discounts);
        }

        // GET: Tour/discount-tour-manager/create
        [HttpGet]
        [Route("create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tour/discount-tour-manager/create
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            string discountPercentage = collection["DiscountPercentage"].ToString();
            string discountName = collection["DiscountName"].ToString();
            string discountDateStart = collection["DIS_DATE_START"].ToString();
            string discountDateEnd = collection["DIS_DATE_END"].ToString();
            if (discountName == null)
            {
                ViewData["validation_message"] = "Tên giảm giá không được để trống!";
                return View();
            } 
            else if (discountPercentage == null)
            {
                ViewData["validation_message"] = "Phần trăm giảm không được để trống!";
                return View();
            }
            else if (discountDateStart == null)
            {
                ViewData["validation_message"] = "Ngày bắt đầu giảm không được để trống!";
                return View();
            }
            else if (discountDateEnd == null)
            {
                ViewData["validation_message"] = "Ngày kết thúc giảm không được để trống!";
                return View();
            }
            else if (checkingDiscounts.checkDiscountsName(_context, discountName))
            {
                ViewData["validation_message"] = "Tên giảm giá này đã tồn tại!";
                return View();
            }
            else
            {
                var dbDiscounts = new Discounts()
                {
                    DiscountName = discountName,
                    DiscountPercentage = Double.Parse(discountPercentage),
                    DiscountDateStart = Convert.ToDateTime(discountDateStart),
                    DiscountDateEnd = Convert.ToDateTime(discountDateEnd)
                };
                _context.Add(dbDiscounts);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

        // GET: DiscountsAdministatorController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DiscountsAdministatorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DiscountsAdministatorController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DiscountsAdministatorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
