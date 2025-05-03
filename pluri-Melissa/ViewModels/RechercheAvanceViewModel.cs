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

namespace Project.ViewModels
{
    public class RechercheAvanceViewModel : ViewModelBase
    {
        // Champs de filtre
        public string NomAuteur { get; set; }
        public string NomEncadrant { get; set; }
        public string NomThese { get; set; }
        public string MotCle { get; set; }
        public string Langue { get; set; }
        public string Departement { get; set; }
        public string Annee { get; set; }
        public string Faculte { get; set; }

        // Commande liée au bouton Rechercher
        public ICommand RechercherCommand { get; }


        //constructor
        public RechercheAvanceViewModel()
        {
            //commands
            RechercherCommand = new ViewModelCommand(
                execute: obj =>
                {
                    Console.WriteLine("RechercherCommand ADVNCD  clicked");
                    RechercherAvecFiltres();
                }
            );
        }

        private void RechercherAvecFiltres()
        {
            try
            {
                Console.WriteLine("RechercherAvecFiltres functipon");
                var repo = new theseResultatRepo();
                var resultats = repo.rechercherAvecFiltres(NomAuteur, NomEncadrant, NomThese, MotCle, Langue, Departement, Annee, Faculte);

                Console.WriteLine("Resultats count: " + resultats.Count);

                var resultViewModel = new ResultPageViewModel();
                foreach (var res in resultats)
                    resultViewModel.Results.Add(res);
                Console.WriteLine("Resultats count displayed : " + resultViewModel.Results.Count);
                var resultWindow = new Resultaaat(resultViewModel);
                resultWindow.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading Resultats: {ex.Message}");
                MessageBox.Show($"Failed to load Resultats: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
