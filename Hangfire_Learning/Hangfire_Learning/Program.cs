using System;


namespace Hangfire_Learning
{
    using Common;
    using Common.Repositories;
    using Hangfire;

    class Program
    {
        static void Main(string[] args)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("hangfireConnStr");

            //F1();
            //F2();
            //F3();
            //F4();

            //F5();
            //F6();
            //F7();


            RecurringJobAmp.RemoveIfExists("hf.amp.votebucketjob");
            RecurringJobAmp.RemoveIfExists("hf.amp.votepercentagjob");

            //var repo = new HangfireRepository();
            //var schedules = repo.AllSchedules();
            //foreach (var s in schedules)
            //{
            //    Console.WriteLine($"{s.Id}  {s.Cron}  {s.Queue}  {s.CreateAt}  {s.NextExec}");
            //}


        }

        public static void F1()
        {   
            //Fire-and-forget
            var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Fire and forget"));
            Console.WriteLine("JobID:{0}", jobId);
        
            //perform jobs
            PerformJobs();
        }

        public static void F2()
        {
            var jobId = BackgroundJob.Schedule(()=>Console.WriteLine("Delay to run..."),TimeSpan.FromMinutes(2));
            Console.WriteLine("JobID:{0}", jobId);
            
            //By default, check interval is equal to 15 seconds
            var op = new BackgroundJobServerOptions { SchedulePollingInterval=TimeSpan.FromMinutes(1)};
            PerformJobs(op);
        }

        public static void F3()
        {
            RecurringJob.AddOrUpdate("my-id",() => Console.WriteLine("minutely recurring job..."), Cron.Minutely);

            PerformJobs();

            //RecurringJob.RemoveIfExists("my-id");
            //RecurringJob.Trigger("my-id");
        }

        public static void F4()
        {
            RecurringJob.AddOrUpdate("my-id2", () => Console.WriteLine("minutely recurring job..."), "0/20 * * * *");

            PerformJobs();
        }

        private static void PerformJobs(BackgroundJobServerOptions op=null)
        {
            var server = (op==null)?  new BackgroundJobServer() : new BackgroundJobServer(op);
            
            
            Console.WriteLine("press [Q] to Quit");
            var answer = Console.ReadLine();
            if (answer.ToLower() == "q")
            {
                server.Dispose();
            }

        //    using (var server = new BackgroundJobServer())
        //    {
        //        Console.WriteLine("Hangfire Server started. Press any key to exit...");
        //        Console.ReadKey();
        //    }
        }

        public static void F5()
        {
            RecurringJobAmp.AddOrUpdate(() => Console.WriteLine("test fn 5"), "0/5 * * * *");

            PerformJobs();
        }

        public static void F6()
        {
            RecurringJobAmp.AddOrUpdate("my_job_1", () => Console.WriteLine("test fn 6"), "0/5 9-21 * * *");

            PerformJobs();
        }

        public static void F7()
        {
            //Cron.Hourly               ->  0 * * * *
            //Cron.MinuteInterval(20)   ->  */20 * * * *
            //Corn.Yearly               ->  0 0 1 1 *

            RecurringJobAmp.AddOrUpdate("my_job_2", () => Console.WriteLine("test fn 7"), Cron.Yearly);

            PerformJobs();
        }

    }
}
