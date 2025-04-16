
using System;

namespace Project.Models
{
    public class EmailVerificationModel
    {
        private static string _generatedVerificationCode;


        //Verification Code
        public string GeneratedVerificationCode
        {
            get => _generatedVerificationCode;
            private set => _generatedVerificationCode = value;
        }

        public void SetVerificationCode(string code)
        {
            _generatedVerificationCode = code;
        }

        public string GetVerificationCode()
        {
            return _generatedVerificationCode;
        }
    }
}