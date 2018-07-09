using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;

namespace MVC5Course.Controllers
{
    [RoutePrefix("clients")]  //17 練習用屬性路由 (Attribute Routing) 來定義自訂網址結構
    public class ClientsController : Controller
    {
        //private FabricsEntities db = new FabricsEntities();

        // 15 請將 ClientController 改用 Repository 與 UnitOfWork 來實作
        //ClientRepository clientRepo = RepositoryHelper.GetClientRepository();
        //OccupationRepository occuRepo = RepositoryHelper.GetOccupationRepository();

        //將兩個 Repository (table) 共用一份 UnitOfWork 物件 客戶資料表 + 客戶職業資料表
        ClientRepository clientRepo;
        OccupationRepository occuRepo;
        public ClientsController()
        {
            clientRepo = RepositoryHelper.GetClientRepository();
            occuRepo = RepositoryHelper.GetOccupationRepository(clientRepo.UnitOfWork);
        }

        // GET: Clients
        [Route("")]
        public ActionResult Index()
        {
            //var client = db.Client.Include(c => c.Occupation);
            var client = clientRepo.All();
            return View(client.OrderByDescending(o => o.ClientId).Take(10));
        }

        //14 對 Client 資料新增「搜尋」功能
        [Route("search")]
        public ActionResult Search(string keyword , int take = 0)
        {
            //var client = db.Client.AsQueryable();

            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    client = client.Where(w => w.FirstName.Contains(keyword)).Take(10);
            //}

            
            var client = clientRepo.SearchKeyword(keyword, take);
            //指定由那個View顯示查詢結果
            return View("Index", client);
            
        }

        // GET: Clients/Details/5
        [Route("id")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Client client = db.Client.Find(id);
            Client client = clientRepo.Find(id.Value);

            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        //示範 CatchAll 路由的用法(會顯示完整的PATH INFO)
        //若網址是 /TEST.axd/a/b/c/d 則 pathInfo 所得到的值為 a/b/c/d，如果沒加上星號 ( * ) 該 pathInfo 就會等於 a 而已。
        [Route("{*name}")]
        public ActionResult Details2(string name)
        {
            string[] names = name.Split('/');
            string FirstName = names[0];
            string MiddleName = names[1];
            string LastName = names[2];

            Client client = clientRepo.All().FirstOrDefault(p => p.FirstName == FirstName && p.MiddleName == MiddleName && p.LastName == LastName);

            if (client == null)
            {
                return HttpNotFound();
            }
            return View("Details", client);
        }

        // GET: Clients/Create
        [Route("create")]
        public ActionResult Create()
        {
            //ViewBag.OccupationId = new SelectList(db.Occupation, "OccupationId", "OccupationName");
            var occuData = occuRepo.All();
            ViewBag.OccupationId = new SelectList(occuData, "OccupationId", "OccupationName");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public ActionResult Create([Bind(Include = "ClientId,FirstName,MiddleName,LastName,Gender,DateOfBirth,CreditRating,XCode,OccupationId,TelephoneNumber,Street1,Street2,City,ZipCode,Longitude,Latitude,Notes,IdNumber")] Client client)
        {
            if (ModelState.IsValid)
            {
                //db.Client.Add(client);
                //db.SaveChanges();

                clientRepo.Add(client);
                clientRepo.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }

            //ViewBag.OccupationId = new SelectList(db.Occupation, "OccupationId", "OccupationName", client.OccupationId);
            var occuData = occuRepo.All();
            ViewBag.OccupationId = new SelectList(occuData, "OccupationId", "OccupationName", client.OccupationId);

            return View(client);
        }

        // GET: Clients/Edit/5
        [Route("edit/{id}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Client client = db.Client.Find(id);
            Client client = clientRepo.Find(id.Value);

            if (client == null)
            {
                return HttpNotFound();
            }
            //ViewBag.OccupationId = new SelectList(db.Occupation, "OccupationId", "OccupationName", client.OccupationId);
            var occuData = occuRepo.All();
            ViewBag.OccupationId = new SelectList(occuData, "OccupationId", "OccupationName", client.OccupationId);

            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit/{id}")]
        public ActionResult Edit([Bind(Include = "ClientId,FirstName,MiddleName,LastName,Gender,DateOfBirth,CreditRating,XCode,OccupationId,TelephoneNumber,Street1,Street2,City,ZipCode,Longitude,Latitude,Notes,IdNumber")] Client client)
        {
            if (ModelState.IsValid)
            {
                var db = clientRepo.UnitOfWork.Context;
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            //ViewBag.OccupationId = new SelectList(db.Occupation, "OccupationId", "OccupationName", client.OccupationId);
            var occuData = occuRepo.All();
            ViewBag.OccupationId = new SelectList(occuData, "OccupationId", "OccupationName", client.OccupationId);

            return View(client);
        }

        // GET: Clients/Delete/5
        [Route("delete/{id}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Client client = db.Client.Find(id);
            Client client = clientRepo.Find(id.Value);

            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("delete/{id}")]
        public ActionResult DeleteConfirmed(int id)
        {
            //Client client = db.Client.Find(id);
            //db.Client.Remove(client);
            //db.SaveChanges();

            Client client = clientRepo.Find(id);
            clientRepo.Delete(client);
            clientRepo.UnitOfWork.Commit();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
                clientRepo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
