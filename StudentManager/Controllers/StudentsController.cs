using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManager.Models;
using StudentManager.Services;
using StudentManager.ViewModels;

namespace StudentManager.Controllers
{
    public class StudentsController : Controller
    {
        protected readonly IStudentManagerService _studentManagerService;

        public StudentsController(IStudentManagerService studentManagerService)
        {
            _studentManagerService = studentManagerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 0)
        {
            double maxPage = await _studentManagerService.AllStudentNumberAsync();
            maxPage = maxPage / 10;
            maxPage = Math.Floor(maxPage);

            if (pageNumber > maxPage)
                pageNumber = (int)maxPage;

            if (pageNumber < 0)
                pageNumber = 0;
            
            TempData["PageNumber"] = pageNumber;

            return View(await _studentManagerService.PagedStudentsAsync(pageNumber,10));
        }

        [HttpGet]
        public async Task<IActionResult> Statistics(int pageNumber = 0)
        {
            double maxPage = await _studentManagerService.AllStatisticsNumberAsync();
            maxPage = maxPage / 10;
            maxPage = Math.Floor(maxPage);

            if (pageNumber > maxPage)
                pageNumber = (int)maxPage;

            if (pageNumber < 0)
                pageNumber = 0;

            TempData["PageNumber"] = pageNumber;

            return View(await _studentManagerService.PagedStudentStatisticsAsync(pageNumber,10));
        }

        [HttpGet]
        [Authorize(Roles ="admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(CreateStudentViewModel viewmodel)
        {
            if (!ModelState.IsValid)
                return View(viewmodel);

            if (!await _studentManagerService.AddNewStudentAsync(viewmodel))
                return View("Error", new ErrorViewModel("Creation of the new student failed"));

            return RedirectToAction("Index");
        }
    }
}
