using CoursesManag.Models;
using CoursesManag.Services;
using jQueryDatatableServerSideNetCore.Models.AuxiliaryModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace CoursesManag.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClassesService _classesService;
        private readonly CourseCollectionService _courseCollectionService;
        private readonly TeachersService _teachersService;
        private readonly StudentsService _studentService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ClassesService classesService, CourseCollectionService courseCollectionService, TeachersService teachersService, StudentsService studentService)
        {
            _logger = logger;
            _classesService = classesService;
            _courseCollectionService = courseCollectionService;
            _studentService = studentService;
            _teachersService = teachersService;
        }

        [HttpGet]
        public IActionResult Index(string searchString, string classsess, string teacherss, int page)
        {
            List<Classes> response = new List<Classes>();

            var lstClass = _classesService.Get();
            var lstTeacher = _teachersService.Get();
            var lstCourse = _courseCollectionService.Get();
            var lstStudent = _studentService.Get();

            ViewBag.classname = lstClass;
            ViewBag.teachername = lstTeacher;

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
                        if (teacherName != null)
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
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Replace(" ", string.Empty);
                response = response.Where(s => s.TeacherID.Any(y => y.Replace(" ", string.Empty).Contains(searchString)) || s.StudentID.Any(y => y.Replace(" ", string.Empty).Contains(searchString)) || s.ClassesName.Replace(" ", string.Empty).Contains(searchString)).ToList();
            }
            if (!string.IsNullOrEmpty(teacherss) && teacherss != "null")
            {
                teacherss = teacherss.Replace(" ", string.Empty);
                response = response.Where(s => s.TeacherID.Any(y => y.Replace(" ", string.Empty).Contains(teacherss))).ToList();
            }
            if (!string.IsNullOrEmpty(classsess) && classsess != "null")
            {
                classsess = classsess.Replace(" ", string.Empty);
                response = response.Where(s => s.ClassesName.Replace(" ", string.Empty).Contains(classsess)).ToList();
            }

            if (page <= 0)
            {
                page = 1;
            }
            int limit = 5;
            int start = (page - 1) * limit;
            int total = response.Count();
            ViewBag.totalclass = total;
            ViewBag.Current = page;
            float numberPage = (float)total / limit;
            ViewBag.numberPage = (int)Math.Ceiling(numberPage);
            var zzzzz = response.OrderByDescending(s => s.Id).Skip(start).Take(limit);
            return View(zzzzz);
        }

        [HttpPost]
        public ActionResult GetList([FromBody] DtParameters dtParameters)
        {
            List<Classes> response = new List<Classes>();

            var lstClass = _classesService.Get();
            var lstTeacher = _teachersService.Get();
            var lstCourse = _courseCollectionService.Get();
            var lstStudent = _studentService.Get();

            ViewBag.classname = lstClass;
            ViewBag.teachername = lstTeacher;

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
                        if (teacherName != null)
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

            int totalRecord = 0;
            int filterRecord = 0;

            var sortColumn = dtParameters.Columns[dtParameters.Order[0].Column].Name;
            var sortColumnDirection = dtParameters.Order[0].Dir;
            var data = response;
            totalRecord = data.Count();

            if (!string.IsNullOrEmpty(dtParameters.TeacherSearch) && dtParameters.TeacherSearch != "null")
            {
                data = (List<Classes>)data.Where(x => string.Join(",", x.TeacherID).ToLower().Contains(dtParameters.TeacherSearch.ToLower())).ToList();

            }

            if (!string.IsNullOrEmpty(dtParameters.ClassSearch) && dtParameters.ClassSearch != "null")
            {
                data = (List<Classes>)data.Where(x => x.ClassesName.ToLower().Contains(dtParameters.ClassSearch.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(dtParameters.Search.Value))
            {
                data = data.Where(x => x.ClassesName.ToLower().Contains(dtParameters.Search.Value.ToLower())
                               || x.CourseID.ToLower().Contains(dtParameters.Search.Value.ToLower())
                               || string.Join(",", x.TeacherID).ToLower().Contains(dtParameters.Search.Value.ToLower())
                               || string.Join(",", x.StudentID).ToLower().Contains(dtParameters.Search.Value.ToLower())).ToList();
            }
            filterRecord = data.Count();


            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
            {
                if (sortColumnDirection == "asc")
                {
                    data = data.OrderBy(x => x.GetType().GetProperty(sortColumn).GetValue(x, null)).ToList();
                }
                else
                {
                    data = data.OrderByDescending(x => x.GetType().GetProperty(sortColumn).GetValue(x, null)).ToList();
                }
            }
            //pagination
            var empList = data.Skip(dtParameters.Start).Take(dtParameters.Length).ToList();
            var returnObj = new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalRecord,
                recordsFiltered = filterRecord,
                data = empList
            };
            return Json(returnObj);

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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
