using gestion.Model;
using Project.Services;
using Project.Stores;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Commands
{
    public class SearchCommand : CommandBase
    {
        private RechercheAvanceViewModel _viewModel;
        private NavigationStore _navigationStore;
        private int user_id;
        public SearchCommand(RechercheAvanceViewModel rechercheAvanceViewModel, NavigationStore navigationStore, int userid) 
        {
            _viewModel = rechercheAvanceViewModel;
            _navigationStore = navigationStore;
            user_id = userid;
        }
        public override void Execute(object parameter)
        {
            var filteredResults = _viewModel.RechercherAvecFiltres();
            _viewModel.TheseService.Theses = new ObservableCollection<theseResultat>(filteredResults);
            var navCommand = new NavigateCommand<ResultPageViewModel>(
                _navigationStore,
                () => new ResultPageViewModel(_navigationStore, user_id, _viewModel.TheseService)
            );
            navCommand.Execute(null);
        }
    }
}
