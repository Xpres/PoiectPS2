using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication3.Controllers
{
    public class HelloController : Controller
    {
        [Authorize(Roles = "AdminRole")]
        public ActionResult Index()
        {
            return View();
        }

        // 
        // GET: /Hello/Welcome/ 

        public string Welcome(int a)
        {
            return "This is the Welcome action method...  : "+ a.ToString();
        }
    }
}