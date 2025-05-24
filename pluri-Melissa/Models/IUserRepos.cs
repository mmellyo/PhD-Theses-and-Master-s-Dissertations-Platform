using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{

    //all methods related to user : 



    public interface IUserRepos
    {
        //login
        public int AuthenticateUser(string user_email, string user_password);
        public int SignUp(UserModel user);

        bool ChangeProfilePic(int user_id, byte[] profilepic);
        public byte[] LoadProfilePic(int user_id);
        public byte[] SetDefaultProfilePic(string Email);

        public string GetUsernameFromEmail(string email);
        public byte[] GetProfilepicFromEmail(string email);
        public byte[] GetProfilepicFromId(int user_id);
        public string GetuserEmail(int user_id);


        void Edit(UserModel usermodel);
        void Removes(string user_id);
        bool IsUsthbMember(String email);
        string AssignUserRole(string email);
        int GetUserId(string email);
        UserModel GetById(string user_id);
        UserModel GetByUsername(string user_name);
        IEnumerable<UserModel> GetByAll();
        //.......else

    }
}
