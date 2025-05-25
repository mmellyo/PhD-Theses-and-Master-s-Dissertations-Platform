using Project.Models;
using Project.Repos;
using Project.Services;
using Project.Stores;
using Project.ViewModels;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace Project.Commands
{
    internal class ResettingPasswordCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly ForgotPasswordViewModel _viewModel;
        private readonly UserRepos _userRepos;

        public ResettingPasswordCommand(ForgotPasswordViewModel viewModel, NavigationStore navigationStore)
        {
            _viewModel = viewModel;
            _navigationStore = navigationStore;
            _userRepos = new UserRepos();
        }







        public override void Execute(object parameter)
        {


            /*if (NewPassword != ConfirmPassword)
            {
                Message = "Passwords do not match.";
                return;
            }*/

            if (string.IsNullOrWhiteSpace(_viewModel.Email) ||
                string.IsNullOrWhiteSpace(_viewModel.RecoveryFilePath) ||
                string.IsNullOrWhiteSpace(_viewModel.NewPassword))
            {
                _viewModel.Message = "Please fill in all fields: Email, Recovery File, and New Password.";
                return;
            }

            try
            {
                byte[] fileBytes = File.ReadAllBytes(_viewModel.RecoveryFilePath);
                using var ms = new MemoryStream(fileBytes);
                using var br = new BinaryReader(ms);

                int saltLength = br.ReadInt32();
                byte[] salt = br.ReadBytes(saltLength);

                int encryptedLength = br.ReadInt32();
                byte[] encrypted = br.ReadBytes(encryptedLength);

                var key = CryptoHelper.DeriveKey(_viewModel.Email, salt);
                string username = CryptoHelper.Decrypt(encrypted, key);

                bool result = _userRepos.ResetPassword(username, _viewModel.NewPassword);

                if (result)
                {
                    MessageBox.Show("Password reset successfully. You may now log in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("User not found or reset failed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to reset password.\nReason: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }




        }
    }
}
