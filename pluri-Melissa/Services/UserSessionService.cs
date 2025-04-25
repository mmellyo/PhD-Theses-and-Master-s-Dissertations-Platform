using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services
{
    public interface IUserSessionService
    {
        string Email { get; set; }
        string Username { get; set; }
        int UserId { get; set; }

    }

    public class UserSessionService : IUserSessionService
    {
        public string Email { get; set; }
        public string Username { get ; set; }
        public int UserId { get ; set; }
    }
}
