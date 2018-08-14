using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class LocalOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // 若不是 local 端就導去首頁
            // 測試的話可以將 ! 拿掉
            if (!filterContext.RequestContext.HttpContext.Request.IsLocal)
            {
                filterContext.Result = new RedirectResult("/");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}