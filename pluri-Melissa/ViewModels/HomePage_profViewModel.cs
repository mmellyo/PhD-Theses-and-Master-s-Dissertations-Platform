using Project.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class HomePage_profViewModel : ViewModelBase
    {
        private readonly int userId;
        private readonly NavigationStore _navigationStore;
        public HomePage_profViewModel(int userId, NavigationStore navigationStore)
        {
            this.userId = userId;
            this._navigationStore = navigationStore;
        }
    }
}
