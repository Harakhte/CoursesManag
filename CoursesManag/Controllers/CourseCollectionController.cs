using CoursesManag.Models;
using CoursesManag.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesManag.Controllers
{
    [Route("courses/[action]")]
    [ApiController]
    public class CourseCollectionController : Controller
    {
        private readonly CourseCollectionService _courseCollectionService;
        private readonly TeachersService _teachersService;

        public CourseCollectionController(CourseCollectionService courseService, TeachersService teachersService)
        {
            _teachersService = teachersService;
            _courseCollectionService = courseService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            List<CourseCollection> response = new List<CourseCollection>();
            var lstTeacher = _teachersService.Get();
            var lstCourse = _courseCollectionService.Get();
            if(lstCourse.Count > 0)
            
            {
                foreach(var item in lstCourse)
                {
                    List<string> nameTeacher = new List<string>();
                    foreach(var nameId in item.TeacherID)
                    {
                        var teacherName = lstTeacher.FirstOrDefault(x => x.Id == nameId);
                        nameTeacher.Add(teacherName.Name);
                    }
                    item.TeacherID = nameTeacher;
                    response.Add(item);
                }
            }
            return View(response);
        }

        [HttpGet]
        public IActionResult CourseDetail(string id)
        {

            var detailCourse = _courseCollectionService.Get(id);
            var lstTeacher = _teachersService.Get();

            List<string> nameTeacher = new List<string>();
            foreach (var nameId in detailCourse.TeacherID)
            {
                var teacherName = lstTeacher.FirstOrDefault(x => x.Id == nameId);
                nameTeacher.Add(teacherName.Name);
            }
            detailCourse.TeacherID = nameTeacher;    
            return View(detailCourse);
        }

        [HttpGet]
        public IActionResult AddCourse()
        {
            var lstTeacher = _teachersService.Get();

            ViewBag.teachername = lstTeacher;
            return View();
        }

        [HttpPost]
        public IActionResult AddCourse([FromForm] CourseCollection course)
        {
            _courseCollectionService.Create(course);
            return RedirectToAction("Get");
        }
        [HttpGet]
        public IActionResult UpdateCourse(string id)
        {
            var lstTeacher = _teachersService.Get();

            ViewBag.teachername = lstTeacher;
            return View(_courseCollectionService.Get(id));
        }

        [HttpPost]
        public IActionResult UpdateCourse([FromForm] CourseCollection course)
        {
            _courseCollectionService.Update(course);
            return RedirectToAction("Get");
        }
        [HttpDelete]
        public IActionResult DeleteCourse(string id)
        {
            _courseCollectionService.Delete(id);
            return RedirectToAction("Get");
        }
    }
}
