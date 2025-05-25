using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{

    //all methods needed for authontucation



     interface IEmailVerificationRepo
    {
        public string GenerateVerificationCode();
        public void SendVerificationEmail(string recipientEmail, string verificationCode);
        public bool IsEmailTaken(string email);

        public void SendCommentToAdmin(string recipientEmail, string verificationCode,  int TheseId);

        public void CreateRecoveryFile(string email, string username);

    }
}
