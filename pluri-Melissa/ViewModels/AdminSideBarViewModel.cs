using Project.Commands;
using Project.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project.ViewModels
{
    public class AdminSideBarViewModel : ViewModelBase
    {
        private readonly NavigationStore navigationStore;
        private int user_id;

        public ICommand NavigateAutoFlaggedCommentsCommand { get; }
        public ICommand NavigateReportedThesesCommand { get; }
        public ICommand NavigateNotApprovedTheseCommand { get; }



        public ICommand ToggleGestion1Command { get; }

        private bool _IsGestion1Expanded;
        public bool IsGestion1Expanded
        {
            get { return _IsGestion1Expanded; }
            set
            {
                if (_IsGestion1Expanded != value)
                {
                    _IsGestion1Expanded = value;
                    OnPropertyChanged(nameof(IsGestion1Expanded));
                }

            }
        }
        public ICommand ToggleGestion2Command { get; }

        private bool _IsGestion2Expanded;
        public bool IsGestion2Expanded
        {
            get { return _IsGestion2Expanded; }
            set
            {
                if (_IsGestion2Expanded != value)
                {
                    _IsGestion2Expanded = value;
                    OnPropertyChanged(nameof(IsGestion2Expanded));
                }

            }
        }

        public AdminSideBarViewModel(int user_id, NavigationStore navigationStore)
        {
          //  this.user_id = user_id;
            this.navigationStore = navigationStore;

            ToggleGestion1Command = new ViewModelCommand(_ => IsGestion1Expanded = !IsGestion1Expanded);
            ToggleGestion2Command = new ViewModelCommand(_ => IsGestion2Expanded = !IsGestion2Expanded);

            NavigateAutoFlaggedCommentsCommand = new NavigateCommand<MODCommentViewModel>(navigationStore, () => new MODCommentViewModel(user_id, navigationStore));
            //to do
            NavigateNotApprovedTheseCommand = new NavigateCommand<MODFlaggedArticlesViewModel>(navigationStore, () => new MODFlaggedArticlesViewModel(user_id, navigationStore));
            //NavigateNotApprovedTheseCommand
            
        }
    }
}
