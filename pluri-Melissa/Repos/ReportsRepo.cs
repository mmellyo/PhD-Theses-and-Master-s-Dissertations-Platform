using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Project.Models;

namespace Project.Repos
{
    public class ReportsRepo : RepoBase
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


                command.CommandText = "INSERT INTO reports (comment_reported,reported_by, report_type) VALUES (@reported_id, @reporter_id, @reported_type)";
                command.Parameters.AddWithValue("@reported_id", CommentId);

                command.Parameters.AddWithValue("@reported_type", "Comment");

                if (UserId.HasValue)
                    command.Parameters.AddWithValue("@reporter_id", UserId.Value);
                else
                    command.Parameters.AddWithValue("@reporter_id", DBNull.Value);
                command.ExecuteNonQuery();
            }
        }


        public List<CommentReportModel> LoadManFlaggedComments()
        {
            var comments = new List<CommentReportModel>();

            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
                                        SELECT u.user_name, c.content, u.user_profilePic, c.comment_id, r.report_id, r.content AS message, r.reported_by, r.reason
                                        FROM comments c
                                        JOIN users u ON c.commented_by = u.user_id
                                        JOIN reports r ON c.comment_id = r.comment_reported
                                        WHERE c.state = -1;
                                        ";


                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comments.Add(new CommentReportModel
                        {
                            comment_id = reader.GetInt32("comment_id"),
                            commentor_name = reader.GetString("user_name"),
                            Content = reader.GetString("message"),
                            comment = reader.GetString("content"),
                            reason = reader.GetString("reason"),
                            Reporter = reader.GetInt32("reported_by"),
                            Id = reader.GetInt32("report_id"),
                            user_image = reader["user_profilepic"] == DBNull.Value ? null : (byte[])reader["user_profilepic"]


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
                command.CommandText = "INSERT INTO reports (reported_id, reported_type, description) VALUES (@id, @type, @desc)";
                command.Parameters.AddWithValue("@id", thesisId);
                command.Parameters.AddWithValue("@type", "Thesis");
                command.Parameters.AddWithValue("@desc", description);
                command.ExecuteNonQuery();
            }
        }

        public int getReportIdFromComment (int comment_id)
        {
            int reportid = -1;

            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT report_id FROM reports  WHERE comment_reported = @comment_id";
                command.Parameters.AddWithValue("@comment_id", comment_id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        reportid = reader.GetInt32("report_id");
                    }
                }

            }

            return reportid;
        }

        public void SubmitReport(int article_id, int user_id, string finalReason, string selected, string v)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO reports (content, report_type, date_reported, reported_by, article_reported, reason) VALUES (@content, @type, @date, @by, @article, @reason)";
                command.Parameters.AddWithValue("@content", finalReason);
                command.Parameters.AddWithValue("@type", v);
                command.Parameters.AddWithValue("@date", DateTime.Now);
                command.Parameters.AddWithValue("@by", user_id);
                command.Parameters.AddWithValue("@article", article_id);
                command.Parameters.AddWithValue("@reason", selected);
                command.ExecuteNonQuery();
            }
        }
    }
}
