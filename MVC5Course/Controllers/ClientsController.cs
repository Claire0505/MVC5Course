﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using System.Data.Entity.Validation;

namespace MVC5Course.Controllers
{
    [RoutePrefix("clients")]  //17 練習用屬性路由 (Attribute Routing) 來定義自訂網址結構
    public class ClientsController : BaseController
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

            // 34 練習 @Html.DropDownList() 下拉選單用法
            var creditRating = clientRepo.All()
                .Select(p => p.CreditRating)
                .Distinct()
                .OrderBy(o => o)
                .Select(s => new SelectListItem()
                {
                    Text = s.Value.ToString(),
                    Value = s.Value.ToString()
                });

            ViewBag.creditRating = new SelectList(creditRating, "Value", "Text");

            return View(client.OrderByDescending(o => o.ClientId).Take(10));
        }

        //14 對 Client 資料新增「搜尋」功能
        [Route("search")]
        public ActionResult Search(string keyword , string CreditRating, int take = 0)
        {
                      
            var client = clientRepo.SearchKeyword(keyword, take, CreditRating);

            var creditRating = clientRepo.All()
                .Select(p => p.CreditRating)
                .Distinct()
                .OrderBy(o => o)
                .Select(s => new SelectListItem()
                {
                    Text = s.Value.ToString(),
                    Value = s.Value.ToString()
                });

            ViewBag.CreditRating = new SelectList(creditRating, "Value", "Text");

            //指定由那個View顯示查詢結果
            return View("Index", client);
            
        }

        // 26 練習透過 Model Binding 實現批次更新功能
        // GET: BatchUpdate
        [Route("BatchUpdate")]
        public ActionResult BatchUpdate()
        {
            var client = clientRepo.All();
            return View(client.OrderByDescending(o => o.ClientId).Take(10));
        }

        // 批次修改資料
        [HttpPost]
        [Route("BatchUpdate")]
        [HandleError(ExceptionType = typeof(DbEntityValidationException), View = "Error_DbEntityValidationException")]
        public ActionResult BatchUpdate(ClientBatchViewModel[] data, PageCondViewModel page)
        {
            // page.Keyword
            // data[0].ClintId

            if (ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    var client = clientRepo.Find(item.ClientId);
                    client.FirstName = item.FirstName;
                    client.MiddleName = item.MiddleName;
                    client.LastName = item.LastName;
                }

                // 示範如何取得 DbEntityValidationException 例外的驗證失敗資訊
                // 使用自訂錯誤處理 HandleError
                clientRepo.UnitOfWork.Commit();

                return RedirectToAction("Index");

            }

            ViewData.Model = clientRepo.All().OrderByDescending(o => o.ClientId).Take(10);
            return View("BatchUpdate");
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

            var client = clientRepo.All()
                .FirstOrDefault(w => w.FirstName == FirstName &&
                                   w.MiddleName == MiddleName &&
                                   w.LastName == LastName);

            if (client == null)
            {

            }

            return View("Details", client);
        }

        [Route("{id}/orders")]
        [ChildActionOnly] // 代表不能讓使用者直接連到該 ActionResult
        public ActionResult Detail_OrderList(int id)
        {
            ViewData.Model = clientRepo.Find(id).Order.ToList();

            return PartialView("OrderList");
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
        // 使用 FormCollection 用意是為了區別與上面 Edit 的 Get 功能區分 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit/{id}")]
        public ActionResult Edit(int id, FormCollection form)
        {
            // 使用模型繫結延遲驗證 TryUpdateModel
            Client client = clientRepo.Find(id);

            // prefix: 前端欄位 name 屬性若使用 page.id 就可以設定此為 page
            //      會去找 page 名稱下的欄位資料
            // includeProperties: 放需要修改的欄位(就算欄位有被修改也會忽略)
            // excludeProperties: 放不需要修改的欄位(就算該欄位有被修改也會忽略)
            if (TryUpdateModel(client, "", null, excludeProperties: new string[] { "FirstName" }))
            {
                clientRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            var occuData = occuRepo.All();
            ViewBag.OccupationId = new SelectList(occuData, "OccupationId", "OccupationName", client.OccupationId);

            // 清除 ModelState
            /* 若清除 ModelState 後會以 Model 為資料 Binding 回 View
                (此指下方再次從 EF 取得資料的 item) */
            //ModelState.Clear();

            // 移除 ModelState 單一欄位
            /* 此處移除欄位後，會再與下方 item 取得的 Model 資料欄位合併，傳回給 View */
            // ModelState.Remove("Latitude");

            // 當 ModelState 資料與 Model 資料都存在時，會以 ModelState 為優先
            var item = clientRepo.Find(client.ClientId);
            return View(item);

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
            client.IsDeleted = true;
            //clientRepo.Delete(client);
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
