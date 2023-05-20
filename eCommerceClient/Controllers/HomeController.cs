using Common;
using eCommerceClient.DataAccess;
using eCommerceClient.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace eCommerceClient.Controllers
{
    public class HomeController : Controller
    {
        UsersService _usersService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, UsersService usersService)
        {
            _logger = logger;
            _usersService = usersService;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Notifications()
        {
            string email = User.FindFirst(ClaimTypes.Email)?.Value;
            List<Notifications> notifications = await _usersService.GetAllNotifications(email);
            return View(notifications);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(Users user)
        {
            user.userId = "0";
            user.Credit = 60;
            if ((user.Email == "MCASTSUPERUSER") && (user.Password == "McastDistributed")) {
                user.isAdmin = true;
             }
            else
            {
                user.isAdmin = false;
            }
            await _usersService.SignUp(user);
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.UserData, $"{user.prefCurrency}/{user.prefAddress}") };
            var identity = new ClaimsIdentity(claims, "MyCookieAuth");

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Logout()
        {
            // Perform the sign-out process
            await HttpContext.SignOutAsync();

            // Redirect the user to a desired page after logging out
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserCredentials userCredentials)
        {
            Users user = await _usersService.SignIn(userCredentials);
            var claims = new List<Claim> { 
                new Claim(ClaimTypes.Name, user.Name), 
                new Claim(ClaimTypes.Email, user.Email), 
                new Claim(ClaimTypes.UserData, $"{user.prefCurrency}/{user.prefAddress}") };
            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            string email = User.FindFirst(ClaimTypes.Email)?.Value;
            //var actualMessage = info.Split("/")[0];
            Users user = await _usersService.GetUser(email);
            return View(user);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}