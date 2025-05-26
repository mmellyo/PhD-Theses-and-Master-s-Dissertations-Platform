using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class CommentReportModel
    {
        public int Id { get; set; }
        public int Reporter { get; set; }
        public string Content { get; set; }
        public int reported_id { get; set; }
        public string reported_type { get; set; }
        public string reported_date { get; set; }
        public string reported_description { get; set; }
        public string reason { get; set; }


        public string comment {  get; set; }
        public string commentor_name { get; set; }
        public int comment_id { get; set; } 
        public byte[] user_image { get; set; }
    }
}
