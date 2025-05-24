using Project.Stores;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Commands
{
    public class ReportCommand : CommandBase
    {
        private NavigationStore _navigationStore;
        private ThesePageViewModel _thesePageViewModel;

        public ReportCommand(NavigationStore navigationStore, ThesePageViewModel thesePageViewModel)
        {
            _navigationStore = navigationStore;
            _thesePageViewModel = thesePageViewModel;

        }

        public override void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
