using System;
using System.Collections.Generic;


namespace WebDEMO.Repositories
{


    public class QuestionModel
    {
        public string Question = "what color you'd like?";

        public List<string> Options =new List<string> { "Red", "Blue","Black","White","Blue"};

        public List<VoteItem> voteList = new List<VoteItem>();

        public Dictionary<string, int> Buckets = new Dictionary<string, int>();
        public DateTime? LastBucketingTime;

        public Dictionary<string, float> Percentage = new Dictionary<string, float>();
        public DateTime? LastPercentageTime;


        private QuestionModel()
        { }

        private static Lazy<QuestionModel> _instance = new Lazy<QuestionModel>(()=> new QuestionModel());
        public static QuestionModel GetInstance()
        {
            return _instance.Value;
        }

    }
}