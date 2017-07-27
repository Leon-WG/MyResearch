
using System;
using System.Threading.Tasks;

namespace Common
{
    using Hangfire;
    using Hangfire.Common;
    using Extensions;
    using System.Linq.Expressions;

    public class RecurringJobAmp
    {
        private static readonly Lazy<RecurringJobManager> Instance = new Lazy<RecurringJobManager>((Func<RecurringJobManager>)(() => new RecurringJobManager()));

        private static string GetRecurringJobId(Job job)
        {
            return string.Format("{0}.{1}", (object)job.Type.ToGenericTypeString(), (object)job.Method.Name);
        }

        public static void AddOrUpdate(Expression<Action> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            RecurringJob.AddOrUpdate(methodCall, cronExpression(), timeZone, queue);
        }

        public static void AddOrUpdate<T>(Expression<Action<T>> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            RecurringJob.AddOrUpdate<T>(methodCall, cronExpression(), timeZone, queue);
        }

        public static void AddOrUpdate(Expression<Action> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            Job job = Job.FromExpression(methodCall);
            string recurringJobId = JobPrefixManager.GetJobId(GetRecurringJobId(job));
            Instance.Value.AddOrUpdate(recurringJobId, job, cronExpression, timeZone ?? TimeZoneInfo.Utc, queue);
        }

        public static void AddOrUpdate<T>(Expression<Action<T>> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            Job job = Job.FromExpression<T>(methodCall);
            string recurringJobId = JobPrefixManager.GetJobId(GetRecurringJobId(job));
            Instance.Value.AddOrUpdate(recurringJobId, job, cronExpression, timeZone ?? TimeZoneInfo.Utc, queue);
        }

        public static void AddOrUpdate(string recurringJobId, Expression<Action> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            RecurringJob.AddOrUpdate(JobPrefixManager.GetJobId(recurringJobId), methodCall, cronExpression(), timeZone, queue);
        }

        public static void AddOrUpdate<T>(string recurringJobId, Expression<Action<T>> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            RecurringJob.AddOrUpdate<T>(recurringJobId, methodCall, cronExpression(), timeZone, queue);
        }

        public static void AddOrUpdate(string recurringJobId, Expression<Action> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            Job job = Job.FromExpression(methodCall);
            Instance.Value.AddOrUpdate(JobPrefixManager.GetJobId(recurringJobId), job, cronExpression, timeZone ?? TimeZoneInfo.Utc, queue);
        }

        public static void AddOrUpdate<T>(string recurringJobId, Expression<Action<T>> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            Job job = Job.FromExpression<T>(methodCall);
            Instance.Value.AddOrUpdate(JobPrefixManager.GetJobId(recurringJobId), job, cronExpression, timeZone ?? TimeZoneInfo.Utc, queue);
        }

        public static void AddOrUpdate(Expression<Func<Task>> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            AddOrUpdate(methodCall, cronExpression(), timeZone, queue);
        }

        public static void AddOrUpdate<T>(Expression<Func<T, Task>> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            AddOrUpdate<T>(methodCall, cronExpression(), timeZone, queue);
        }

        public static void AddOrUpdate(Expression<Func<Task>> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            Job job = Job.FromExpression(methodCall);
            string recurringJobId = JobPrefixManager.GetJobId(GetRecurringJobId(job));
            Instance.Value.AddOrUpdate(recurringJobId, job, cronExpression, timeZone ?? TimeZoneInfo.Utc, queue);
        }

        public static void AddOrUpdate<T>(Expression<Func<T, Task>> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            Job job = Job.FromExpression<T>(methodCall);
            string recurringJobId = JobPrefixManager.GetJobId(GetRecurringJobId(job));
            Instance.Value.AddOrUpdate(recurringJobId, job, cronExpression, timeZone ?? TimeZoneInfo.Utc, queue);
        }

        public static void AddOrUpdate(string recurringJobId, Expression<Func<Task>> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            AddOrUpdate(JobPrefixManager.GetJobId(recurringJobId), methodCall, cronExpression(), timeZone, queue);
        }

        public static void AddOrUpdate<T>(string recurringJobId, Expression<Func<T, Task>> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            AddOrUpdate<T>(JobPrefixManager.GetJobId(recurringJobId), methodCall, cronExpression(), timeZone, queue);
        }

        public static void AddOrUpdate(string recurringJobId, Expression<Func<Task>> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            Job job = Job.FromExpression(methodCall);
            Instance.Value.AddOrUpdate(JobPrefixManager.GetJobId(recurringJobId), job, cronExpression, timeZone ?? TimeZoneInfo.Utc, queue);
        }

        public static void AddOrUpdate<T>(string recurringJobId, Expression<Func<T, Task>> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            Job job = Job.FromExpression<T>(methodCall);
            Instance.Value.AddOrUpdate(JobPrefixManager.GetJobId(recurringJobId), job, cronExpression, timeZone ?? TimeZoneInfo.Utc, queue);
        }

        public static void RemoveIfExists(string recurringJobId)
        {
            Instance.Value.RemoveIfExists(JobPrefixManager.GetJobId(recurringJobId));
        }

        public static void Trigger(string recurringJobId)
        {
            Instance.Value.Trigger(JobPrefixManager.GetJobId(recurringJobId));
        }
    }
}
