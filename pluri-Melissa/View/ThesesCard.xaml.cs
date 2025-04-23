using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using gestion.Model;

namespace gestion.Views
{
    /// <summary>
    /// Logique d'interaction pour ThesesCard.xaml
    /// </summary>
    public partial class ThesesCard : UserControl
    {
        public ThesesCard()
        {
            InitializeComponent();
        }
        private void resultatRecherche_Load(object sender, EventArgs e)
        {

        }
        public void details(theseResultat d)
        {
            tNom.Text = d.NomThese;
            aNom.Text = d.NomAuteur;
            mCle1.Text = d.MotCles;
            Diplome.Text = d.Diplome;
            nomEncadrant.Text = d.NomEncadrant;
            Langue.Text = d.Langue;
            anneeUniv.Text = d.AnneeUniversitaire;
            Filiere.Text = d.Filiere;
        }
        public void resultatRecherche(string key)
        {
            theseResultat get = new theseResultat();
            get.rechercheThese(key);
            tNom.Text = get.NomThese;
            aNom.Text = get.NomAuteur;
            mCle1.Text = get.MotCles;
            Diplome.Text = get.Diplome;
            nomEncadrant.Text = get.NomEncadrant;
            Langue.Text = get.Langue;
            anneeUniv.Text = get.AnneeUniversitaire;
            Filiere.Text = get.Filiere;
        }
    }
}

