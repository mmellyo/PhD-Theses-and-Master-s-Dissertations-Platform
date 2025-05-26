using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Project.Repos;
using Project.Stores;
using Project.ViewModels;

namespace Project.Commands
{
    internal class DeleteAccountCommand : CommandBase
    {
        private readonly UserProfileViewModel _viewModel;
        private readonly NavigationStore _navigationStore;
        private readonly UserRepos _userRepos;

        public DeleteAccountCommand(UserProfileViewModel viewModel, NavigationStore navigationStore)
        {
            _viewModel = viewModel;
            _navigationStore = navigationStore;
            _userRepos = new UserRepos();
        }
        
        public override void Execute(object parameter)
        {
            bool success = _userRepos.DeleteUser(_viewModel.Email);
            if (success)
            {
                MessageBox.Show("Compte supprimé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                _navigationStore.CurrentViewModel = new WelcomeViewModel(_navigationStore);
            }
            else
            {
                MessageBox.Show("Échec de la suppression du compte. Veuillez réessayer plus tard.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }
    }
}
