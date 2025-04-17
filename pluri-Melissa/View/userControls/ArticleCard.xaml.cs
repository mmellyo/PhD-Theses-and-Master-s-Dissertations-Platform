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

namespace Project.View.userControl
{ 

    public partial class ArticleCard : UserControl
    {
        public ICommand LoadedCommand
        {
            get { return (ICommand)GetValue(LoadedCommandProperty); }
            set { SetValue(LoadedCommandProperty, value); }
        }
        public static readonly DependencyProperty LoadedCommandProperty =
            DependencyProperty.Register("LoadedCommand", typeof(ICommand), typeof(ArticleCard), new PropertyMetadata(null));

        

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ArticleCard), new PropertyMetadata(string.Empty));



        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(ArticleCard), new PropertyMetadata(string.Empty));



        public string ImagePath
        {
            get { return (string)GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }

        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.Register("ImagePath", typeof(string), typeof(ArticleCard), new PropertyMetadata("/img/dummy.jpg"));




        public string Type
        {
            get { return (string)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(string), typeof(ArticleCard), new PropertyMetadata("Article"));



        public string Date
        {
            get { return (string)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(string), typeof(ArticleCard), new PropertyMetadata(string.Empty));



        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(ArticleCard), new PropertyMetadata(Brushes.Aqua));


        /*public static readonly RoutedEvent DownloadClickEvent =
            EventManager.RegisterRoutedEvent(
                "DownloadClick", 
                RoutingStrategy.Bubble, 
                typeof(RoutedEventHandler), 
                typeof(ArticleCard)
                );


        public event RoutedEventHandler Downloaded
        {
            add 
            {
                AddHandler(DownloadClickEvent, value);
            }
            remove 
            {
                RemoveHandler(DownloadClickEvent, value);
            }

        }

        protected virtual void onDownload()
        {
            RaiseEvent (new RoutedEventArgs(DownloadClickEvent,this));  
        }*/
        
        public ArticleCard()
        {
            InitializeComponent();
            
        }

        private void articleCard_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoadedCommand != null) 
            {
                LoadedCommand.Execute(null);
            }
        }

        /* ArticleModel Article
       {
           get { return (ArticleModel)GetValue(ArticleProperty); }
           set { SetValue(ArticleProperty, value); }
       }

       public static readonly DependencyProperty ArticleProperty =
           DependencyProperty.Register("Article", typeof(ArticleModel), typeof(ArticleCard), new PropertyMetadata(null));
*/
    }
}
