using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
     interface IEmailVerificationRepo
    {
        public string GenerateVerificationCode();
        public void SendVerificationEmail(string recipientEmail, string verificationCode);

    }
}
