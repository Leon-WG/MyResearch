using System;
using System.Collections.Generic;

namespace WebDEMO.Repositories
{
    public class VoteRepository
    {
        public void AddVote(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            var model = QuestionModel.GetInstance();
            if (model.Options.Contains(key))
            {
                model.voteList.Add(new VoteItem { SubmitDate = DateTime.Now, Key = key});
            }
        }

        public void CalcBuckets()
        {

            var model = QuestionModel.GetInstance();

            var newBucket = new Dictionary<string, int>();

            foreach (var item in model.voteList)
            {
                if (newBucket.ContainsKey(item.Key))
                {
                    newBucket[item.Key] = ++newBucket[item.Key];
                }
                else
                {
                    newBucket[item.Key] = 1;
                }
            }


            model.Buckets = newBucket;
            model.LastBucketingTime = DateTime.Now;
        }

        public void CalcPercentage()
        {
            var model = QuestionModel.GetInstance();

            var total = model.voteList.Count;

            if (total == 0)
            {
                return;
            }

            var newPercentage = new Dictionary<string, float>();

            float point = 1f / total;

            foreach (var item in model.voteList)
            {
                if (newPercentage.ContainsKey(item.Key))
                {
                    newPercentage[item.Key] = (point + newPercentage[item.Key]);
                }
                else
                {
                    newPercentage[item.Key] = point;
                }
            }


            model.Percentage = newPercentage;
            model.LastPercentageTime = DateTime.Now;
        }
    }
}