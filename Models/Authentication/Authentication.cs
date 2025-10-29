using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShopWeb_Project.Models.Authentication
{
    public class Authentication: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(context.HttpContext.Session.GetString("UserName") == null)
            {
                context.Result=new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        {"controller","Access" },
                        {"action","Login" }
                    }
                    );
            }
        }
    }
}
