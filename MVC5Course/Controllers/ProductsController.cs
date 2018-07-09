using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using Omu.ValueInjecter;

namespace MVC5Course.Controllers
{
    public class ProductsController : Controller
    {
        //private FabricsEntities db = new FabricsEntities();

        //將 ProductsController 改用 Repository 與 UnitOfWork 來實作
        ProductRepository prodRepo = RepositoryHelper.GetProductRepository();

        // GET: Products
        public ActionResult Index()
        {
            var data = prodRepo.TakeData(10);

            return View(data);
        }

        //06 練習透過 ViewModel 建立頁面
        public ActionResult Index2()
        {
            IQueryable<ProductViewModel> data = prodRepo.ProductViewModel();

            return View(data);
        }

        //07 練習透過 ViewModel 建立表單頁面與透過 ViewModel 接資料
        public ActionResult AddNewProduct()
        {
            return View();
        }

        // 07 練習透過 ViewModel 建立表單頁面與透過 ViewModel 接資料
        [HttpPost]
        public ActionResult AddNewProduct(ProductViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // TODO 新增Product資料
            // 08 練習透過 Entity Framework 新增一筆 Product 資料
            //新增 Product 資料
            var product = new Product()
            {
                // ProductId 由 db table 自動編號
                ProductId = data.ProductId,
                ProductName = data.ProductName,
                Price = data.Price,
                Active = true,
                Stock = data.Stock
            };

            prodRepo.Add(product);
            prodRepo.UnitOfWork.Commit();

            return RedirectToAction("Index2");
        }

        // 09 練習透過 Entity Framework 更新一筆 Product 資料
        public ActionResult EditProduct( int id)
        {
            var data = prodRepo.Find(id);
            return View(data);
        }

        // 09 練習透過 Entity Framework 更新一筆 Product 資料
        [HttpPost]
        public ActionResult EditProduct(int id, ProductViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var product = prodRepo.Find(id);

            //product.ProductName = data.ProductName;
            //product.Price = data.Price;
            //product.Stock = data.Stock;

            // 示範 ValueInjecter 的基本用法
            // ValueInjecter基本使用說明
            // ViewModel要轉資料庫Model的情形發生，一的般方法通常都會打一長串code將ViewModel的值給Model
            // product.InjectFrom(data)這段的意思是，將同樣型別、同樣屬性名稱的職Mapping到data去
            // 當Properties有十幾二十個的後，會有明顯的感覺，程式碼看起來也會短很多。
            product.InjectFrom(data);
            prodRepo.UnitOfWork.Commit();

            return RedirectToAction("Index2");
        }

        // 10 練習透過 Entity Framework 刪除一筆 Product 資料
        public ActionResult DeleteProduct(int id)
        {
            var data = prodRepo.Find(id);
            return View(data);
        }

        [HttpPost, ActionName("DeleteProduct")]
        public ActionResult DeleteProductConfirmed(int id)
        {
            var product = prodRepo.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            prodRepo.Delete(product);
            prodRepo.UnitOfWork.Commit();

            return RedirectToAction("Index2");
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = prodRepo.Find(id.Value);

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        {
            if (ModelState.IsValid)
            {
                prodRepo.Add(product);
                prodRepo.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = prodRepo.Find(id.Value);

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        {
            if (ModelState.IsValid)
            {
                var db = prodRepo.UnitOfWork.Context;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = prodRepo.Find(id.Value);

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = prodRepo.Find(id);
            prodRepo.Delete(product);
            prodRepo.UnitOfWork.Commit();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                prodRepo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
