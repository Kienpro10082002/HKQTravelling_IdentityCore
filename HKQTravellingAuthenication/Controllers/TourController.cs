//using database
#region Khai báo
using HKQTravellingAuthenication.Extension;
using HKQTravellingAuthenication.Data;
using HKQTravellingAuthenication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MailKit.Net.Smtp;
using MimeKit;
using X.PagedList;
using System.Linq;
using System.Collections.Generic;
using HKQTravellingAuthenication.Models.Tour;
using HKQTravellingAuthenication.Areas.Identity.Controllers;
using Microsoft.AspNetCore.Identity;

#endregion


namespace HKQTravelling.Controllers
{
    public class TourController : Controller
    {
        #region Recourse
        //khai báo hàm app 
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccountController> _logger;

        public TourController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger,
            ApplicationDbContext db,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            IPagedList<Tours> objTourList = _db.tours.ToPagedList(pageNumber, pageSize);
            var startLocations = _db.startLocations.ToList();
            var endLocations = _db.endLocations.ToList();
            var tourImages = _db.tourImages.ToList();
            var types = _db.tourTypes.GroupBy(t => t.typeName).Select(g => g.First()).ToList();

            var tourImageUrls = new List<string>();
            foreach (var tour in objTourList)
            {
                var image = tourImages.FirstOrDefault(ti => ti.TourId == tour.TourId);
                if (image != null)
                {
                    tourImageUrls.Add(image.ImageUrl);
                }
            }

            ViewBag.StartLocations = new SelectList(startLocations, "StartLocationId", "StartLocationName");
            ViewBag.EndLocations = new SelectList(endLocations, "EndLocationId", "EndLocationName");
            ViewBag.TypeName = new SelectList(types, "typeID", "typeName");

            ViewBag.TourImages = tourImageUrls;
            return View(objTourList);
        }
        #endregion

        #region DetailTour
        [HttpGet]

        public IActionResult DetailTour(long id, IFormCollection f)
        {
            var soluongTreEm = f["soluongTreEm"].ToString();
            var soluongNguoiLon = f["soluongNguoiLon"].ToString();
            var tourImages = _db.tourImages.Where(t => t.TourId == id).ToList();
            var detail = _db.tours.FirstOrDefault(t => t.TourId == id);
            var tourDays = _db.tourDays.Where(t => t.TourId == id).OrderBy(t => t.DayNumber).ToList();
            if (tourImages == null && detail == null)
            {
                return NotFound();
            }
            var imageUrls = tourImages.Select(t => t.ImageUrl).ToList();


            ViewBag.Detail = detail;
            ViewBag.ImageUrls = imageUrls;
            ViewBag.TourDays = tourDays;

            return View();
        }
        #endregion

        #region BookingTour
            [HttpPost]
            public async Task<IActionResult> DetailTour(IFormCollection f)
            {
                var tourId = f["tourId"].ToString();
                var numAdults = f["nguoilon"].ToString();
                var numKids = f["treem"].ToString();
                var price = f["tongcong"].ToString();
                var roundedPrice = Math.Round(double.Parse(price));
                var dbtourId = Convert.ToInt32(tourId);
                var getUserInfo = await _userManager.GetUserAsync(HttpContext.User);
                var bookingUserId = getUserInfo.Id.ToString();
                // Lưu tourId và userId vào phiên
    /*            HttpContext.Session.SetString("bookingUserId", bookingUserId);*/
                HttpContext.Session.SetInt32("tourId", dbtourId);
                /*            HttpContext.Session.SetInt32("bookingId", dbbookingId);*/
                if (string.IsNullOrEmpty(numAdults))
                {
                    ViewData["checking_numAdults"] = "Chưa chọn số người đi!";
                    return View();
                }
                else if (string.IsNullOrEmpty(numAdults))
                {
                    ViewData["checking_numKids"] = "Chưa chọn số trẻ em!";
                    return View();
                }
                else
                {
                    var booking = new Bookings
                    {
                        BookingDate = DateTime.Now,
                        TourId = long.Parse(tourId),
                        NumAdults = int.Parse(numAdults),
                        NumKids = int.Parse(numKids),
                        PriceAdults = roundedPrice,
                        PriceKids = null,
                        PriceToddlers = null,
                        UserId = bookingUserId,
                        NumToddlers = null
                    };
                    _db.bookings.Add(booking);
                    await _db.SaveChangesAsync();
                    var dbbookingId = Convert.ToInt32(booking.BookingId);
                    HttpContext.Session.SetInt32("bookingId", dbbookingId);
                    return RedirectToAction("Payments", "Tour");
                }
            }

            #endregion

        #region Payment
        [HttpGet]
        public async Task<IActionResult> Payments()
        {
            // Lấy dữ liệu từ phiên
            var getUserInfo = await _userManager.GetUserAsync(HttpContext.User);
            var bookingUserId = getUserInfo.Id.ToString();
            var tourId = HttpContext.Session.GetInt32("tourId");
            var bookingId = HttpContext.Session.GetInt32("bookingId");
            var name = getUserInfo.NormalizedUserName;
            var email = getUserInfo.Email;

            // Chuyển đổi dữ liệu từ phiên
            long? dbTourId = 0;
            if (tourId != null)
            {
                dbTourId = Convert.ToInt64(tourId);
            }

            long? dbBookingId = 0;
            if (bookingId != null)
            {
                dbBookingId = Convert.ToInt64(bookingId);
            }

            var booking = _db.bookings.FirstOrDefault(t => t.BookingId == dbBookingId && t.TourId == tourId);
            ViewBag.Booking = booking;
            ViewBag.Name = name;
            ViewBag.Email = email;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Payments(IFormCollection f)
        {
            // Lấy dữ liệu từ phiên
            var bookingId = f["BookingId"].ToString();
            var totalPrice = f["TotalMoney"].ToString();

            var payment = new Payments
            {
                PaymentDate = DateTime.Now,
                BookingId = long.Parse(bookingId),
                TotalPrices = long.Parse(totalPrice),
                Amount = 1
            };
            _db.payments.Add(payment);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Tour");
        }

        #endregion

        #region Sorted by price
        [HttpGet]
        public IActionResult GetToursSortedByPriceAsc(int? page)
        {
            int pageSize = 1000;
            int pageNumber = (page ?? 1);
            IPagedList<Tours> tours = _db.tours.OrderBy(t => t.Price).ToPagedList(pageNumber, pageSize);
            var tourImages = _db.tourImages.ToList();

            var tourImageUrls = new List<string>();
            string startLocationName = "";
            string endLocationName = "";
            foreach (var tour in tours)
            {
                var image = tourImages.FirstOrDefault(ti => ti.TourId == tour.TourId);
                if (image != null)
                {
                    tourImageUrls.Add(image.ImageUrl);
                }
                var firstTour = tours.First();
                var startLocation = _db.startLocations.FirstOrDefault(sl => sl.StartLocationId == firstTour.StartLocationId);
                if (startLocation != null)
                {
                    startLocationName = startLocation.StartLocationName;
                }
                var endLocation = _db.endLocations.FirstOrDefault(sl => sl.EndLocationId == firstTour.EndLocationId);
                if (endLocation != null)
                {
                    endLocationName = endLocation.EndLocationName;
                }
            }

            ViewBag.StartLocationNames = startLocationName;
            ViewBag.EndLocationNames = endLocationName;
            ViewBag.TourImages = tourImageUrls;
            return PartialView("_ToursPricePartial", tours);
        }
        [HttpGet]
        public IActionResult GetToursSortedByPriceDesc(int? page)
        {
            int pageSize = 1000;
            int pageNumber = (page ?? 1);
            IPagedList<Tours> tours = _db.tours.OrderByDescending(t => t.Price).ToPagedList(pageNumber, pageSize);
            var tourImages = _db.tourImages.ToList();

            var tourImageUrls = new List<string>();
            string startLocationName = "";
            string endLocationName = "";
            foreach (var tour in tours)
            {
                var image = tourImages.FirstOrDefault(ti => ti.TourId == tour.TourId);
                if (image != null)
                {
                    tourImageUrls.Add(image.ImageUrl);
                }
                var firstTour = tours.First();
                var startLocation = _db.startLocations.FirstOrDefault(sl => sl.StartLocationId == firstTour.StartLocationId);
                if (startLocation != null)
                {
                    startLocationName = startLocation.StartLocationName;
                }
                var endLocation = _db.endLocations.FirstOrDefault(sl => sl.EndLocationId == firstTour.EndLocationId);
                if (endLocation != null)
                {
                    endLocationName = endLocation.EndLocationName;
                }
            }
            ViewBag.StartLocationNames = startLocationName;
            ViewBag.EndLocationNames = endLocationName;
            ViewBag.TourImages = tourImageUrls;
            return PartialView("_ToursPricePartial", tours);
        }

        #endregion

        #region Search by Location
        [HttpGet]
        public IActionResult GetToursByStartLocation(int? startLocationId, int? page)
        {
            int pageSize = 1000;
            int pageNumber = (page ?? 1);
            IPagedList<Tours> TourList = _db.tours.ToPagedList(pageNumber, pageSize);
            var tourImages = _db.tourImages.ToList();

            var tourImageUrls = new List<string>();
            string startLocationName = "";
            string endLocationName = "";
            foreach (var tour in TourList)
            {
                var image = tourImages.FirstOrDefault(ti => ti.TourId == tour.TourId);
                if (image != null)
                {
                    tourImageUrls.Add(image.ImageUrl);
                }
                var firstTour = TourList.First();
                var startLocation = _db.startLocations.FirstOrDefault(sl => sl.StartLocationId == firstTour.StartLocationId);
                if (startLocation != null)
                {
                    startLocationName = startLocation.StartLocationName;
                }
                var endLocation = _db.endLocations.FirstOrDefault(sl => sl.EndLocationId == firstTour.EndLocationId);
                if (endLocation != null)
                {
                    endLocationName = endLocation.EndLocationName;
                }
            }
            if (startLocationId.HasValue)
            {
                TourList = _db.tours.Where(t => t.StartLocationId == startLocationId).ToPagedList(pageNumber, pageSize);
            }
            else
            {
                TourList = _db.tours.ToPagedList(pageNumber, pageSize);
            }
            ViewBag.StartLocationNames = startLocationName;
            ViewBag.EndLocationNames = endLocationName;
            ViewBag.TourImages = tourImageUrls;
            return PartialView("_ToursPricePartial", TourList);
        }

        [HttpGet]
        public IActionResult GetToursByEndLocation(int? endLocationId, int? page)
        {
            int pageSize = 1000;
            int pageNumber = (page ?? 1);
            IPagedList<Tours> TourList = _db.tours.ToPagedList(pageNumber, pageSize);
            var tourImages = _db.tourImages.ToList();

            var tourImageUrls = new List<string>();
            string startLocationName = "";
            string endLocationName = "";
            foreach (var tour in TourList)
            {
                var image = tourImages.FirstOrDefault(ti => ti.TourId == tour.TourId);
                if (image != null)
                {
                    tourImageUrls.Add(image.ImageUrl);
                }
                var firstTour = TourList.First();
                var startLocation = _db.startLocations.FirstOrDefault(sl => sl.StartLocationId == firstTour.StartLocationId);
                if (startLocation != null)
                {
                    startLocationName = startLocation.StartLocationName;
                }
                var endLocation = _db.endLocations.FirstOrDefault(sl => sl.EndLocationId == firstTour.EndLocationId);
                if (endLocation != null)
                {
                    endLocationName = endLocation.EndLocationName;
                }
            }
            if (endLocationId.HasValue)
            {
                TourList = _db.tours.Where(t => t.EndLocationId == endLocationId).ToPagedList(pageNumber, pageSize);
            }
            else
            {
                TourList = _db.tours.ToPagedList(pageNumber, pageSize);
            }
            ViewBag.StartLocationNames = startLocationName;
            ViewBag.EndLocationNames = endLocationName;
            ViewBag.TourImages = tourImageUrls;
            return PartialView("_ToursPricePartial", TourList);
        }
        #endregion

        #region Search by type
        [HttpGet]
        public IActionResult GetToursByType(string typeName, int? page)
        {
            int pageSize = 1000;
            int pageNumber = (page ?? 1);
            IPagedList<Tours> TourList = _db.tours.ToPagedList(pageNumber, pageSize);
            var tourImages = _db.tourImages.ToList();

            var tourImageUrls = new List<string>();
            string startLocationName = "";
            string endLocationName = "";
            foreach (var tour in TourList)
            {
                var image = tourImages.FirstOrDefault(ti => ti.TourId == tour.TourId);
                if (image != null)
                {
                    tourImageUrls.Add(image.ImageUrl);
                }
                var firstTour = TourList.First();
                var startLocation = _db.startLocations.FirstOrDefault(sl => sl.StartLocationId == firstTour.StartLocationId);
                if (startLocation != null)
                {
                    startLocationName = startLocation.StartLocationName;
                }
                var endLocation = _db.endLocations.FirstOrDefault(sl => sl.EndLocationId == firstTour.EndLocationId);
                if (endLocation != null)
                {
                    endLocationName = endLocation.EndLocationName;
                }
            }
            if (!string.IsNullOrEmpty(typeName))
            {
                var tourIDs = _db.tourTypes.Where(tt => tt.typeName == typeName).Select(tt => tt.TourId).ToList();
                TourList = _db.tours.Where(t => tourIDs.Contains(t.TourId)).ToPagedList(pageNumber, pageSize);
            }
            else
            {
                TourList = _db.tours.ToPagedList(pageNumber, pageSize);
            }

            ViewBag.StartLocationNames = startLocationName;
            ViewBag.EndLocationNames = endLocationName;
            ViewBag.TourImages = tourImageUrls;
            return PartialView("_ToursPricePartial", TourList);
            
        }
        #endregion
    }
}
