using CoursesManag.Models;
using CoursesManag.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesManag.Controllers
{
    [Route("classes/[action]")]
    [ApiController]
    public class ClassesController : Controller
    {
        private readonly ClassesService _classesService;
        private readonly CourseCollectionService _courseCollectionService;
        private readonly TeachersService _teachersService;
        private readonly StudentsService _studentService;

        public ClassesController(ClassesService classesService, CourseCollectionService courseCollectionService, TeachersService teachersService, StudentsService studentService)
        {
            _classesService = classesService;
            _courseCollectionService = courseCollectionService;
            _studentService = studentService;
            _teachersService = teachersService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            List<Classes> response = new List<Classes>();

            var lstClass = _classesService.Get();
            var lstTeacher = _teachersService.Get();
            var lstCourse = _courseCollectionService.Get();
            var lstStudent = _studentService.Get();

            if (lstClass.Count > 0)
            {
                foreach (var item in lstClass)
                {
                    var courseName = lstCourse.FirstOrDefault(x => x.Id == item.CourseID);
                    item.CourseID = courseName.CourseName;

                    List<string> nameTeacher = new List<string>();
                    foreach (var nameId in item.TeacherID)
                    {
                        var teacherName = lstTeacher.FirstOrDefault(x => x.Id == nameId);
                        if(teacherName != null)
                        nameTeacher.Add(teacherName.Name);

                    }
                    item.TeacherID = nameTeacher;

                    List<string> nameStudent = new List<string>();
                    foreach (var nameID in item.StudentID)
                    {
                        var studentName = lstStudent.FirstOrDefault(y => y.Id == nameID);
                        nameStudent.Add(studentName.Name);
                    }
                    item.StudentID = nameStudent;
                    response.Add(item);
                }
            }
            return View(response);
        }
        [HttpGet]
        public IActionResult AddClass()
        {
            var lstTeacher = _teachersService.Get();
            var lstStudent = _studentService.Get();
            var lstCourse = _courseCollectionService.Get();

            ViewBag.studentname = lstStudent;
            ViewBag.teachername = lstTeacher;
            ViewBag.coursename = lstCourse;
            return View();
        }

        [HttpPost]
        public IActionResult AddClass([FromForm] Classes classes)
        {
            _classesService.Create(classes);
            return RedirectToAction("Get");
        }
        [HttpGet]
        public IActionResult ClassDetail(string id)
        {

            var detailClass = _classesService.Get(id);
            var lstTeacher = _teachersService.Get();
            var lstCourse = _courseCollectionService.Get();
            var lstStudent = _studentService.Get();

            List<string> nameTeacher = new List<string>();
            foreach (var nameId in detailClass.TeacherID)
            {
                var teacherName = lstTeacher.FirstOrDefault(x => x.Id == nameId);
                nameTeacher.Add(teacherName.Name);
            }
            detailClass.TeacherID = nameTeacher;

            List<string> nameStudent = new List<string>();
            foreach (var nameID in detailClass.StudentID)
            {
                var studentName = lstStudent.FirstOrDefault(x => x.Id == nameID);
                nameStudent.Add(studentName.Name);
            }
            detailClass.StudentID = nameStudent;

            var courseName = lstCourse.FirstOrDefault(x => x.Id == detailClass.CourseID);
            detailClass.CourseID = courseName.CourseName;

            return View(detailClass);
        }
        [HttpGet]
        public IActionResult UpdateClass(string id)
        {
            var lstTeacher = _teachersService.Get();
            var lstStudent = _studentService.Get();
            var lstCourse = _courseCollectionService.Get();
            var lstClass = _classesService.Get(id);

            ViewBag.studentname = lstStudent;
            ViewBag.coursename = lstCourse;

            List<Teachers> suitablename = new List<Teachers>();
            foreach (var nameId in lstClass.TeacherID)
            {
                var teacherName = lstTeacher.FirstOrDefault(x => x.Id == nameId);
                suitablename.Add(teacherName);
            }

            var courseName = lstCourse.FirstOrDefault(x => x.Id == lstClass.CourseID);
            ViewBag.coursemess = courseName.CourseName;
            ViewBag.teachername = suitablename;
            return View(lstClass);
        }

        [HttpPost]
        public IActionResult UpdateClass([FromForm] Classes classes)
        {
            _classesService.Update(classes);
            return RedirectToAction("Get");
        }
        [HttpDelete]
        public IActionResult DeleteClass(string id)
        {
            _classesService.Delete(id);
            return RedirectToAction("Get");
        }

        [NonAction]
        public JsonResult FindSuitableTeacher(string id)
        {
            //var teacherresponse = _classesService.Get(id);
            var lstTeacher = _teachersService.Get();
            var detailCourse = _courseCollectionService.Get(id);

            List<string> nameTeacher = new List<string>();
            foreach (var nameId in detailCourse.TeacherID)
            {
                var teacherName = lstTeacher.FirstOrDefault(x => x.Id == nameId);
                var idAndName = nameId + "," + teacherName.Name;
                nameTeacher.Add(idAndName);
            }
            detailCourse.TeacherID = nameTeacher;
            return Json(detailCourse);
        }
    }
}
