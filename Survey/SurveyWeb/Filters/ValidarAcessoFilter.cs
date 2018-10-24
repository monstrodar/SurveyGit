using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Filters
{
    public class ValidarAcessoFilter : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpCookie ck = filterContext.HttpContext.Request.Cookies["token"];
            if (ck == null || int.Parse(ck.Values["idUsuario"].ToString()) <= 0)
            {
                filterContext.Result = new RedirectResult("/Home/Logout");
            }
        }
    }
}