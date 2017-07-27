using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDEMO.Controllers
{
    using Common.Models;
    using Common.Repositories;

    public class ScheduleController : Controller
    {
        // GET: Schedule
        public ActionResult Index()
        {

            var data = Jobs.JobManager.AllJobs();

            return View(data);
        }

     
        public ActionResult Update(string id)
        {
            id = id.Replace("___", ".");
            
            var model = Jobs.JobManager.FindSchedule(id);
            if (model == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        public ActionResult Modify(FormCollection form)
        {
            var id       = form["id"];
            var cron     = form["cron"];
            var quene    = form["queue"];
            var disabled = form["disabled"]=="on";

            if (!string.IsNullOrWhiteSpace(id))
            {
                var jobItem = Jobs.JobManager.FindById(id);
                jobItem?.SetCron(cron).SetQueueName(quene).Update();

                if (disabled)
                {
                    jobItem?.Disable();
                }
            }

            return RedirectToAction("Index", "Schedule");
        }
    }
}