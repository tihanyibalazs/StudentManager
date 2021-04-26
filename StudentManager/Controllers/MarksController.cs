using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManager.Models;
using StudentManager.Services;
using StudentManager.ViewModels;
using System.Threading.Tasks;

namespace StudentManager.Controllers
{
    public class MarksController : Controller
    {
        protected readonly IStudentManagerService _studentManagerService;

        public MarksController(IStudentManagerService studentManagerService)
        {
            _studentManagerService = studentManagerService;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Create(int id)
        {
            TempData["Id"] = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(CreateMarkViewModel viewmodel)
        {
            viewmodel.StudentId = (int)TempData.Peek("Id");
            if (!ModelState.IsValid)
                return View(viewmodel);

            if (!await _studentManagerService.AddNewMarkAsync(viewmodel))
                return View("Error", new ErrorViewModel("Creation of the new mark failed."));

            return RedirectToAction("Index", "Students");
        }
    }
}
