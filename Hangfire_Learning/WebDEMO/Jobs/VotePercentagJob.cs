

namespace WebDEMO.Jobs
{
    using Repositories;

    public class VotePercentagJob : JobBase
    {

        public VotePercentagJob()
        {
            this._Cron = "0/1 * * * *";
        }


        public override void Executing()
        {
            new VoteRepository().CalcPercentage();
        }
    }
}