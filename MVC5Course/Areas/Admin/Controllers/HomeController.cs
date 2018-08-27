using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        // 41 練習 Ajax.JavaScriptStringEncode 輸出安全的 JavaScript 字串
        public ActionResult Index(string msg)
        {
            ViewBag.Msg = msg + "');\t\n" + "console.log('jj";
            return View();
        }
    }
}