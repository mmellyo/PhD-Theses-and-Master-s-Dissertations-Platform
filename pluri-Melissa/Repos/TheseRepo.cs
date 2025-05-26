using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using Project.Models;
using Project.View.userControls;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace Project.Repos
{
    public class TheseRepo : RepoBase, ITheseRepo
    {
        public void ShowPdf(int article_id)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "SELECT article_file FROM `articles` WHERE article_id = @article_id";

                command.Parameters.AddWithValue("@article_id", article_id);
                var result = command.ExecuteScalar();

                if (result != DBNull.Value && result is byte[] pdfBytes && pdfBytes.Length > 0)
                {
                    string tempFilePath = Path.Combine(Path.GetTempPath(), $"thesis_{article_id}.pdf");
                    File.WriteAllBytes(tempFilePath, pdfBytes);
                    Process.Start(new ProcessStartInfo(tempFilePath) { UseShellExecute = true });
                }
                else
                {
                    // Optional: show a message to the user
                    MessageBox.Show("No PDF data found for this thesis.");
                }
            }
        }


        public string GetNomEncadrantFromTheseId(int these_id)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT NomEncadrant FROM `theses` WHERE these_id = @these_id";
                command.Parameters.AddWithValue("@these_id", these_id);

                var result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    return result.ToString();
                }
                else
                {
                    MessageBox.Show("No encadrant name found for this thesis.");
                    return null;
                }
            }
        }

        public List<int> getSupervisorArticles(int supervisorId)
        {
            List<int> theses_id = new List<int>();

            string query = @"
                SELECT a.article_id
                FROM articles a
                JOIN supervised_by sb ON a.article_id = sb.article_id
                WHERE sb.supervisor_id = @supervisorId AND a.article_state ='Not Approved';
                ";

            using (var connection = GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@supervisorId", supervisorId);
                connection.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int articleId = reader.GetInt32("article_id");
                        theses_id.Add(articleId);
                    }
                }
            }

            return theses_id;
        }

        public ArticleModel GetThesisDetails(int articleId)
        {
            var thesis = new ArticleModel();

            using (var connection = GetConnection())
            using (var command = new MySqlCommand(@"
                SELECT 
                a.article_id, a.title, a.summary, a.article_date,
                a.department,
                a.faculty,
                a.language, a.article_type, a.article_file, a.visit_number, a.saves_number
                FROM articles a
                
                WHERE a.article_id = @articleId", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@articleId", articleId);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    thesis.id = reader.GetInt32("article_id");
                    thesis.title = reader.GetString("title");
                    thesis.description = reader.GetString("summary");
                    thesis.date = reader.GetString("article_date");
                    thesis.department = reader.GetString("department");
                    thesis.faculty = reader.GetString("faculty");
                    thesis.language = reader.GetString("language");
                    thesis.type = reader.GetString("article_type");
                    thesis.visits = reader.GetInt32("visit_number");
                    thesis.saves = reader.GetInt32("saves_number");
                }
            }

            // 2. Get supervisor name
            using (var connection = GetConnection())
            using (var cmd = new MySqlCommand(@"
                    SELECT u.user_name
                    FROM supervised_by sb
                    JOIN users u ON sb.supervisor_id = u.user_id
                    WHERE sb.article_id = @articleId", connection))
            {
                connection.Open();
                cmd.Parameters.AddWithValue("@articleId", articleId);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                    thesis.supervisor = reader.GetString("user_name");
            }

            // 3. Get authors (both user and non-user)
            using (var connection = GetConnection())
            using (var cmd = new MySqlCommand(@"
                    SELECT CONCAT(wb.first_name, ' ', wb.last_name) AS name
                    FROM written_by wb
                    WHERE wb.article_id = @articleId", connection))
            {
                connection.Open();
                cmd.Parameters.AddWithValue("@articleId", articleId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    thesis.authors.Add(reader.GetString("name"));
            }

            // 4. Get keywords
            using (var connection = GetConnection())
            using (var cmd = new MySqlCommand(@"
                    SELECT k.keyword
                    FROM used_keywords uk
                    JOIN keywords k ON uk.keyword_id = k.keyword_id
                    WHERE uk.article_id = @articleId", connection))
            {
                connection.Open();
                cmd.Parameters.AddWithValue("@articleId", articleId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    thesis.keywords.Add(reader.GetString("keyword"));
            }
            // 5. Get uploader name and email and role
            using (var connection = GetConnection())
            using (var cmd = new MySqlCommand(@"
                    SELECT u.user_name, u.user_role, u.user_email, u.user_profilePic
                    FROM articles a
                    JOIN users u ON a.poster_id = u.user_id
                    WHERE a.article_id = @articleId", connection))
            {
                connection.Open();
                cmd.Parameters.AddWithValue("@articleId", articleId);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                    thesis.uploader = reader.GetString("user_name");
                    thesis.uploader_email = reader.GetString("user_email");
                    thesis.uploader_role = reader.GetString("user_role");
                    thesis.uploader_pic = reader["user_profilePic"] as byte[];
            }
            return thesis;
        }
       
        public long GetOrInsertKeyword(MySqlConnection conn, MySqlTransaction tx, string keyword)
        {
            // Check existence
            var checkCmd = new MySqlCommand("SELECT keyword_id FROM keywords WHERE keyword = @kw", conn, tx);
            checkCmd.Parameters.AddWithValue("@kw", keyword);
            object result = checkCmd.ExecuteScalar();
            if (result != null)
                return Convert.ToInt64(result);

            // Insert
            var insertCmd = new MySqlCommand("INSERT INTO keywords (keyword) VALUES (@kw)", conn, tx);
            insertCmd.Parameters.AddWithValue("@kw", keyword);
            insertCmd.ExecuteNonQuery();
            return insertCmd.LastInsertedId;
        }

        public List<int> GetThesesByUser(int user_id)
        {
            List<int> theses = new List<int>();

            string query = @"
                SELECT a.article_id
                FROM articles a
                LEFT JOIN supervised_by sb ON a.article_id = sb.article_id
                LEFT JOIN users u ON sb.supervisor_id = u.user_id
                WHERE a.poster_id = @userId;
                ";

            using (var connection = GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@userId", user_id);
                connection.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int articleId = reader.GetInt32("article_id");

                        theses.Add(articleId);
                    }
                }
            }

            return theses;
        }

        public List<int> GetSavedThesesByUser(int user_id)
        {
            List<int> theses = new List<int>();

            string query = @"
                        SELECT a.article_id
                        FROM saved_articles sa
                        JOIN articles a ON sa.article_id = a.article_id
                        WHERE sa.user_id = @userId";

            using (var connection = GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@userId", user_id);
                connection.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        int id = reader.GetInt32("article_id");
                        theses.Add(id);
                    }
                }
            }

            return theses;
        }
        public UserModel GetUploaderInfo(int articleId)
        {
            var uploader = new UserModel();

            using (var connection = GetConnection())

            using (var cmd = new MySqlCommand(@"
                    SELECT u.user_email, u.user_name, u.user_role, u.user_profilePic
                    FROM articles a
                    JOIN users u ON a.poster_id = u.user_id
                    WHERE a.article_id = @articleId", connection))
            {
                connection.Open();

                cmd.Parameters.AddWithValue("@articleId", articleId);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    uploader.user_email = reader.GetString("user_email");
                    uploader.user_name = reader.GetString("user_name");
                    uploader.user_role = reader.GetString("user_role");
                    uploader.user_profilepic = reader["user_profilePic"] as byte[];
                }
            }

            return uploader;
        }

        public string GetNomAuteurFromTheseId(int these_id)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                // Adjusted query with JOINs
                command.CommandText = @"
                            SELECT u.user_name
                            FROM users u
                            JOIN written_by_users wbu ON u.user_id = wbu.user_id
                            JOIN articles t ON wbu.article_id = t.article_id
                            WHERE t.article_id = @these_id";

                command.Parameters.AddWithValue("@these_id", these_id);

                var result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return result.ToString();
                }
                else
                {
                    MessageBox.Show("No author name found for this thesis.");
                    return null;
                }
            }
        }

        public void approveArticle(int article_id)
        {
            string query = @"
                UPDATE articles SET article_state ='Approved'
                WHERE article_id = @article_id
                ";

            using (var connection = GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                connection.Open();
                cmd.Parameters.AddWithValue("@article_id", article_id);
                cmd.ExecuteNonQuery();

            }
        }
        public void denyArticle(int article_id)
        {
            string query = @"
                UPDATE articles SET article_state ='Denied'
                WHERE article_id = @article_id
                ";

            using (var connection = GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                connection.Open();
                cmd.Parameters.AddWithValue("@article_id", article_id);
                cmd.ExecuteNonQuery();
            }
        }


        public byte[]? GetPdfBytesFromDatabase(int article_id)
        {
            try
            {
                using var connection = GetConnection();
                using var command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = "SELECT article_file FROM `articles` WHERE article_id = @article_id"
                };
                command.Parameters.AddWithValue("@article_id", article_id);

                connection.Open();
                var result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                    return (byte[])result;

                Console.WriteLine("No PDF found for the given article_id.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return null;
            }
        }

        public string GetTitleById(int article_id)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT title FROM `articles` WHERE article_id = @these_id";
                command.Parameters.AddWithValue("@these_id", article_id);

                var result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    return result.ToString();
                }
                else
                {
                    MessageBox.Show("No title found for this thesis.");
                    return null;
                }
            }
        }

        public void IncVisitCount(int theseId)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = @"
                                    UPDATE articles
                                    SET visit_number = visit_number + 1
                                    WHERE article_id = @articleId"
                ;

                command.Parameters.AddWithValue("@articleId", theseId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void IncSaveCount(int theseId)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = @"
                                    UPDATE articles
                                    SET saves_number = saves_number + 1
                                    WHERE article_id = @articleId"
                ;

                command.Parameters.AddWithValue("@articleId", theseId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public byte[] loadFileByte(int theseId)
        {
            byte[] fileData = null;
            using (var connection = GetConnection())
            using (var command = new MySqlCommand("SELECT article_file FROM articles WHERE article_id = @fileId", connection))
            {
                command.Parameters.AddWithValue("@fileId", theseId);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    fileData = (byte[])reader["article_file"];
                }
            }
            return fileData;
        }

        public List<string>? GetKeywords(int theseId)
        {
            List<string> keywords = new List<string>();

            string query = @"
                SELECT k.keyword
                FROM keywords k 
                JOIN used_keywords uk ON uk.article_id = k.keyword_id
                WHERE uk.article_id = @supervisorId ;
                ";

            using (var connection = GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@supervisorId", theseId);
                connection.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string keyword = reader.GetString("keyword");
                        keywords.Add(keyword);
                    }
                }
            }

            return keywords;
        }

        public List<string>? getSupervisors(int theseId)
        {
            List<string> names = new List<string>();

            string query = @"
                SELECT u.user_name
                FROM users u
                JOIN supervised_by sb ON sb.supervisor_id = u.user_id
                WHERE sb.article_id = @supervisorId ;
                ";

            using (var connection = GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@supervisorId", theseId);
                connection.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString("user_name");
                        names.Add(name);
                    }
                }
            }

            return names;
        }

        public List<string>? getAuthors(int theseId)
        {
            List<string> names = new List<string>();

            string query = @"
                SELECT CONCAT(wb.first_name, ' ', wb.last_name) AS name
                FROM written_by wb
                WHERE wb.article_id = @supervisorId ;
                ";

            using (var connection = GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@supervisorId", theseId);
                connection.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString("name");
                        names.Add(name);
                    }
                }
            }

            return names;
        }
    }
}
