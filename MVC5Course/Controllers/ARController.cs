using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class ARController : Controller
    {
        // 練習使用各種 Active Result 方式
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewTest()
        {
            // View 可以帶入 object model 或 sting viewName
            // 當有個 string 型別的 model 要傳入 View，會被誤判為 viewName
            // 因此在傳入前將 object 型別加上去避免誤判，實際上比較少遇到此做法

            string model = "TestModel";
            return View((object)model);
        }
    }
}