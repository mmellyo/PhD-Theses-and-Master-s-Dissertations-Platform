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
using gestion.Views;
using Mysqlx.Notice;
using MySqlX.XDevAPI.Common;

namespace gestion
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void loadDetails()
        {
            for (int i = 0; i < suggestionRecherche.list.Count; i++)
            {
                var data = suggestionRecherche.list[i];
                rechercheResultat res = new rechercheResultat();
                res.details(data);
                resultContainer.Children.Add(res);

                if (i < suggestionRecherche.list.Count - 1)
                {
                    Separator sep = new Separator();
                    sep.Margin = new Thickness(5, 10, 5, 10);
                    sep.Height = 1;
                    sep.Background = Brushes.LightGray;
                    resultContainer.Children.Add(sep);
                }
            }

        }
        private void TheseSearch_TextChanged(object sender, EventArgs e)
        {
            if (TheseSearch.Text.Length >= 1)
            {   
                resultContainer.Children.Clear();
                rechercheResultat res = new rechercheResultat();
                res.resultatRecherche(TheseSearch.Text);
                loadDetails();
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            string motcle = TheseSearch.Text;
            theseResultat model = new theseResultat();
            model.rechercheThese(motcle);
            resultaaat fenetre = new resultaaat(motcle);
            fenetre.Show();
            this.Close();
        }

        private void RechercheAvancee_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}