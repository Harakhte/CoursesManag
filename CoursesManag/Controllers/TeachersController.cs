using CoursesManag.Models;
using CoursesManag.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesManag.Controllers
{

    [Route("teachers/[action]")]
    [ApiController]
    public class TeachersController : Controller
    {

        private readonly TeachersService _teachersService;

        public TeachersController(TeachersService teacherService)
        {
            _teachersService = teacherService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return View(_teachersService.Get());
        }

        [HttpGet("{id}", Name = "detail")]
        public IActionResult Detail(string id)
        {
            return View(_teachersService.Get(id));
        }

        [HttpGet]
        public IActionResult AddTeacher()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddTeacher([FromForm] Teachers teachers)
        {
            _teachersService.Create(teachers);
            return RedirectToAction("Get");
        }

        [HttpGet]
        public IActionResult UpdateTeacher(string id)
        {
            return View(_teachersService.Get(id));
        }

        [HttpPost]
        public IActionResult UpdateTeacher([FromForm] Teachers teachers)
        {
            _teachersService.Update(teachers);
            return RedirectToAction("Get");
        }
        [HttpDelete]
        public IActionResult DeleteTeacher(string id)
        {
            _teachersService.Delete(id);
            return RedirectToAction("Get");
        }
    }
}
