using CoursesManag.Models;
using CoursesManag.Services;
using jQueryDatatableServerSideNetCore.Models.AuxiliaryModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace CoursesManag.Controllers
{
    [Route("datatable/[action]")]
    [ApiController]
    public class DataTableTest : Controller
    {
        private readonly ClassesService _classesService;
        private readonly CourseCollectionService _courseCollectionService;
        private readonly TeachersService _teachersService;
        private readonly StudentsService _studentService;

        public DataTableTest(ClassesService classesService, CourseCollectionService courseCollectionService, TeachersService teachersService, StudentsService studentService)
        {
            _classesService = classesService;
            _courseCollectionService = courseCollectionService;
            _studentService = studentService;
            _teachersService = teachersService;
        }

        [HttpGet]
        public IActionResult Index()
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
            return View(response);
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

            //var draw = Request.Form["draw"].FirstOrDefault();
            //var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            //var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            //var searchValue = Request.Form["search[value]"].FirstOrDefault();
            //int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            //int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            var data = response.AsQueryable();
            //get total count of data in table
            totalRecord = data.Count();

            if (!string.IsNullOrEmpty(dtParameters.Search.Value))
            {
                data = data.Where(x => x.ClassesName.ToLower().Contains(dtParameters.Search.Value.ToLower()) 
                || x.CourseID.ToLower().Contains(dtParameters.Search.Value.ToLower()) 
                || x.DateEnd.ToString().ToLower().Contains(dtParameters.Search.Value.ToLower()) 
                || x.DateStart.ToString().ToLower().Contains(dtParameters.Search.Value.ToLower()));
            }
            filterRecord = data.Count();


            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection)) data = data.OrderBy(sortColumn + " " + sortColumnDirection);
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

    }
}
