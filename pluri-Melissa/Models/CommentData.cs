using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace Project.Models
{
    //this should look identical to the ML models struct



    public class CommentData
    {
        [LoadColumn(0)]
        public string? SentimentText { get; set; }

        [LoadColumn(1)]
        public float Sentiment { get; set; }

    }
}
