using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gestion.Model;
using MaterialDesignThemes.Wpf;
using Project.Repos;

namespace Project.Services
{
    public interface ITheseService
    {


        public ObservableCollection<theseResultat> Theses { get; set; }
        void DisplayThese(int TheseId);
        void ReloadTheses(int theseId);
    }




    public class TheseService : ITheseService
    {
        private int theseId;
        private readonly theseResultatRepo _theseresultatRepo;

        public TheseService(theseResultatRepo theseresultatRepo = null)
        {
            _theseresultatRepo = theseresultatRepo ?? new theseResultatRepo();
            Theses = new ObservableCollection<theseResultat>();

        }
        

        public ObservableCollection<theseResultat> Theses { get; set; }

        public void DisplayThese(int TheseId)
        {
            if (Theses == null)
            {
                Theses = new ObservableCollection<theseResultat>();
            }
            Theses.Add(new theseResultat { TheseId = TheseId });
        }


        public void ReloadTheses(int theseId)
        {
          
            //later
        }
    }
}
