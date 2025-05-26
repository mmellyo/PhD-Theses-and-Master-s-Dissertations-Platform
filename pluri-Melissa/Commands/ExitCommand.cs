using System;
using System.Windows;
using Project.Stores;
using Project.ViewModels;

namespace Project.Commands
{
    internal class ExitCommand : CommandBase
    {
        private readonly UserProfileViewModel _viewModel;
        private readonly NavigationStore _navigationStore;

        public ExitCommand(UserProfileViewModel viewModel, NavigationStore navigationStore)
        {
            _viewModel = viewModel;
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            var result = MessageBox.Show("Voulez-vous vraiment quitter l'application ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
