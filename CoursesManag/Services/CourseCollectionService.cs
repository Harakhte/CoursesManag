using CoursesManag.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesManag.Services
{
    public class CourseCollectionService
    {
        private readonly IMongoCollection<CourseCollection> _courseCollection;
        public CourseCollectionService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _courseCollection = database.GetCollection<CourseCollection>(nameof(CourseCollection));

        }

        public List<CourseCollection> Get()
        {
            List<CourseCollection> courseCollection;
            courseCollection = _courseCollection.Find(temp => true).ToList();
            return courseCollection;
        }

        public CourseCollection Get(string id)
        {
            return _courseCollection.Find<CourseCollection>(temp => temp.Id == id).FirstOrDefault();
        }

        public CourseCollection Create(CourseCollection courseCollection)
        {
            _courseCollection.InsertOne(courseCollection);
            return courseCollection;
        }

        public CourseCollection Update(CourseCollection course)
        {
            Get(course.Id);
            _courseCollection.ReplaceOne(temp => temp.Id == course.Id, course);
            return course;
        }

        public void Delete(string id)
        {
            _courseCollection.DeleteOne(temp => temp.Id == id);
        }
    }
}
