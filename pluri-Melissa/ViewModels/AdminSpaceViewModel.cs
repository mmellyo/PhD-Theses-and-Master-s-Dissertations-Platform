using Project.Stores;

namespace Project.ViewModels
{
    internal class AdminSpaceViewModel : ViewModelBase
    {
        private NavigationStore _navigationStore;

        public AdminSpaceViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
    }
}