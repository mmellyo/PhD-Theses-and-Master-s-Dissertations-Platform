using Project.Commands;
using Project.Repos;
using Project.Stores;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Project.ViewModels
{
    public class ForgotPasswordViewModel : ViewModelBase
    {
        private string _email;
        private string _newPassword;
       // private string _confirmPassword;
        private string _recoveryFilePath;
        private string _message;

        private readonly UserRepos _userRepos = new UserRepos();


       // getters setters
       public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        public string NewPassword
        {
            get => _newPassword;
            set { _newPassword = value; OnPropertyChanged(nameof(NewPassword)); }
        }

      /*  public string ConfirmPassword
        {
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(nameof(ConfirmPassword)); }
        }*/

        public string RecoveryFilePath
        {
            get => _recoveryFilePath;
            set { _recoveryFilePath = value; OnPropertyChanged(nameof(RecoveryFilePath)); }
        }

        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(nameof(Message)); }
        }




        public ICommand ResettingPasswordCommand { get; }
        public ICommand BrowseRecoveryFileCommand { get; }
        

        public ForgotPasswordViewModel(NavigationStore navigationStore)
        {
            ResettingPasswordCommand = new ResettingPasswordCommand(this, navigationStore);
            BrowseRecoveryFileCommand = new BrowseRecoveryFileCommand(this, navigationStore);
        }
    }
}
