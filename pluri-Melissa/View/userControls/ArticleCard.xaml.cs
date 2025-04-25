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
using Project.Models;

namespace Project.view.userControls
{ 

    public partial class ArticleCard : UserControl
    {


        public ArticleCard()
        {
        }

        public ArticleModel article
        {
            get { return (ArticleModel)GetValue(ArticleProperty); }
            set { SetValue(ArticleProperty, value); }
        }

        public static readonly DependencyProperty ArticleProperty =
            DependencyProperty.Register("Article", typeof(ArticleModel), typeof(ArticleCard), new PropertyMetadata(null));

    }
}
