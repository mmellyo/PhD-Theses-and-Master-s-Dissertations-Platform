using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Project.Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;
using System.IO;
using System.Windows;
using MySqlX.XDevAPI.Common;

namespace Project.Repos
{




    //CLASSS TO IMPLEMET USER INTERFACES METHODS (MODEL)






    public class UserRepos : RepoBase, IUserRepos
    {



        ///implementing  IUserRepos interface methods : 

        //METHOD SIGNUP USER - id if added successfully
        public int SignUp(UserModel user)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.user_password);


            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                string query = "INSERT INTO `users` (user_email, user_password, user_role, user_name, user_profilePic";
                string values = "VALUES (@user_email, @user_password, @user_role,  @user_name, @profilePic";

                command.Parameters.Add("@user_email", MySqlDbType.VarChar).Value = user.user_email;
                command.Parameters.Add("@user_password", MySqlDbType.VarChar).Value = hashedPassword;
                command.Parameters.Add("@user_role", MySqlDbType.VarChar).Value = user.user_role;
                command.Parameters.Add("@user_name", MySqlDbType.VarChar).Value = user.user_name;
                command.Parameters.Add("@profilePic", MySqlDbType.Blob).Value = user.user_profilepic;
                if (!string.IsNullOrEmpty(user.user_uni))
                {
                    query += ", user_uni";
                    values += ", @user_uni";
                    command.Parameters.Add("@user_uni", MySqlDbType.VarChar).Value = user.user_uni;
                }

                query += ") ";
                values += ")";

                command.CommandText = query + values + "; SELECT LAST_INSERT_ID();";

                object result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }


        // Returns the user ID if credentials are valid, otherwise -1
        public int AuthenticateUser(string user_email, string user_password)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT user_id, user_password FROM `users` WHERE user_email = @user_email";
                command.Parameters.Add("@user_email", MySqlDbType.VarChar).Value = user_email;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var userId = reader.GetInt32("user_id");
                        var storedHash = reader.GetString("user_password");

                        if (BCrypt.Net.BCrypt.Verify(user_password, storedHash))
                        {
                            return userId; 
                        }
                    }
                }
            }

            return -1;
        }

        public bool IsUsthbMember(String email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            // name.lastname@etu/edu.usthb.dz
            string emailPattern = @"^[a-zA-Z]+\.[a-zA-Z]+@(etu|edu)\.usthb\.dz$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
        }

        public UserModel GetByUsername(string user_name)
        {
            var allUsers = new UserRepos().GetAllUsers(); // You need to implement this
            return allUsers.FirstOrDefault(u =>
                $"{u.user_name}".Equals(user_name, StringComparison.InvariantCultureIgnoreCase));
        }

        public List<UserModel> GetAllUsers()
        {
            var users = new List<UserModel>();

            using (var connection = GetConnection())
            {
                connection.Open();
                var query = "SELECT user_id, user_name, user_role FROM users";

                using (var cmd = new MySqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new UserModel
                        {
                            user_id = reader.GetInt32("user_id").ToString(),
                            user_name = reader.GetString("user_name"),
                            user_role = reader.GetString("user_role")
                        };

                        users.Add(user);
                    }
                }
            }

            return users;
        }

        public UserModel GetById(string user_id)
        {
            var allUsers = new UserRepos().GetAllUsers(); // You need to implement this
            return allUsers.FirstOrDefault(u =>
                $"{u.user_id}".Equals(user_id, StringComparison.InvariantCultureIgnoreCase));
        }


        //not implemented yet
        public void Edit(UserModel usermodel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserModel> GetByAll()
        {
            throw new NotImplementedException();
        }

        public void Removes(string user_id)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM `users` WHERE user_id = @user_id";
                command.Parameters.Add("@user_id", MySqlDbType.VarChar).Value = user_id;

            }
        }






        ///HELPER METHODS 


            //Method to Assign user's Role (still working on it)
        public string AssignUserRole(string email)
        {
            if (email.Equals("theses.usthb@gmail.com"))
                if (string.IsNullOrEmpty(email))
                {
                    return "Admin";
                }

            // Check if email contains @ symbol
            int atIndex = email.IndexOf('@');
            if (atIndex <= 0 || atIndex == email.Length - 1)
            {
                return "Unknown";
            }

            // Get the domain part after @
            string domain = email.Substring(atIndex + 1).ToLower();

            // Check if domain contains edu or etu
            if (domain.Contains("edu"))
            {
                return "Member";
            }
            else if (domain.Contains("etu"))
            {
                return "Student";
            }
            else
            {
                return "error";
            }
        }


        public string GetUsernameFromEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return string.Empty;

            // Split the email before the '@'
            var atSplit = email.Split('@');
            if (atSplit.Length < 2)
                return string.Empty;

            // Replace '.' with '_' in the username part
            string usernamePart = atSplit[0].Replace('.', '_');
            return usernamePart;
        }


        public byte[] GetProfilepicFromEmail(string email)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "SELECT user_profilePic FROM `users` WHERE user_email=@user_email";
                command.Parameters.Add("@user_email", MySqlDbType.VarChar).Value = email;
                var result = command.ExecuteScalar();

                return result != DBNull.Value ? (byte[])result : null;
            }
        }

        public byte[] GetProfilepicFromId(int user_id)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "SELECT user_profilePic FROM `users` WHERE user_id=@user_id";
                command.Parameters.Add("@user_id", MySqlDbType.VarChar).Value = user_id;
                var result = command.ExecuteScalar();

                return result != DBNull.Value ? (byte[])result : null;
            }
        }




        public string GetuserEmail(int user_id)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT user_email FROM `users` WHERE user_id=@user_id";
                command.Parameters.Add("@user_id", MySqlDbType.Int32).Value = user_id;
                var result = command.ExecuteScalar();
                return result != DBNull.Value ? (string)result : null;
            }
        }

        public string GetuserName(int user_id)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT user_name FROM `users` WHERE user_id=@user_id";
                command.Parameters.Add("@user_id", MySqlDbType.Int32).Value = user_id;
                var result = command.ExecuteScalar();
                return result != DBNull.Value ? (string)result : null;
            }
        }


        public int GetUserId(string email)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "SELECT user_id FROM `users` WHERE user_email=@user_email";
                command.Parameters.Add("@user_email", MySqlDbType.VarChar).Value = email;
                var userId = command.ExecuteScalar();
                return Convert.ToInt32(userId);
            }
        }

        public string GetUserRole(int user_id)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "SELECT user_role FROM `users` WHERE user_id=@user_id";
                command.Parameters.Add("@user_id", MySqlDbType.Int32).Value = user_id;
                var role = command.ExecuteScalar();
                return role != DBNull.Value ? (string)role : null;
            }
        }

        public bool ChangeProfilePic(int user_id, byte[] profilepic)
        {
            if (profilepic.Length > 5 * 1024 * 1024)
            {
                MessageBox.Show("Profile picture is too large (max 5 MB).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE `users` SET user_profilepic=@profilePic WHERE user_id=@user_id";
                command.Parameters.Add("@profilepic", MySqlDbType.Blob).Value = profilepic;
                command.Parameters.Add("@user_id", MySqlDbType.Int32).Value = user_id;

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public byte[] LoadProfilePic(int user_id)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT user_profilePic FROM `users` WHERE user_id=@user_id";
                command.Parameters.Add("@user_id", MySqlDbType.Int32).Value = user_id;

                var result = command.ExecuteScalar();
                //if true converts the result to a byte array
                return result != DBNull.Value ? (byte[])result : null;
            }
        }


        public byte[] SetDefaultProfilePic(string Email)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "SELECT user_profilePic FROM `users` WHERE user_email=@user_email";
                command.Parameters.Add("@user_email", MySqlDbType.VarChar).Value = Email;
                var result = command.ExecuteScalar();
                //if true converts the result to a byte array
                return result != DBNull.Value ? (byte[])result : null;
            }
        }




        public void SaveArticleForUser(int articleId, int userId)
        {
            string query = @"
                        INSERT INTO saved_articles (article_id, user_id)
                        VALUES (@articleId, @userId)
                        ON DUPLICATE KEY UPDATE article_id = article_id;
                        ";

            using (var connection = GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@articleId", articleId);
                cmd.Parameters.AddWithValue("@userId", userId);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public bool ResetPassword(string username, string newPassword)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "UPDATE `users` SET user_password = @password WHERE user_name = @user_name";
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = hashedPassword;
                command.Parameters.Add("@user_name", MySqlDbType.VarChar).Value = username;

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }



    }

}




