using MVC5Course.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    //18 練習建立 BaseController 當成所有控制器的共同祖先 (請用抽象類別)
    public abstract class BaseController : Controller
    {
        protected FabricsEntities db = new FabricsEntities();

        protected override void HandleUnknownAction(string actionName)
        {
            // 可在此作若輸入不存在的 ActionName 時，可以在這邊處理或記錄一筆資料到 DB

            this.RedirectToAction("Index").ExecuteResult(this.ControllerContext);
        }
    }
}