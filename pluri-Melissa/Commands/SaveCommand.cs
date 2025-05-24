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
        private ThesePageViewModel viewModel;
        private NavigationStore NavigationStore;
        private UserRepos UserRepos;
        public SaveCommand (NavigationStore navigationStore, ThesePageViewModel viewModel)
        {
            this.NavigationStore = navigationStore;
            this.viewModel= viewModel;
            UserRepos = new UserRepos();
        }
        public override void Execute(object parameter)
        {
            UserRepos.SaveArticleForUser(viewModel.theseId, viewModel.userid);
        }
    }
}
