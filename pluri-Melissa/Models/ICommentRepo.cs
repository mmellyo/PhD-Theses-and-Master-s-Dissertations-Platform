using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Project.Models
{
    public interface ICommentRepo
    {
        bool AddCommentInDb(int TheseId, string commentText, int UserId, int State);
        public List<Comment> LoadTheseComments(int theseId);
        void DeleteComment(int commentId);
        void UpdateComment(int commentId, string newCommentText);
        public bool UpdateCommentState(int commentId, int newState);
        int GetCommentId(string comment);
    



    }
}
