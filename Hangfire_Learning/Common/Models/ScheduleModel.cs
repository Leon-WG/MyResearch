using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ScheduleModel
    {
        public string Id { get; set; }

        public string Job { get; set; }

        public string Cron { get; set; }

        public string TimeZone { get; set; }

        public string Queue     { get; set; }
        public DateTime? CreateAt { get; set; }

        public string CreateAtStr { get; set; }

        public DateTime? NextExec { get; set; }

        public string NextExecStr { get; set; }

        public bool Disabled { get; set; }

        public ScheduleModel SetDisabled(bool disabled)
        {
            Disabled = disabled;
            return this;
        }
    }
}
