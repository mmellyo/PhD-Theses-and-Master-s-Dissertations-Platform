using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Project.Models
{
    public interface ICommentRepo
    {
        bool AddCommentInDb(int TheseId, string commentText, int UserId, int State);
        List<Comment> GetComments();
        void DeleteComment(int commentId);
        void UpdateComment(int commentId, string newCommentText);

        int GetCommentId(string comment);
    



    }
}
