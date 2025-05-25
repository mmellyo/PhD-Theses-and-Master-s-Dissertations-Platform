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
using gestion.Model;
using System.Collections.ObjectModel;
using System.Windows.Documents;


namespace Project.Repos
{
    public class CommentRepo : RepoBase, ICommentRepo
    {



        public bool AddCommentInDb(int TheseId, string commentText, int UserId, int State)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;




                command.CommandText = "INSERT INTO Comments (content, commented_by, commented_on, state) VALUES (@comment_text, @user_id, @these_id, @state)";
                //prevent SQL injection
                command.Parameters.Add("@comment_text", MySqlDbType.VarChar).Value = commentText;
                command.Parameters.Add("@user_id", MySqlDbType.Int32).Value = UserId; 
                command.Parameters.Add("@these_id", MySqlDbType.Int32).Value = TheseId;
                command.Parameters.Add("@state", MySqlDbType.Int32).Value = State;



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

        public List<Comment> LoadTheseComments(int theseId)
        {
            var comments = new List<Comment>();

            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
                                        SELECT u.user_id, u.user_name, c.comment_text, u.user_profilepic
                                        FROM comments c
                                        JOIN user u ON c.user_id = u.user_id
                                        WHERE c.these_id = @TheseId 
                                        AND c.state NOT IN (1, 3);
        ";

                command.Parameters.AddWithValue("@TheseId", theseId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comments.Add(new Comment
                        {
                            UserId = reader.GetInt32("user_id"),
                            Username = reader.GetString("user_name"),
                            CommentText = reader.GetString("comment_text"),
                            user_profilepic = reader["user_profilepic"] == DBNull.Value ? null : (byte[])reader["user_profilepic"]
                        });
                    }
                }
            }

            return comments;
        }


        public List<CommentModel> LoadTheseComments2(int theseId)
        {
            var comments = new List<CommentModel>();

            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
                                        SELECT u.user_id, u.user_name, c.content, u.user_profilePic, c.comment_id
                                        FROM comments c
                                        JOIN users u ON c.commented_by = u.user_id
                                        WHERE c.commented_on = @TheseId 
                                        AND c.state NOT IN (1, 3);
        ";

                command.Parameters.AddWithValue("@TheseId", theseId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comments.Add(new CommentModel
                        {
                            comment_id = reader.GetInt32("comment_id"),
                            user_id = reader.GetInt32("user_id"),
                            user_name = reader.GetString("user_name"),
                            content = reader.GetString("content"),
                            user_image = reader["user_profilepic"] == DBNull.Value ? null : (byte[])reader["user_profilepic"]
                        });
                    }
                }
            }

            return comments;
        }

        public List<CommentModel> LoadAutoFlaggedComments()
        {
            var comments = new List<CommentModel>();

            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
                                        SELECT u.user_id, u.user_name, c.content, u.user_profilePic, c.comment_id
                                        FROM comments c
                                        JOIN users u ON c.commented_by = u.user_id
                                        WHERE c.state = 1;
                                        ";


                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comments.Add(new CommentModel
                        {
                            comment_id = reader.GetInt32("comment_id"),
                            user_id = reader.GetInt32("user_id"),
                            user_name = reader.GetString("user_name"),
                            content = reader.GetString("content"),
                            user_image = reader["user_profilepic"] == DBNull.Value ? null : (byte[])reader["user_profilepic"]
                        });
                    }
                }
            }

            return comments;
        }


        public void UpdateComment(int commentId, string newCommentText)
        {
        }

        public int GetCommentId(string comment)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "SELECT comment_id FROM `comments` WHERE content=@comment_text";
                command.Parameters.Add("@comment_text", MySqlDbType.VarChar).Value = comment;
                var CommentId = command.ExecuteScalar();
                return Convert.ToInt32(CommentId);
            }
        }


        public List<CommentModel> GetUserComments(int user_id)
        {
            List<CommentModel> comments = new List<CommentModel>();

            using (var connection = GetConnection()) 
            using (MySqlCommand command = new MySqlCommand(@"SELECT comment_id, content, commented_on, commented_by 
                         FROM comments 
                         WHERE commented_by = @userId", connection))
                {
                    connection.Open();

                    command.Parameters.AddWithValue("@userId", user_id);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CommentModel comment = new CommentModel
                            {
                                comment_id = reader.GetInt32("comment_id"),
                                content = reader.GetString("content"),
                                user_id = reader.GetInt32("commented_by"),
                                article_id = reader.GetInt32("commented_on")
                                
                            };

                            comments.Add(comment);
                        }
                    }
                }
            return comments;
        }

            
  
        


        public bool UpdateCommentState(int commentId, int newState)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;

                    command.CommandText = "UPDATE comments SET state = @state WHERE comment_id = @comment_id";
                    command.Parameters.AddWithValue("@state", newState);
                    command.Parameters.AddWithValue("@comment_id", commentId); 

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error updating comment state: {ex.Message}");

                return false;
            }
        }

        internal List<int>? GetCommentId(int userid)
        {
            List<int> comments = new List<int>();

            using (var connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(@"SELECT comment_id 
                         FROM comments 
                         WHERE commented_by = @userId", connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@userId", userid);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int comment_id = reader.GetInt32("comment_id");
                        comments.Add(comment_id);
                    }
                }
            }
            return comments;
        }
    }
}
