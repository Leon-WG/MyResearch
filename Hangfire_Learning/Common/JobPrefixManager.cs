namespace Common
{
    using System;
    using Util;
    using Extensions;

    public class JobPrefixManager
    {
        private static string defaultValue = "hf.amp";
        public static string GetPrefix()
        {
            return ConfigHelper.GetSetting(k => k.ScheduleIdPrefix, defaultValue);
        }

        public static string GetJobId(string jobId)
        {
            if (string.IsNullOrWhiteSpace(jobId))
            {
                throw new ArgumentException(jobId);
            }

            var prefix = GetPrefix().ToLower();
            jobId = jobId.ToLower();
            return (jobId.StartsWith(prefix)) ? jobId : ($"{GetPrefix()}.{jobId}");
        }

        public static string RemovePrefix(string jobId)
        {
            var prefix = (GetPrefix()+".").ToLower();
            return jobId.RemovePrefix(prefix);
        }
    }
}
