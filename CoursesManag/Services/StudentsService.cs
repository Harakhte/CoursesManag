using CoursesManag.Models;
using MongoDB.Driver;
using MongoDBWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesManag.Services
{
    public class StudentsService
    {
        private readonly IMongoCollection<Students> _students;
        public StudentsService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _students = database.GetCollection<Students>(nameof(Students));

        }

        public List<Students> Get()
        {
            List<Students> students;
            students = _students.Find(temp => true).ToList();
            return students;
        }
        
        public Students Get(string id) =>
            _students.Find<Students>(temp => temp.Id == id).FirstOrDefault();

        public Students Create(Students students)
        {
            _students.InsertOne(students);
            return students;
        }

        public Students Update(Students students)
        {
            Get(students.Id);
            _students.ReplaceOne(temp => temp.Id == students.Id, students);
            return students;
        }
        public void Remove(string id)
        {
            _students.DeleteOne(temp => temp.Id == id);
        }
    }
}
