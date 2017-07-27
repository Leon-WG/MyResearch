using System;
using System.Collections.Generic;


using System.Data.SqlClient;

namespace Common.Repositories
{
    using Models;
    using Extensions;

    public class AdoBase : IDisposable
    {
        private string _connStrKey;
        private SqlConnection _conn;

        public AdoBase(string connStrKey)
        {
            _connStrKey = connStrKey;
        }

        public void Dispose()
        {
            if (_conn != null) {
                _conn.Dispose();
                _conn = null;
            }
        }

        protected void Open()
        {
            var conStr = System.Configuration.ConfigurationManager.ConnectionStrings[_connStrKey].ConnectionString;
            _conn = new SqlConnection(conStr);
            _conn.Open();
        }

        protected void Execute(string sql, Action<SqlDataReader> readFn)
        {
            Open();
            using (var cmd = new SqlCommand(sql, _conn))
            {
                var reader = cmd.ExecuteReader();
                readFn(reader);
            }
        }
    }
    public class HangfireRepository : AdoBase
    {
        private string  keyPrefix = "recurring-job:";

        public HangfireRepository() : base(Util.Key.ConnectionStrs.HangfireConnStr)
        { }

        public List<ScheduleModel> AllSchedules()
        {
        
            var sql = "select [Key], "+
             "max(case Field when 'Job' then[Value]  end) as Job,"+
             "max(case Field when 'Cron' then[Value]  end) as Cron," +
             "max(case Field when 'TimeZoneId' then[Value] end) as TimeZoneId," +
             "max(case Field when 'Queue' then[Value]  end) as 'Queue'," +
             "max(case Field when 'CreatedAt' then[Value]  end) as CreatedAt," +
             "max(case Field when 'NextExecution' then[Value]  end) as NextExecution " +
             "from [HangFire].[HangFire].[Hash] " +
             "group by[Key]";

            

            var result = new List<ScheduleModel>();
            this.Execute(sql, reader => {
                while (reader.Read()) {

                    
                    var item = new ScheduleModel();

                    item.Id          = reader.GetString(0).RemovePrefix(keyPrefix);
                    item.Job         = reader.GetString(1);
                    item.Cron        = reader.GetString(2);
                    item.TimeZone    = reader.GetString(3);
                    item.Queue       = reader.GetString(4);
                    item.CreateAtStr = reader.GetString(5);
                    item.CreateAt    = reader.GetString(5).ToDatetime();
                    
                    item.NextExecStr = reader[6].ToString();
                    item.NextExec    = item.NextExecStr.ToDatetime();
                    
                    result.Add(item);
                }
                
            });

            return result;
        }

        public ScheduleModel FindSchedule(string id)
        {
            ScheduleModel result = null;

            if (string.IsNullOrWhiteSpace(id)) return result;

            var sql = $"select * from [HangFire].[HangFire].[Hash] where [Key] = '{keyPrefix+id.Trim()}'";
            this.Execute(sql, reader => {
                while (reader.Read()){
                    if (result == null) result = new ScheduleModel { Id = id};
                    var filed = reader.GetString(2);
                    var value = reader.GetString(3);
                    switch (filed)
                    {
                        case "Job":         result.Job = value;      break;
                        case "Cron":        result.Cron = value;     break;
                        case "TimeZoneId":  result.TimeZone = value; break;
                        case "Queue":       result.Queue = value;    break;
                        case "CreatedAt":
                            result.CreateAt = value.ToDatetime();
                            result.CreateAtStr = value;
                            break;
                        case "NextExecution":
                            result.NextExec = value.ToDatetime();
                            result.NextExecStr = value;
                            break;
                    }
                }
            });

            return result;
        }


        public List<string> AllScheduleIDs()
        {
            var sql = " select distinct [key] from [HangFire].[HangFire].[Hash]";

            var result = new List<string>();

            this.Execute(sql, reader => {
                while (reader.Read()) {
                    result.Add(reader.GetString(0).RemovePrefix(keyPrefix));
                }
            });

            return result;
        }

        public bool Exists(string id)
        {
            var result = false;

            if (string.IsNullOrEmpty(id))
            {
                return result;
            }

            var sql = $" select * from [HangFire].[HangFire].[Hash] where [key] = '{id}'";
            
            this.Execute(sql, reader =>  result = reader.HasRows);

            return result;
        }
    }
}
