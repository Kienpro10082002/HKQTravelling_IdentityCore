using HKQTravellingAuthenication.Areas.DashBoard.Models;
using HKQTravellingAuthenication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HKQTravellingAuthenication.Areas.DashBoard.Controllers
{
    [Area("DashBoard")]
    [Route("/DashBoard")]
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
    public class DashBoardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashBoardController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("NumberOfBookings")]
        public async Task<IActionResult> NumberOfBookings()
        {
            int numberOfBookings = await _context.bookings.CountAsync();

            DashboardInfo dashboardInfo = new DashboardInfo
            {
                NumberOfBookings = numberOfBookings
            };

            return View(dashboardInfo);
        }
    }
}
