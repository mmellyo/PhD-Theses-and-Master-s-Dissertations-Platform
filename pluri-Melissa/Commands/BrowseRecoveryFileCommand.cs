using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Repos;
using Project.Stores;
using Project.ViewModels;
using System.Windows;
using Microsoft.Win32;

namespace Project.Commands
{
    internal class BrowseRecoveryFileCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly ForgotPasswordViewModel _viewModel;
        private readonly UserRepos _userRepos;

        public BrowseRecoveryFileCommand(ForgotPasswordViewModel viewModel, NavigationStore navigationStore)
        {
            _viewModel = viewModel;
            _navigationStore = navigationStore;
            _userRepos = new UserRepos();
        }







        public override void Execute(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Recovery File (*.reset)|*.reset",
                Title = "Select your recovery file"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _viewModel.RecoveryFilePath = openFileDialog.FileName;
            }else
            {
                MessageBox.Show("No file selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }
    }
}
