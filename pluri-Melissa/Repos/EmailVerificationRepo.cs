using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Project.Models;

namespace Project.Repos
{



    //CLASSS TO IMPLEMET EMAIL VERIFICATION INTERFACES METHODS (MODEL)








    class EmailVerificationRepo : IEmailVerificationRepo
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
                // Ensure the verification code is generated before sending the email
                // Generate the verification code before sending the email

                MailMessage mail = new MailMessage();


                // Set sender and recipient
                mail.From = new MailAddress("theses.usthb@gmail.com");

                mail.To.Add(recipientEmail); // Correctly use recipientEmail variable
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
    }
}
