using Project.Repos;
using Project.Stores;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project.Commands
{
    public class LoginCommand : CommandBase
    {
        private readonly LoginViewModel _viewModel;
        private readonly NavigationStore _navigationStore;
        private readonly UserRepos _userRepos;

        public LoginCommand(LoginViewModel viewModel, NavigationStore navigationStore)
        {
            _viewModel = viewModel;
            _navigationStore = navigationStore;
            _userRepos = new UserRepos();
        }

        public override void Execute(object parameter)
        {
     

            _viewModel.UserId = _userRepos.AuthenticateUser(_viewModel.LoginEmail, _viewModel.LoginPassword);    


            if((_viewModel.UserId) != -1)
            {
                //navigate to the adminspace if the user is an admin 
                if (_viewModel.LoginEmail.Equals("theses.usthb@gmail.com"))
                {
                    _navigationStore.CurrentViewModel = new AdminSpaceViewModel(_navigationStore, _viewModel.UserId);
                }

                    

                    //if its a regular user
                    else
                    {
                        _navigationStore.CurrentViewModel = new HomePageViewModel(_navigationStore, _viewModel.UserId);
                    }
                
            }
            else
            {
                MessageBox.Show("Invalid email or password");

            }
        }
    }
}
