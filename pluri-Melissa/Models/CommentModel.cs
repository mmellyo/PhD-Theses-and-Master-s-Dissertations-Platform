using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Project.Models
{
    public class CommentModel
    {
        public int comment_id {  get; set; }
        public string content { get; set; }
        public int user_id { get; set; }
        public string user_name { get; set; }
        public Byte[] user_image { get; set; }
        public int article_id { get; set; }

        public int reply_id { get; set; }

    }
}
