using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class UserModel
    {
        public UserModel()
        {
        }

        public string user_id { get; set; }
        public string user_name { get; set; }
        public string user_uni   { get; set; }
        public string user_email { get; set; }
        public string user_password { get; set; }
        public string user_role { get; set; }
        public byte[] user_profilepic { get; set; }


        /*  //EMAIL
          private static string _user_email;

          public string User_email
          {
              get => _user_email;
              private set => _user_email = value;
          }

          public void SetCurrentUserEmail(string email)
          {
              _user_email = email;
          }

          public string GetCurrentUserEmail()
          {
              return _user_email;
          }




          //ROLE
          private static string _user_role;

          public string User_role
          {
              get => _user_role;
              private set => _user_role = value;
          }

          public void SetCurrentUserRole(string role)
          {
              _user_role = role;
          }

          public string GetCurrentUserRole()
          {
              return _user_role;
          }



          //PASSWORD
          private static string _user_password;

          public string User_password
          {
              get => _user_password;
              private set => _user_password = value;
          }

          public void SetCurrentUserPassword(string email)
          {
              _user_password = email;
          }

          public string GetCurrentUserPassword()
          {
              return _user_password;
          }

          */
    }
}
