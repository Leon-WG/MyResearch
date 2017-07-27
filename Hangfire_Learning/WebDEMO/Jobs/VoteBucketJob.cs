


namespace WebDEMO.Jobs
{
    using System;
    using Repositories;
    public class VoteBucketJob : JobBase
    {
        public VoteBucketJob()
        {
            this._Cron = "0/1 * * * *";
        }
        

        public override void Executing()
        {
            new VoteRepository().CalcBuckets();
        }


    }
}