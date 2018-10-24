using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb
{
    internal class Util
    {
        internal static int ObterUsuario
        {
            get
            {
                HttpCookie ck = HttpContext.Current.Request.Cookies["token"];
                if (ck != null)
                    return int.Parse(ck.Values["idUsuario"].ToString());
                else
                    return -1;
            }
        }
    }
}