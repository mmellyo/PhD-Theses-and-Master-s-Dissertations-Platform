using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Project.Repos;
using Project.Services;
using Project.Stores;
using Project.ViewModels;

namespace Project.Commands
{
    internal class ChangeProfilePictureCommand : CommandBase
    {
        private readonly UserProfileViewModel _viewModel;
        private readonly NavigationStore _navigationStore;
        private readonly UserRepos _userRepos;

        public ChangeProfilePictureCommand(UserProfileViewModel viewModel, NavigationStore navigationStore)
        {
            _viewModel = viewModel;
            _navigationStore = navigationStore;
            _userRepos = new UserRepos();
        }

        public override void Execute(object parameter)
        {
            int UserId = _userRepos.GetUserId(_viewModel.Email);//not null


            //ask to upload a pic
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg",
                Title = "Select Profile Picture"
            };

            if (dialog.ShowDialog() == true)
            {
                // convert into binary array
                byte[] imageBytes = File.ReadAllBytes(dialog.FileName);

                bool success = _userRepos.ChangeProfilePic(UserId, profilepic: imageBytes);

                if (success)
                {
                    MessageBox.Show("Profile picture updated!");

                    //update ui (convert imageBytes to imagesource)
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = ms;
                        image.EndInit();
                        image.Freeze(); 

                        _viewModel.user_profilepic = image;
                    }
                }

                else
                { MessageBox.Show("Failed to update profile picture."); }


            }
        }
    }
}
