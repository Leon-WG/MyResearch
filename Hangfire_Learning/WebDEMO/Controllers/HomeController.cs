using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDEMO.Controllers
{
    using Common.Repositories;
    using Hangfire;
    using Repositories;

    public class HomeController : Controller
    {
        

        // GET: Home
        public ActionResult Index()
        {
            return View(QuestionModel.GetInstance());
           
        }

        public ActionResult Vote(string key)
        {
            var repo = new VoteRepository();
            repo.AddVote(key);

            return RedirectToAction("Index", "Home");
        }



      
    }
}