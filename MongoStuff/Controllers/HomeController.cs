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
                try
                {
                    //connection string from web.config
                    string constr = ConfigurationManager.AppSettings["connectionString"];
                    
                    //init client and db
                    MongoClient Client = new MongoClient(constr);
                    IMongoDatabase DB = Client.GetDatabase("Trips");
                    
                    //get collection and add new trip to collection
                    var collection = DB.GetCollection<TripDetails>("TripDetails");
                    trip.Id = ObjectId.GenerateNewId().ToString();
                    collection.InsertOneAsync(trip);

                    return RedirectToAction("TripList");
                }
                catch(Exception ex)
                {
                    //can log error to db here
                    Console.WriteLine(ex.Message);

                    //return this view, could be expanded to alert user of error
                    return RedirectToAction("TripList");
                }
                
            }
            return View();
        }

        public ActionResult TripList()
        {
            try
            {
                //connection string from web.config
                string constr = ConfigurationManager.AppSettings["connectionString"];

                //init client and db
                MongoClient Client = new MongoClient(constr);
                var db = Client.GetDatabase("Trips");

                //get collection
                var collection = db.GetCollection<TripDetails>("TripDetails").Find(new BsonDocument()).ToList();

                return View(collection);
            }
            catch(Exception ex)
            {
                //can log error to db here
                Console.WriteLine(ex.Message);

                //return this view, could be expanded to alert user of error
                return RedirectToAction("Index");
            }
            
        }

        public ActionResult Delete(string id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string constr = ConfigurationManager.AppSettings["connectionString"];
                    var Client = new MongoClient(constr);
                    var DB = Client.GetDatabase("Trips");
                    var collection = DB.GetCollection<TripDetails>("TripDetails");
                    var DeleteRecored = collection.DeleteOneAsync(
                                   Builders<TripDetails>.Filter.Eq("Id", id));
                    return RedirectToAction("TripList");
                }
                catch(Exception ex)
                {
                    //can log error to db here
                    Console.WriteLine(ex.Message);

                    //return this view, could be expanded to alert user of error
                    return View("TripList");
                }
                
            }
            //return this view, could be expanded to alert user of error
            return View("TripList");
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            try
            {
                string constr = ConfigurationManager.AppSettings["connectionString"];
                MongoClient Client = new MongoClient(constr);
                var db = Client.GetDatabase("Trips");
                var collection = db.GetCollection<TripDetails>("TripDetails").Find(new BsonDocument()).ToList();
                TripDetails tripDetails = collection.Where(c => c.Id == id).FirstOrDefault();

                return View(tripDetails);
            }
            catch(Exception ex)
            {
                //can log error to db here
                Console.WriteLine(ex.Message);

                //return this view, could be expanded to alert user of error
                return RedirectToAction("TripList");
            }

        }

        [HttpPost]
        public ActionResult Edit(TripDetails trip)
        {
            if (ModelState.IsValid)
            {
                try
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
                }
                catch(Exception ex)
                {
                    //can log error to db here
                    Console.WriteLine(ex.Message);
                }

                return RedirectToAction("TripList");
            }
            return View();
        }

        public ActionResult Contact()
        {
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