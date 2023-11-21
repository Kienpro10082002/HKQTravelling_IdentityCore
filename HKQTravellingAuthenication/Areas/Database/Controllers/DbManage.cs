using HKQTravellingAuthenication.Data;
using HKQTravellingAuthenication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HKQTravellingAuthenication.Areas.Database.Controllers
{
    [Area("Database")]
    [Authorize(Roles = RoleName.Administrator)]
    [Route("/database-manage/[action]")]
    public class DbManageController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbManageController(ApplicationDbContext dbcontext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DeleteDb()
        {
            return View();
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpPost]
        public async Task<IActionResult> DeleteDbAsync()
        {
            var success = await _dbcontext.Database.EnsureDeletedAsync();
            StatusMessage = success ? "Xóa Database thành công" : "Không xóa được Db";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Migrate()
        {
            await _dbcontext.Database.MigrateAsync();
            StatusMessage = "Cập nhật Database thành công";
            return RedirectToAction(nameof(Index));
        }

        //https://localhost:5126/database-manage/SeedData
        public async Task<IActionResult> SeedDataAsync()
        {
            //Create Roles
            var rolenames = typeof(RoleName).GetFields().ToList();
            foreach (var r in rolenames)
            {
                var rolename = (string)r.GetRawConstantValue();
                var rfound = await _roleManager.FindByNameAsync(rolename);
                if (rfound == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(rolename));
                }
            }

            //admin, pass=admin123, admin@example.com
            var useradmin = await _userManager.FindByEmailAsync("admin");
            if(useradmin == null) {
                useradmin = new AppUser(){
                    UserName = "admin",
                    Email = "hkqtravelling@gmail.com",
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(useradmin, "admin123");
                await _userManager.AddToRoleAsync(useradmin, RoleName.Administrator);
            }
            StatusMessage = "Vừa Seed DB";
            return RedirectToAction("Index");
        }
    }
}
