using CoursesManag.Models;
using MongoDB.Driver;
using MongoDBWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesManag.Services
{
    public class TeachersService
    {
        private readonly IMongoCollection<Teachers> _teachers;
        public TeachersService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _teachers = database.GetCollection<Teachers>(nameof(Teachers));

        }

        public List<Teachers> Get()
        {
            List<Teachers> teachers;
            teachers = _teachers.Find(temp => true).ToList();
            return teachers;
        }

        public Teachers Get(string id)
        {
            return _teachers.Find<Teachers>(temp => temp.Id == id).FirstOrDefault();
        }

        public Teachers Create(Teachers teachers)
        {
            _teachers.InsertOne(teachers);
            return teachers;
        }

        public Teachers Update(Teachers teachers)
        {
            Get(teachers.Id);
            _teachers.ReplaceOne(temp => temp.Id == teachers.Id, teachers);
            return teachers;
        }

        public void Delete(string id)
        {
            _teachers.DeleteOne(temp => temp.Id == id);
        }

    }
}
