using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Models;
using System.Data;
using System.Net;
using Project.Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;


namespace Project.Repos
{
    public class CommentRepo : RepoBase, ICommentRepo
    {



        public bool AddCommentInDb(int TheseId, string commentText, int UserId)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;




                command.CommandText = "INSERT INTO Comments (comment_text, user_id, these_id) VALUES (@comment_text, @user_id, @these_id)";
                //prevent SQL injection
                command.Parameters.Add("@comment_text", MySqlDbType.VarChar).Value = commentText;
                command.Parameters.Add("@user_id", MySqlDbType.Int32).Value = UserId; 
                command.Parameters.Add("@these_id", MySqlDbType.Int32).Value = TheseId;
                

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected == 1;
            }
        }




        public void ReportThesis(int thesisId, string description)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO Reports (reported_id, reported_type, description) VALUES (@id, @type, @desc)";
                command.Parameters.AddWithValue("@id", thesisId);
                command.Parameters.AddWithValue("@type", "Thesis");
                command.Parameters.AddWithValue("@desc", description);
                command.ExecuteNonQuery();
            }
        }







        public void DeleteComment(int commentId)
        {
        }

        public List<Comment> GetComments()
        {
            throw new NotImplementedException();
        }

        

        public void UpdateComment(int commentId, string newCommentText)
        {
        }
    }
}
