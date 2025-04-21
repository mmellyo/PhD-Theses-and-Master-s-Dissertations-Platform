using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using gestion.Model;
using MySql.Data.MySqlClient;

namespace gestion.Views
{
    /// <summary>
    /// Logique d'interaction pour resultaaat.xaml
    /// </summary>
    public partial class resultaaat : Window
    {
        private string _motcle;

        public resultaaat(string motcle)
        {
            InitializeComponent();
            _motcle = motcle;
            loadDetails();
        }

        private void loadDetails()
        {
            for (int i = 0; i < theseResultat.list.Count; i++)
            {
                var data = theseResultat.list[i];
                ThesesCard res = new ThesesCard();
                res.details(data);
                ResultatsWrapPanel.Children.Add(res);

                if (i < theseResultat.list.Count - 1)
                {
                    Separator sep = new Separator();
                    sep.Margin = new Thickness(5, 10, 5, 10);
                    sep.Height = 1;
                    sep.Background = Brushes.LightGray;
                    ResultatsWrapPanel.Children.Add(sep);
                }
            }

        }


    }
}
