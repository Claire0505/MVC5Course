using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace MVC5Course.Controllers
{
    public class ARController : Controller
    {
        // 練習使用各種 Active Result 方式
        public ActionResult Index()
        {
            return View();
        }

        //20 練習將字串當成 Model 傳到 View 並顯示 Model 內容
        public ActionResult ViewTest()
        {
            // View 可以帶入 object model 或 sting viewName
            // 當有個 string 型別的 model 要傳入 View，會被誤判為 viewName
            // 因此在傳入前將 object 型別加上去避免誤判，實際上比較少遇到此做法

            string model = "TestModel";
            return View((object)model);
        }


        //21 練習用 PartialView 顯示一個沒有 Layout 的頁面
        public ActionResult PartialViewTest()
        {
            string model = "PartialView";
            return PartialView("ViewTest", (object)model);
        }

        //示範 ContentResult 的用法
        public ActionResult ContentTest()
        {
            return Content("Test Content!!",
                    "text/plain",
                    Encoding.GetEncoding("Big5"));
        }
    }
}