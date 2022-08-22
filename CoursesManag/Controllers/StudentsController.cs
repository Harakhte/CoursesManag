using CoursesManag.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDBWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesManag.Controllers
{
    [Route("students/[action]")]
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly StudentsService _studentsService;

        public StudentsController(StudentsService studentsService)
        {
            _studentsService = studentsService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return View(_studentsService.Get());
        }

       [HttpGet]
        public IActionResult DetailStudent(string id)
        {
            return View(_studentsService.Get(id));
        }
        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddStudent([FromForm] Students students)
        {
            _studentsService.Create(students);
            return RedirectToAction("Get");
        }
        [HttpGet]
        public IActionResult UpdateStudent(string id)
        {
            return View(_studentsService.Get(id));
        }

        [HttpPost]
        public IActionResult UpdateStudent([FromForm] Students students)
        {
            _studentsService.Update(students);
            return RedirectToAction("Get");
        }
        [HttpDelete]
        public IActionResult DeleteStudent(string id)
        {
            _studentsService.Remove(id);
            return RedirectToAction("Get");
        }
    }
}
