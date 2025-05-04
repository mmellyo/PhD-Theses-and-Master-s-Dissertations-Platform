using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gestion.Model;
using Project.Utils;
using System.Windows.Input;
using Project.Services;
using Project.Repos;
using Project.View;


//this will search - get results - send them to resultvm
// resultvm will display them


namespace Project.ViewModels
{
    public class rechercheWinViewModel : ViewModelBase
    {
        //fields
        private readonly IWindowManager _windowManager;
        private readonly ViewModelLocator _viewModelLocator;
        public ITheseService TheseService { get; }

        private ObservableCollection<suggestionRecherche> _suggestions;
        public ObservableCollection<suggestionRecherche> Suggestions
        {
            get => _suggestions;
            set
            {
                _suggestions = value;
                OnPropertyChanged(nameof(Suggestions));
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                Recherche(value);
            }
        }


        //commands
        public ICommand OpenResultPageCommand { get; }
        public ICommand OpenAdvancedSearchCommand { get; }





        //constructor
        public rechercheWinViewModel(ITheseService theseService, IWindowManager windowManager, ViewModelLocator viewModelLocator)
        {
            //fields
            Suggestions = new ObservableCollection<suggestionRecherche>();

            _windowManager = windowManager;
            _viewModelLocator = viewModelLocator;
            TheseService = theseService;


            //initialize the theses collection
            if (TheseService.Theses == null)
            {
                Console.WriteLine(" collection is null - Initializing  collection");
                TheseService.Theses = new ObservableCollection<theseResultat>();
            }


            //command 1
            OpenResultPageCommand = new ViewModelCommand(
                execute: obj =>
                {
                    _viewModelLocator.ResultPageViewModel.SearchKey = SearchText;
                    _windowManager.CloseWindow();
                    _windowManager.ShowWindow(_viewModelLocator.ResultPageViewModel);
                }
            );


            //command 2
            OpenAdvancedSearchCommand = new ViewModelCommand(
                execute: obj =>
                {
                    _windowManager.CloseWindow();
                    _windowManager.ShowWindow(_viewModelLocator.RechercheAvanceViewModel);
                }
            );
        }



        private void Recherche(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                Suggestions.Clear();
                return;
            }

            if (key != SearchText) return;

            var repo = new suggestionRechercheRepos();
            var gets = repo.recherche(key).Take(3);

            Suggestions.Clear();
            foreach (var get in gets)
            {
                Suggestions.Add(get);
            }
        }
    }
}
