using CoursesManag.Models;
using MongoDB.Driver;
using MongoDBWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesManag.Services
{
    public class ClassesService
    {
        private readonly IMongoCollection<Classes> _classes;
        public ClassesService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _classes = database.GetCollection<Classes>(nameof(Classes));
        }
        public List<Classes> Get()
        {
            List<Classes> classes;
            classes = _classes.Find(temp => true).ToList();
            return classes;
        }

        public Classes Create(Classes classes)
        {
            _classes.InsertOne(classes);
            return classes;
        }
        public Classes Get(string id)
        {
            return _classes.Find<Classes>(temp => temp.Id == id).FirstOrDefault();
        }

        public Classes Update(Classes classes)
        {
            Get(classes.Id);
            _classes.ReplaceOne(temp => temp.Id == classes.Id, classes);
            return classes;
        }

        public void Delete(string id)
        {
            _classes.DeleteOne(temp => temp.Id == id);
        }
    }
}
