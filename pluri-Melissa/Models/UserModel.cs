using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        public string user_faculty { get; set; } 


        public ImageSource pfp { get; set; }

        public UserModel(string user_id, string user_name)
        {
            this.user_id = user_id;
            this.user_name = user_name;
        }

        public BitmapImage ProfileImage
        {
            get
            {
                if (user_profilepic == null) return null;
                using (var stream = new MemoryStream(user_profilepic))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();
                    image.Freeze(); // For thread-safety
                    return image;
                }
            }
        }


        public bool isAdmin (string user_id)
        {
            return user_role.Equals("Admin");
        }

        public bool isStudent (string user_id)
        {
            return user_role.Equals("Student");

        }

        public bool isMember (string user_id)
        {
            return user_role.Equals("Member");
        }

        
    }

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
