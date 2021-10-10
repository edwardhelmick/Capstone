using MongoDB.Bson;
using MongoDB.Driver;
using MongoStuff.Models;
using System.Configuration;
using System.Web.Mvc;
using System.Linq;
using System;

namespace MongoStuff.Controllers
{
    public class HomeController : Controller
    {        
        public ActionResult Index()
        {
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://ehelmick93:<password>@cluster0.cmv2d.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
            var client = new MongoClient(settings);
            var database = client.GetDatabase("test");

            return View();
        }

        [HttpGet]
        public ActionResult AddTrip()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddTrip(TripDetails trip)
        {
            if (ModelState.IsValid)
            {
                string constr = ConfigurationManager.AppSettings["connectionString"];
                var Client = new MongoClient(constr);
                //var DB = Client.GetDatabase("Employee");
                var DB = Client.GetDatabase("Trips");
                var collection = DB.GetCollection<TripDetails>("TripDetails");
                trip.Id = ObjectId.GenerateNewId().ToString();
                collection.InsertOneAsync(trip);

                
                return RedirectToAction("TripList");
            }
            return View();
        }

        public ActionResult Contact()
        {
            string constr = ConfigurationManager.AppSettings["connectionString"];
            var Client = new MongoClient(constr);
            //MongoClient dbClient = new MongoClient("mongodb+srv://ehelmick93:Abc123@cluster0.cmv2d.mongodb.net/Testdatabase?retryWrites=true&w=majority");
            var dbList = Client.ListDatabases().ToList();

            return View();
        }

        public ActionResult TripList()
        {
            string constr = ConfigurationManager.AppSettings["connectionString"];
            MongoClient Client = new MongoClient(constr);
            var db = Client.GetDatabase("Trips");
            var collection = db.GetCollection<TripDetails>("TripDetails").Find(new BsonDocument()).ToList();



            return View(collection);
        }

        public ActionResult Delete(string id)
        {
            if (ModelState.IsValid)
            {
                string constr = ConfigurationManager.AppSettings["connectionString"];
                var Client = new MongoClient(constr);
                var DB = Client.GetDatabase("Trips");
                var collection = DB.GetCollection<TripDetails>("TripDetails");
                var DeleteRecored = collection.DeleteOneAsync(
                               Builders<TripDetails>.Filter.Eq("Id", id));
                return RedirectToAction("TripList");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            string constr = ConfigurationManager.AppSettings["connectionString"];
            MongoClient Client = new MongoClient(constr);
            var db = Client.GetDatabase("Trips");
            var collection = db.GetCollection<TripDetails>("TripDetails").Find(new BsonDocument()).ToList();
            TripDetails tripDetails = collection.Where(c => c.Id == id).FirstOrDefault();

            return View(tripDetails);
        }

        [HttpPost]
        public ActionResult Edit(TripDetails trip)
        {
            if (ModelState.IsValid)
            {
                string constr = ConfigurationManager.AppSettings["connectionString"];
                var Client = new MongoClient(constr);
                var Db = Client.GetDatabase("Trips");
                var collection = Db.GetCollection<TripDetails>("TripDetails");

                var update = collection.FindOneAndUpdateAsync(Builders<TripDetails>.Filter.Eq("Id", trip.Id), Builders<TripDetails>.Update
                    .Set("Code", trip.Code)
                    .Set("Name", trip.Name)                    
                    .Set("LengthDays", trip.LengthDays)
                    .Set("StartDate", trip.StartDate)
                    .Set("ResortName", trip.ResortName)
                    .Set("CostPerPerson", trip.CostPerPerson)
                    .Set("Img_Base64", trip.Img_Base64)
                    .Set("Description", trip.Description));

                return RedirectToAction("TripList");
            }
            return View();
        }

        public ActionResult Travel()
        {
            return View();
        }

        public ActionResult Rooms()
        {
            return View();
        }
    }
}