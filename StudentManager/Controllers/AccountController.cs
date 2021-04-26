using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManager.ViewModels;
using System.Threading.Tasks;

namespace StudentManager.Controllers
{
    public class AccountController : Controller
    {
		private readonly SignInManager<IdentityUser> _signInManager;

		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		: base()
		{
			_signInManager = signInManager;
		}

		[HttpGet]
		public IActionResult Login()
        {
			return View();
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel viewmodel)
        {
			if (!ModelState.IsValid)
				return View("Login", viewmodel);

			var signInResult = await _signInManager.PasswordSignInAsync(viewmodel.UserName, viewmodel.Password, false, false);

			if(!signInResult.Succeeded)
            {
				ModelState.AddModelError("","Username or password is wrong");
				return View("Login", viewmodel);
            }

			return RedirectToAction("Index", "Students");
        }
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Students");
		}
	}
}
