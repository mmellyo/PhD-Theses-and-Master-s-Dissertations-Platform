/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Repos;
using Project.Stores;
using Project.ViewModels;

namespace Project.Commands
{
    internal class LogoutCommand : CommandBase
    {
        private readonly UserProfileViewModel _viewModel;
        private readonly NavigationStore _navigationStore;
        private readonly UserRepos _userRepos;

        public LogoutCommand(UserProfileViewModel viewModel, NavigationStore navigationStore)
        {
            _viewModel = viewModel;
            _navigationStore = navigationStore;
            _userRepos = new UserRepos();
        }

        public override void Execute(object parameter)
        {
            var navigateHome = new NavigateCommand<WelcomeViewModel>(_navigationStore, () => new WelcomeViewModel(_navigationStore));


        }
    }
}*/
