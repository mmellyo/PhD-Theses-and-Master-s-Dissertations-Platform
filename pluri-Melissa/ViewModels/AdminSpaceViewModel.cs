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


        public ICommand ToggleGestion1Command { get; }
        private bool _isGestion1Expanded;
        public bool IsGestion1Expanded
        {
            get { return _isGestion1Expanded; }
            set
            {
                if (_isGestion1Expanded != value)
                {
                    _isGestion1Expanded = value;
                    OnPropertyChanged(nameof(IsGestion1Expanded));
                }

            }
        }
        public ICommand ToggleGestion2Command { get; }
        private bool _isGestion2Expanded;
        public bool IsGestion2Expanded
        {
            get { return _isGestion2Expanded; }
            set
            {
                if (_isGestion2Expanded != value)
                {
                    _isGestion2Expanded = value;
                    OnPropertyChanged(nameof(IsGestion2Expanded));
                }

            }
        }
        public ICommand NavigateReportedCommentsCommand { get; }
        public AdminSpaceViewModel(NavigationStore navigationStore, int user_id)
        {
            this.user_id = user_id;
            _navigationStore = navigationStore;


            ToggleGestion1Command = new ViewModelCommand(_ => IsGestion1Expanded = !IsGestion1Expanded);
            ToggleGestion2Command = new ViewModelCommand(_ => IsGestion2Expanded = !IsGestion2Expanded);

            NavigateReportedCommentsCommand = new NavigateCommand<MODCommentViewModel>(navigationStore, () => new MODCommentViewModel(user_id ,navigationStore));
            this.user_id = user_id;
            //NavigateAutoFlaggedCommentsCommand =
            //NavigateReportedThesesCommand =
            //NavigateReportedAccountsCommand =

        }
    }
}