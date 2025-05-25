using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Project.Models;
using MySql.Data.MySqlClient;
using System.Windows;
using System.IO;

namespace Project.Repos
{



    //CLASSS TO IMPLEMET EMAIL VERIFICATION INTERFACES METHODS (MODEL)





    class EmailVerificationRepo : RepoBase, IEmailVerificationRepo
    {

        public string GenerateVerificationCode()
        {
            Random random = new Random();
            string VerificationCode = random.Next(100000, 999999).ToString();
            return VerificationCode;
            //string VerificationCode = "123456";
            //SetVerificationCode(VerificationCode);

        }

        public void SendVerificationEmail(string recipientEmail, string verificationCode)
        {
            try
            {

                MailMessage mail = new MailMessage();



                mail.From = new MailAddress("theses.usthb@gmail.com");

                mail.To.Add(recipientEmail);
                mail.Subject = "Email Verification Code";

                // Create the email body with the verification code
                mail.Body = $"Hello! Your verification code is: {verificationCode}";


                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("theses.usthb@gmail.com", "kgbj jman hzvw ngtj"),
                    EnableSsl = true
                };



                // Send the email
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Failed to send email: " + ex.Message);
                throw;
            }
        }


        public bool IsEmailTaken(string email)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "SELECT COUNT(*) FROM `users` WHERE user_email = @user_email";

                //prevent SQL injection
                command.Parameters.Add("@user_email", MySqlDbType.VarChar).Value = email;

                int userCount = Convert.ToInt32(command.ExecuteScalar());

                return userCount > 0;
            }
        }


        public void SendCommentToAdmin(string UserName, string UserComment, int TheseId)
        {
            string Commenter = UserName;
        }

        public void CreateRecoveryFile(string email, string username)
        {
            try
            {
                var salt = CryptoHelper.GenerateSalt();
                var key = CryptoHelper.DeriveKey(email, salt);
                var encrypted = CryptoHelper.Encrypt(username, key);

                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "backup.reset");

                using (var fs = File.Create(filePath))
                using (var bw = new BinaryWriter(fs))
                {
                    bw.Write(salt.Length);
                    bw.Write(salt);
                    bw.Write(encrypted.Length);
                    bw.Write(encrypted);
                }

                MessageBox.Show("Recovery file created on your desktop.", "Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to create recovery file.\n" + ex.Message, "Error");
            }
        }


    }
}
