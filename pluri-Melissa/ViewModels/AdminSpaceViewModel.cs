using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Org.BouncyCastle.Security;
using Project.Commands;
using Project.Stores;
using System.Windows.Input;

namespace Project.ViewModels
{
    internal class AdminSpaceViewModel : ViewModelBase
    {
        private NavigationStore _navigationStore;
        private int user_id;


        public ICommand Logout { get; }
        public ICommand CommentaireSIGNALER { get; }
        public ICommand ThesesSIGNALER { get; }


        public AdminSpaceViewModel(NavigationStore navigationStore, int user_id)
        {
            this.user_id = user_id;
            _navigationStore = navigationStore;

            Logout = new NavigateCommand<WelcomeViewModel>(navigationStore, ()=> new WelcomeViewModel(navigationStore));

            CommentaireSIGNALER = new NavigateCommand<MODCommentViewModel>(navigationStore, ()=> new MODCommentViewModel(user_id, navigationStore));

            //ThesesSIGNALER = new NavigateCommand<MODarticles>

        }
    }
}