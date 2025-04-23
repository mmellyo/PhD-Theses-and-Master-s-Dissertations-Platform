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
    public partial class rechercheResultat : UserControl
    {
        public rechercheResultat()
        {
            InitializeComponent();
        }
        private void resultatRecherche_Load(object sender, EventArgs e)
        {

        }
        public void details(suggestionRecherche d)
        {
            tNom.Text = d.NomThese;
            aNom.Text = d.NomAuteur;
            mCle.Text = d.MotCles;
        }
        public void resultatRecherche(string key)
        {
            suggestionRecherche get = new suggestionRecherche();
            get.recherche(key);
            tNom.Text = get.NomThese;
            aNom.Text = get.NomAuteur;
            mCle.Text = get.MotCles;
        }
    }
}
