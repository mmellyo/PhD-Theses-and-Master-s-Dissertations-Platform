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

namespace Project.Repos
{




    //CLASSS TO IMPLEMET USER INTERFACES METHODS (MODEL)






    class UserRepos : RepoBase, IUserRepos
    {



        ///implementing  IUserRepos interface methods : 

        //METHOD SIGNUP USER - id if added successfully
        public int SignUp(UserModel user)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.user_password);
            byte[] defaultProfilePic = File.ReadAllBytes("C:\\ProgramData\\MySQL\\MySQL Server 8.0\\Uploads\\Default.jpg");

            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                string query = "INSERT INTO `user` (user_email, user_password, user_role, user_profilepic, user_name";
                string values = "VALUES (@user_email, @user_password, @user_role, @user_profilepic, @user_name";

                command.Parameters.Add("@user_email", MySqlDbType.VarChar).Value = user.user_email;
                command.Parameters.Add("@user_password", MySqlDbType.VarChar).Value = hashedPassword;
                command.Parameters.Add("@user_role", MySqlDbType.VarChar).Value = user.user_role;
                command.Parameters.Add("@user_name", MySqlDbType.VarChar).Value = user.user_name;
                command.Parameters.Add("@user_profilepic", MySqlDbType.Blob).Value = defaultProfilePic;

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
                command.CommandText = "SELECT user_id, user_password FROM `user` WHERE user_email = @user_email";
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




        //not implemented yet
        public void Edit(UserModel usermodel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserModel> GetByAll()
        {
            throw new NotImplementedException();
        }

        public UserModel GetById(int user_id)
        {
            throw new NotImplementedException();
        }

        public UserModel GetByUsername(string user_name)
        {
            throw new NotImplementedException();
        }

        public void Removes(int user_id)
        {
            throw new NotImplementedException();
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
                return "Memeber";
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

                command.CommandText = "SELECT user_profilepic FROM `user` WHERE user_email=@user_email";
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

                command.CommandText = "SELECT user_profilepic FROM `user` WHERE user_id=@user_id";
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
                command.CommandText = "SELECT user_email FROM `user` WHERE user_id=@user_id";
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

                command.CommandText = "SELECT user_id FROM `user` WHERE user_email=@user_email";
                command.Parameters.Add("@user_email", MySqlDbType.VarChar).Value = email;
                var userId = command.ExecuteScalar();
                return Convert.ToInt32(userId);
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
                command.CommandText = "UPDATE `user` SET user_profilepic=@profilepic WHERE user_id=@user_id";
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
                command.CommandText = "SELECT user_profilepic FROM `user` WHERE user_id=@user_id";
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

                command.CommandText = "SELECT user_profilepic FROM `user` WHERE user_email=@user_email";
                command.Parameters.Add("@user_email", MySqlDbType.VarChar).Value = Email;
                var result = command.ExecuteScalar();
                //if true converts the result to a byte array
                return result != DBNull.Value ? (byte[])result : null;
            }
        }










    }

}




