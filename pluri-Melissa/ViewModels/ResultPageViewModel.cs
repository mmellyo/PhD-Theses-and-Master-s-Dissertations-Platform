using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using gestion.Model;
using Project.Models;
using Project.Repos;
using Project.Services;

namespace Project.ViewModels
{
    public class ResultPageViewModel : ViewModelBase
    {

        //fields
        private readonly UserModel _userModel;
        private readonly IUserSessionService _userSession;
        private readonly UserRepos _userRepos;
        private readonly TheseRepo _theseRepo;
        private readonly theseResultatRepo _theseResultatRepo;
        private readonly ReportsRepo _reportRepo;
        private readonly EmailVerificationRepo _emailVerificationRepo;
        private readonly IWindowManager _windowManager;
        private readonly ViewModelLocator _viewModelLocator;


        //setters / getters
        public string NomThese { get; set; }
        public string NomAuteur { get; set; }
        public string MotCles { get; set; }
        public string Faculte { get; set; }
        public string NomEncadrant { get; set; }
        public string Langue { get; set; }
        public string Diplome { get; set; }
        public string AnneeUniversitaire { get; set; }
        public string Departement { get; set; }
        public string Resume { get; set; }

        public ITheseService TheseService { get; }



        private ObservableCollection<theseResultat> _results;
        public ObservableCollection<theseResultat> Results
        {
            get { return _results; }
            set
            {
                _results = value;
                OnPropertyChanged(nameof(Results));
                IsAdvancedSearch = true;
            }
        }

        private bool _isAdvancedSearch;
        public bool IsAdvancedSearch
        {
            get => _isAdvancedSearch;
            set
            {
                _isAdvancedSearch = value;
                OnPropertyChanged(nameof(IsAdvancedSearch));
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
                if (!IsAdvancedSearch)
                    LoadResults();
            }
        }


        //commands
        public ICommand consulterTheseCommand { get; }



        private readonly ITheseService _theseService;
        private theseResultat _selectedThese;


        //constructors
        public ResultPageViewModel(ITheseService theseService, IWindowManager windowManager, ViewModelLocator viewModelLocator)
        {
            //fields
            _userModel = new UserModel();
            _userRepos = new UserRepos();
            _theseRepo = new TheseRepo();
            _reportRepo = new ReportsRepo();
            _theseResultatRepo = new theseResultatRepo();
            _windowManager = windowManager;
            _viewModelLocator = viewModelLocator;
            TheseService = theseService;
            _theseService = theseService;

            // Ensure these collection is initialized
            if (TheseService.Theses == null)
            {
                Console.WriteLine("Theses collection is null - Initializing Theses collection");
                TheseService.Theses = new ObservableCollection<theseResultat>();
            }


            if (TheseService.Theses.Count == 0)
            {
                Console.WriteLine("Theses collection is empty - Loading ALL theses");
                TheseService.Theses = new ObservableCollection<theseResultat>();
                LoadResults();
            }
            else
            {
                Console.WriteLine("Theses collection is not empty - we came from ADVNCD SEARCH");
            }

           



            // commands
            consulterTheseCommand = new ViewModelCommand(
                execute: obj =>
                {
                    _windowManager.CloseWindow();
                    _windowManager.ShowWindow(_viewModelLocator.CommentViewModel);
                },
                canExecute: obj => true
            );
        }


      //  public ResultPageViewModel(string searchKey)
      //  {
      //      Results = new ObservableCollection<theseResultat>();
       //     SearchKey = searchKey;
        //    LoadResults();
       // }



    //    public ResultPageViewModel(List<theseResultat> resultats)
       // {
      //     Results = new ObservableCollection<theseResultat>(resultats);
      //  }







        //methods
        private void LoadResults()
        {
            try
            {
                TheseService.Theses.Clear();
                Console.WriteLine("Cleared  collections");

                // Load from DB
                var results = _theseResultatRepo.rechercheThese(SearchKey);
                Console.WriteLine("Loaded comments from repo");

                if (results != null)
                {
                    foreach (var r in results)
                    {
                        var result = new theseResultat
                        {
                            NomThese = r.NomThese,
                            NomAuteur = r.NomAuteur,
                            MotCles = r.MotCles,
                            Faculte = r.Faculte,
                            NomEncadrant = r.NomEncadrant,
                            Langue = r.Langue,
                            Diplome = r.Diplome,
                            AnneeUniversitaire = r.AnneeUniversitaire,
                            Departement = r.Departement,
                            Resume = r.Resume,
                            TheseId = r.TheseId
                        };

                        TheseService.Theses.Add(result);

                    }

                    Console.WriteLine($"Loaded {TheseService.Theses.Count} result successfully");
                }
                else
                {
                    Console.WriteLine("No comments found for this these");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading comments: {ex.Message}");
                MessageBox.Show($"Failed to load comments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    

        private void ConsulterThese(theseResultat these)
        {
            // Logique pour consulter la thèse
            // Par exemple, ouvrir une nouvelle fenêtre avec les détails de la thèse
            Console.WriteLine($"Consulter la thèse : {these.NomThese}");
        }
    }
}
