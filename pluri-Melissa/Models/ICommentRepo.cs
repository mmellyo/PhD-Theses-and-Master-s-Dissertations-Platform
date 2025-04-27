using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public interface ICommentRepo
    {
        bool AddCommentInDb(int TheseId, string commentText, int UserId);
        List<Comment> GetComments();
        void DeleteComment(int commentId);
        void UpdateComment(int commentId, string newCommentText);
    }
}
