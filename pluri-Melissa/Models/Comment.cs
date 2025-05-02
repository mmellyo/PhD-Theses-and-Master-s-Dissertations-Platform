using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project.Models
{
    public class Comment
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string CommentText { get; set; }
        public int TheseId { get; set; }
        public int UserId { get; set; }
        public int commentId { get; set; }
        public byte[] user_profilepic { get; set; }

        public int State { get; set; } // 1 = flagged, 2 = approved, 3 = denied

        // Commands for UI binding
        public ICommand ApproveCommand { get; set; }
        public ICommand DenyCommand { get; set; }
    }
}
