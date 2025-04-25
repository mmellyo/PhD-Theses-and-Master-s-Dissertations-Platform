using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Comment
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string CommentText { get; set; }
        public int TheseId { get; set; }

    }
}
