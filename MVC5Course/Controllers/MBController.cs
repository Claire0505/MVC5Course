using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class MBController : BaseController
    {
        // GET: MB
        public ActionResult Index()
        {
            var data = "Hello World";
            ViewData.Model = data;
            return View();
        }

        // 示範 ViewData.Model
        public ActionResult ViewDataModelDemo()
        {
            var data = "這是ViewData.Model Demo";
            ViewData.Model = data;
            return View();
        }

        // 示範 ViewBag.keyName
        // 骨子裡用的是 ViewData
        public ActionResult ViewBagDemo()
        {
            ViewBag.Text = "這是 ViewBag Demo";
            return View();
        }

        // 示範 ViewData[KeyName]
        public ActionResult ViewDataDemo()
        {
            ViewData["Text"] = @"這是ViewData[""Text""] Demo";
            return View();
        }

        // 示範 TempData["KeyName"]
        /* TempData 使用情境
         * 當表單資料送出後，先透過 TempDataSave 取得輸入的資料
         * 中間需要處理輸入的資料儲存到 DB
         * 接著在轉跳完成畫面
         */
        public ActionResult TempDataSave()
        {
            TempData["Text"] = @"這是 TempDataSave[""Text""] Demo";
            return RedirectToAction("TempDataDemo");
        }

        public ActionResult TempDataDemo()
        {

            return View();
        }
    }
}