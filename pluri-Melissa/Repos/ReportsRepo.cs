using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Project.Models;

namespace Project.Repos
{
    public class ReportsRepo : RepoBase, IReportsRepo
    {
        public void ReportAccount(int Account_Id, string description)
        {
            throw new NotImplementedException();
        }

        public void ReportComment(int CommentId, int? UserId)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;


                command.CommandText = "INSERT INTO reports (reported_id,reporter_id, reported_type) VALUES (@reported_id, @reporter_id, @reported_type)";
                command.Parameters.AddWithValue("@reported_id", CommentId);

                command.Parameters.AddWithValue("@reported_type", "Comment");

                if (UserId.HasValue)
                    command.Parameters.AddWithValue("@reporter_id", UserId.Value);
                else
                    command.Parameters.AddWithValue("@reporter_id", DBNull.Value);
                command.ExecuteNonQuery();
            }
        }


        public List<Comment> LoadFlaggedComments()
        {
            var comments = new List<Comment>();

            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = @"
                                        SELECT c.comment_text, c.these_id, c.state
                                        FROM reports r
                                        JOIN comments c ON c.comment_id = r.reported_id
                                        WHERE r.reported_type = 'Comment'
                                        AND c.state != 2

        ";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comments.Add(new Comment
                        {
                            CommentText = reader.GetString("comment_text"),
                            TheseId = reader.GetInt32("these_id")
                        });
                    }
                }
            }

            return comments;
        }



        //not verified yet
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

    }
}
