using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication3.Controllers
{
    public class helloWorldControler : Controller
    {
        // GET: helloWorldControler
        public string Index()
        {
            return "This is my <b>default</b> action...";
        }

        // 
        // GET: /helloWorldControler/Welcome/ 

        public string Welcome()
        {
            return "This is the Welcome action method...";
        }
    }
}