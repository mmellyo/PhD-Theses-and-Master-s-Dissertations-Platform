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

namespace Project.Repos
{




    //CLASSS TO IMPLEMET USER INTERFACES METHODS (MODEL)






    class UserRepos : RepoBase, IUserRepos
    {



        ///implementing  IUserRepos interface methods : 

        //METHOD SIGNUP USER - true if added successfully
        public bool SignUp(UserModel user)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.user_password);

            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                // Base query and parameters
                string query = "INSERT INTO `user` (user_email, user_password, user_role";
                string values = "VALUES (@user_email, @user_password, @user_role";

                command.Parameters.Add("@user_email", MySqlDbType.VarChar).Value = user.user_email;
                command.Parameters.Add("@user_password", MySqlDbType.VarChar).Value = hashedPassword;
                command.Parameters.Add("@user_role", MySqlDbType.VarChar).Value = user.user_role;
                command.Parameters.Add("@user_name", MySqlDbType.VarChar).Value = GetUsernameFromEmail(user.user_email);


                ///not used : 
                // Conditionally add user_uni if not null or empty
                if (!string.IsNullOrEmpty(user.user_uni))
                {
                    query += ", user_uni";
                    values += ", @user_uni";
                    command.Parameters.Add("@user_uni", MySqlDbType.VarChar).Value = user.user_uni;
                }

                // Close the parentheses
                query += ") ";
                values += ")";

                // Combine query and values
                command.CommandText = query + values;

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected == 1;
            }
        }



        //returns true if the credentials are valid
        public bool AuthenticateUser(string user_email, string user_password)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT user_password FROM `user` WHERE user_email=@user_email";
                command.Parameters.Add("@user_email", MySqlDbType.VarChar).Value = user_email;

                var storedHash = command.ExecuteScalar()?.ToString();
                if (storedHash == null) return false;

                // Compare hashed passwords securely
                return BCrypt.Net.BCrypt.Verify(user_password, storedHash);
            }
        }


        //still working on it
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
            if(email.Equals("milissa.ameryahia@etu.usthb.dz"))
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
                return "Regular";
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

    }

}




