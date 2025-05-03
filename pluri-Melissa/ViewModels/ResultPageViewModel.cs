using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gestion.Model;
using Project.Repos;

namespace Project.ViewModels
{
    public class ResultPageViewModel : ViewModelBase
    {

        //fields
        private ObservableCollection<theseResultat> _results;
        public ObservableCollection<theseResultat> Results
        {
            get { return _results; }
            set
            {
                _results = value;
                OnPropertyChanged(nameof(Results));
            }
        }

        private string _searchKey;
        public string SearchKey
        {
            get { return _searchKey; }
            set
            {
                _searchKey = value;
                OnPropertyChanged(nameof(SearchKey));
                LoadResults();
            }
        }



        //constructors
        public ResultPageViewModel()
        {
            Results = new ObservableCollection<theseResultat>();
        }


        public ResultPageViewModel(string searchKey)
        {
            Results = new ObservableCollection<theseResultat>();
            SearchKey = searchKey;
            LoadResults();
        }


        public ResultPageViewModel(List<theseResultat> resultats)
        {
            Results = new ObservableCollection<theseResultat>(resultats);
        }







        //methods
        private void LoadResults()
        {
            var repo = new theseResultatRepo();
            var results = repo.rechercheThese(SearchKey);
            Results.Clear();
            foreach (var result in results)
            {
                Results.Add(result);
            }
        }
    }
}
