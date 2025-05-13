using Project.Models;
using Project.Repos;
using Project.Services;
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
    internal class SignUpCommand : CommandBase
    {
        private readonly SignUpViewModel _viewModel;
        private readonly NavigationStore _navigationStore;
        private readonly UserRepos _userRepos;

        public SignUpCommand(SignUpViewModel viewModel, NavigationStore navigationStore)
        {
            _viewModel = viewModel;
            _navigationStore = navigationStore;
            _userRepos = new UserRepos();
        }

        public override void Execute(object parameter)
        {
            if (_viewModel.Password  != _viewModel.ConfirmPassword)
            {
                MessageBox.Show("Passwords do not match! Please re-enter to confirm.", "Passwords do not match", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var user = new UserModel
            {
                user_email = _viewModel.Email,
                user_password = _viewModel.Password,
                user_role = "user" //later :P
            };

            //_userSession.Email = this.Email;
            //ErrorMessage = $"saved: mail='{user.user_email}', pw='{user.user_password}', role='{repo.AssignUserRole(user.user_email)}'";

            int userId = _userRepos.SignUp(user);
            if(userId > 0)
            {
                new NavigateCommand<EmailVerificationViewModel>(_navigationStore, () => new EmailVerificationViewModel(_navigationStore));
            }

            //if (success) { _userSession.Email = this.Email; }



        }
    }
}
