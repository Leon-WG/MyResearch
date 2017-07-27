
using System.Collections.Generic;
using System.Linq;

namespace WebDEMO.Jobs
{
    using Common;
    using Common.Models;
    using Common.Repositories;

    public class JobManager
    {

        private static HangfireRepository _repo = new HangfireRepository();

        private static List<JobBase> _jobItems = new List<JobBase>() {
            new VoteBucketJob(),
            new VotePercentagJob()
        };

        public static List<JobBase> Jobs
        {
            get
            {
                return _jobItems;
            }
        }

        public static JobBase FindById(string id)
        {
            var idWithoutPrefix = JobPrefixManager.RemovePrefix(id);
            foreach(var i in _jobItems)
            {
                var itemId = i.GetJobID().ToLower();
                if (itemId == id || itemId == idWithoutPrefix)
                {
                    return i;
                }
            }

            return null;
        }

        public static ScheduleModel FindSchedule(string id)
        {
            var memeoryJob = FindById(id);
            var persistJob = _repo.FindSchedule(id);

            if (persistJob != null)
            {
                return persistJob;
            }
            else if (memeoryJob != null)
            {
                return memeoryJob.ToScheduleModel().SetDisabled(true);
            }
            else
            {
                return null;
            }
        }

        public static void SetupJobs()
        {
            var id_memoryJobs = _jobItems.Select(i => JobPrefixManager.GetJobId(i.GetJobID())).ToList();
            var id_persistedJobs = _repo.AllScheduleIDs();

            var creatingIDs = id_memoryJobs.Except(id_persistedJobs);
            foreach (var id in creatingIDs)
            {
                FindById(id).Update();
            }


            var deletingIDs = id_persistedJobs.Except(id_persistedJobs);

            foreach (var id in deletingIDs)
            {
                RecurringJobAmp.RemoveIfExists(id);
            }
        }

        public static List<ScheduleModel> AllJobs()
        {
            var memoryJobs = _jobItems.Select(i => i.ToScheduleModel()).ToList();
            var persistedJobs = _repo.AllSchedules();

            var id_persistedJobs = persistedJobs.Select(i => i.Id);
            var others = memoryJobs.Where(i => !id_persistedJobs.Contains(i.Id)).Select(i=>i.SetDisabled(true)).ToArray();
            persistedJobs.AddRange(others);
            
            return persistedJobs;
        }

        
    }
}