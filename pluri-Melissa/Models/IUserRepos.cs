using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{

    //all methods related to user : 



    interface IUserRepos
    {
        //login
        bool AuthenticateUser(string user_email, string user_password);
        bool SignUp(UserModel usermodel);

        bool ChangeProfilePic(int user_id, byte[] profilepic);
        public byte[] LoadProfilePic(int user_id);
        public byte[] SetDefaultProfilePic(string Email);

        public string GetUsernameFromEmail(string email);
        public byte[] GetProfilepicFromEmail(string email);


        void Edit(UserModel usermodel);
        void Removes(int user_id);
        bool IsUsthbMember(String email);
        string AssignUserRole(string email);
        int GetUserId(string email);
        UserModel GetById(int user_id);
        UserModel GetByUsername(string user_name);
        IEnumerable<UserModel> GetByAll();
        //.......else

    }
}
