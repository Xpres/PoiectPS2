using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication3.Controllers
{
    [Authorize]
    public class SecretController : Controller
    {
        
        public ContentResult Secret() {
            return Content("This is a secret!");
        }

        public ContentResult Overt() {
            return Content("This is not a secret ..");
        }
    }
}