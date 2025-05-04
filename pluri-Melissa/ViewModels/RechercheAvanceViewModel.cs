using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Repos;
using Project.Utils;
using Project.View;
using System.Windows.Input;
using System.Windows;
using Project.Services;
using Project.Models;
using gestion.Model;
using System.Collections.ObjectModel;
using MySqlX.XDevAPI.Common;
using MailKit.Search;

namespace Project.ViewModels
{
    public class RechercheAvanceViewModel : ViewModelBase
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

       //setter / getter
        public string NomAuteur { get; set; }
        public string NomEncadrant { get; set; }
        public string NomThese { get; set; }
        public string MotCle { get; set; }

        private ObservableCollection<string> _langues;
        public ObservableCollection<string> Langues
        {
            get => _langues;
            set { _langues = value; OnPropertyChanged(nameof(Langues)); }
        }

        private string _langue;
        public string Langue
        {
            get => _langue;
            set { _langue = value; OnPropertyChanged(nameof(Langue)); }
        }


        public string Departement { get; set; }
        public string Annee { get; set; }
        public string Faculte { get; set; }

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

        public ITheseService TheseService { get; }




        /// used for dropdowns
        /*private void InitializeFilters()
        {
            Langues = new ObservableCollection<string> { "Français", "Anglais" };
            //Departements = new ObservableCollection<string> { "Informatique", "Biologie" };
            //Annees = new ObservableCollection<string> { "2020", "2021", "2022", "2023", "2024" };
            //Facultes = new ObservableCollection<string> { "Intelligence artificielle", "cybersécurité" };
        }*/




        //commands
        public ICommand RechercherCommand { get; }


        //constructor
        public RechercheAvanceViewModel(ITheseService theseService, IWindowManager windowManager, ViewModelLocator viewModelLocator)
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

            /// used for dropdowns
            //InitializeFilters();

            // Ensure collection is initialized
            if (TheseService.Theses == null)
            {
                Console.WriteLine("comment collection is null - Initializing Comments collection");
                TheseService.Theses = new ObservableCollection<theseResultat>();
            }

            //commands
            RechercherCommand = new ViewModelCommand(
                execute: obj =>
                {
                    var filteredResults = RechercherAvecFiltres();
                    TheseService.Theses = new ObservableCollection<theseResultat>(filteredResults);

                    _viewModelLocator.ResultPageViewModel.TheseService.Theses = TheseService.Theses;
                    _windowManager.ShowWindow(_viewModelLocator.ResultPageViewModel);
                }
            );
        }

        

        private List<theseResultat> RechercherAvecFiltres()
        {
            var results = new List<theseResultat>();
            try
            {
                TheseService.Theses.Clear();

                // Load from DB
                var resultats = _theseResultatRepo.rechercherAvecFiltres(NomAuteur, NomEncadrant, NomThese, MotCle, Langue, Departement, Annee, Faculte);
                Console.WriteLine("Resultats count: " + resultats.Count);
                Console.WriteLine($"Langue used in filter: '{Langue}'");

                if (resultats != null)
                {
                    foreach (var r in resultats)
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

                        results.Add(result);
                    }

                    Console.WriteLine($"Loaded {results.Count} result successfully");
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
            return results;
        }


        }

    }

