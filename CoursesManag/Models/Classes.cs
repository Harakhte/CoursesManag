using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesManag.Models
{
    public class Classes
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ClassesName { get; set; }

        public string CourseID { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public string SessionStart { get; set; }

        public string SessionEnd { get; set; }

        public List<string> SessionsEachWeek { get; set; }

        public List<string> StudentID { get; set; }

        public List<string> TeacherID { get; set; }


    }
}
