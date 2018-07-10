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

            return View();
        }
    }
}