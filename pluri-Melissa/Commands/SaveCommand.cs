using Project.Repos;
using Project.Stores;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Commands
{
    public class SaveCommand : CommandBase
    {
        private int theseId;
        private int userid;

        private NavigationStore NavigationStore;
        private UserRepos UserRepos;
        private TheseRepo TheseRepo;
        public SaveCommand (NavigationStore navigationStore, int theseId, int userid)
        {
            this.NavigationStore = navigationStore;
            this.theseId = theseId;
            this.userid = userid;
            UserRepos = new UserRepos();
            TheseRepo = new TheseRepo();
        }
        public override void Execute(object parameter)
        {
            UserRepos.SaveArticleForUser(theseId, userid);
            TheseRepo.IncSaveCount(theseId);
        }
    }
}
