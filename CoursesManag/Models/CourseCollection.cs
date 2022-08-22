using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesManag.Models
{
    public class CourseCollection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string CourseName { get; set; }

        public string CoursePrice { get; set; }

        public List<string> TeacherID { get; set; }
    }
}
