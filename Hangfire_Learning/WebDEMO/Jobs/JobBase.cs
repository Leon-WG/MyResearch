
using Common;
using Common.Models;

namespace WebDEMO.Jobs
{
    public abstract class JobBase
    {
        protected string _Cron;

        protected string _jobId;

        protected string _queueName = "default";

        public JobBase()
        {
            _Cron = "0 0/1 * * *";
        }

     
        public JobBase SetQueueName(string qName)
        {
            if (!string.IsNullOrWhiteSpace(qName))
            {
                _queueName = qName;
            }
            return this;
        }

        public JobBase SetCron(string cron)
        {
            if (!string.IsNullOrWhiteSpace(cron))
            {
                _Cron = cron;
            }
            return this;
        }

        public void Execute()
        {
            Executing();
        }

        public string GetJobID()
        {
            if (_jobId == null)
            {
                _jobId = this.GetType().Name;
            }
            return _jobId;
        }

        public abstract void Executing();


        public void Update()
        {
            RecurringJobAmp.AddOrUpdate(this.GetJobID(), () => this.Execute(), this._Cron, null, _queueName);
        }

        public void Disable()
        {
            RecurringJobAmp.RemoveIfExists(this.GetJobID());
        }

        public ScheduleModel ToScheduleModel()
        {
            var model = new ScheduleModel();
            model.Id =  JobPrefixManager.GetJobId(this.GetJobID());
            model.Cron = this._Cron;
            model.Queue = this._queueName;
            return model;
        }
    }
}