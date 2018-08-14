using System;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class 產生ViewBag下有一個Message可以用Attribute : ActionFilterAttribute
    {
        //在進入 ActionResult 之前，塞入一 ViewBag.Message
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.Message = "這是 Shard data";

            base.OnActionExecuting(filterContext);
        }
    }
}