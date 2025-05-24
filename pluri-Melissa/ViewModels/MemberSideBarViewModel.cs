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
    public class MemberSideBarViewModel : ViewModelBase
    {
        private int user_id;
        private readonly NavigationStore navigationStore;


        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateNormalSearchCommand { get; }
        public ICommand NavigateAdvancedSearchCommand { get; }
        public ICommand NavigateProfileCommand { get; }
        public ICommand NavigateHelpCommand { get; }
        public ICommand NavigateManageArticlesCommand { get; }

        public ICommand ToggleSearchCommand { get; }

        private bool _isSearchExpanded;

        public bool IsSearchExpanded
        {
            get { return _isSearchExpanded; }
            set
            {
                if (_isSearchExpanded != value)
                {
                    _isSearchExpanded = value;
                    OnPropertyChanged(nameof(IsSearchExpanded));
                }

            }
        }

        private bool _isHomeExpanded;
        public bool IsHomeExpanded
        {
            get { return _isHomeExpanded; }
            set
            {
                if (_isHomeExpanded != value)
                {
                    _isHomeExpanded = value;
                    OnPropertyChanged(nameof(IsHomeExpanded));
                }

            }
        }

        public ICommand ToggleHomeCommand { get; }

        public MemberSideBarViewModel(int user_id, NavigationStore navigationStore)
        {
            this.user_id = user_id;
            this.navigationStore = navigationStore;
            ToggleHomeCommand = new ViewModelCommand(_ => IsHomeExpanded = !IsHomeExpanded);
            NavigateManageArticlesCommand = new NavigateCommand<MemberModArticlesViewModel>(navigationStore, () => new MemberModArticlesViewModel(user_id, navigationStore));
            ToggleSearchCommand = new ViewModelCommand(_ => IsSearchExpanded = !IsSearchExpanded);
            NavigateHomeCommand = new NavigateCommand<HomePageViewModel>(navigationStore, () => new HomePageViewModel(navigationStore, user_id));
            NavigateNormalSearchCommand = new NavigateCommand<rechercheWinViewModel>(navigationStore, () => new rechercheWinViewModel(navigationStore, user_id));
            NavigateAdvancedSearchCommand = new NavigateCommand<RechercheAvanceViewModel>(navigationStore, () => new RechercheAvanceViewModel(navigationStore, user_id));
            NavigateProfileCommand = new NavigateCommand<UserProfileViewModel>(navigationStore, () => new UserProfileViewModel(navigationStore, user_id));
        }

        
    }
}
