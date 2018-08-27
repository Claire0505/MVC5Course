using System.Web.Mvc;

namespace MVC5Course.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[]
                {
                    "MVC5Course.Areas.Admin.Controllers"
                }
            );
        }
    }
}

// 必免找到多個與名為 'Home' 的控制器相符的類型。
// 如果服務此要求('{controller}/{action}/{id}') 的路由沒有指定命名空間以搜尋符合該要求的控制器，
// 就會發生這個情況。在這種情況下，請呼叫可接受 'namespaces' 參數的 'MapRoute' 方法的多載來註冊此路由。
// namespaces: new string[]
//                {
//                    "MVC5Course.Areas.Admin.Controllers"
//                }