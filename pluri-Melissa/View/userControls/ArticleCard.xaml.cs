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
using Project.ViewModels;

namespace Project.View.userControls
{ 

    public partial class ArticleCard : UserControl
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ArticleCard), new PropertyMetadata(string.Empty));



        public string Type
        {
            get { return (string)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(string), typeof(ArticleCard), new PropertyMetadata(string.Empty));



        public string Date
        {
            get { return (string)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(string), typeof(ArticleCard), new PropertyMetadata(string.Empty));



        public string Author1_name
        {
            get { return (string)GetValue(Author1_nameProperty); }
            set { SetValue(Author1_nameProperty, value); }
        }

        public static readonly DependencyProperty Author1_nameProperty =
            DependencyProperty.Register("Author1_name", typeof(string), typeof(ArticleCard), new PropertyMetadata(string.Empty));
        public ImageSource Author1_pfp
        {
            get { return (ImageSource)GetValue(Author1_pfpProperty); }
            set { SetValue(Author1_pfpProperty, value); }
        }

        public static readonly DependencyProperty Author1_pfpProperty =
            DependencyProperty.Register("Author1_pfp", typeof(ImageSource), typeof(ArticleCard), new PropertyMetadata(null));

        public string Author2_name
        {
            get { return (string)GetValue(Author2_nameProperty); }
            set { SetValue(Author2_nameProperty, value); }
        }

        public static readonly DependencyProperty Author2_nameProperty =
            DependencyProperty.Register("Author2_name", typeof(string), typeof(ArticleCard), new PropertyMetadata(string.Empty));
        public ImageSource Author2_pfp
        {
            get { return (ImageSource)GetValue(Author2_pfpProperty); }
            set { SetValue(Author2_pfpProperty, value); }
        }

        public static readonly DependencyProperty Author2_pfpProperty =
            DependencyProperty.Register("Author2_pfp", typeof(ImageSource), typeof(ArticleCard), new PropertyMetadata(null));

        public string Author3_name
        {
            get { return (string)GetValue(Author3_nameProperty); }
            set { SetValue(Author3_nameProperty, value); }
        }

        public static readonly DependencyProperty Author3_nameProperty =
            DependencyProperty.Register("Author3_name", typeof(string), typeof(ArticleCard), new PropertyMetadata(string.Empty));
        public ImageSource Author3_pfp
        {
            get { return (ImageSource)GetValue(Author3_pfpProperty); }
            set { SetValue(Author3_pfpProperty, value); }
        }

        public static readonly DependencyProperty Author3_pfpProperty =
            DependencyProperty.Register("Author3_pfp", typeof(ImageSource), typeof(ArticleCard), new PropertyMetadata(null));

        public string Author4_name
        {
            get { return (string)GetValue(Author4_nameProperty); }
            set { SetValue(Author4_nameProperty, value); }
        }

        public static readonly DependencyProperty Author4_nameProperty =
            DependencyProperty.Register("Author4_name", typeof(string), typeof(ArticleCard), new PropertyMetadata(string.Empty));
        public ImageSource Author4_pfp
        {
            get { return (ImageSource)GetValue(Author4_pfpProperty); }
            set { SetValue(Author4_pfpProperty, value); }
        }

        public static readonly DependencyProperty Author4_pfpProperty =
            DependencyProperty.Register("Author4_pfp", typeof(ImageSource), typeof(ArticleCard), new PropertyMetadata(null));
        public string Summary
        {
            get { return (string)GetValue(SummaryProperty); }
            set { SetValue(SummaryProperty, value); }
        }

        public static readonly DependencyProperty SummaryProperty =
            DependencyProperty.Register("Summary", typeof(string), typeof(ArticleCard), new PropertyMetadata(string.Empty));


        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(ArticleCard), new PropertyMetadata(Brushes.Transparent));


        public static RoutedEvent DownloadClickEvent = 
            EventManager.RegisterRoutedEvent(nameof(DownloadClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ArticleCard));

        public event RoutedEventHandler DownloadClick
        {
            add { AddHandler(DownloadClickEvent, value); }
            remove { RemoveHandler(DownloadClickEvent, value); }
        }

        public static RoutedEvent SaveClickEvent =
            EventManager.RegisterRoutedEvent(nameof(SaveClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ArticleCard));

        public event RoutedEventHandler SaveClick
        {
            add { AddHandler(SaveClickEvent, value); }
            remove { RemoveHandler(SaveClickEvent, value); }
        }

        public static RoutedEvent FollowClickEvent =
            EventManager.RegisterRoutedEvent(nameof(FollowClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ArticleCard));

        public event RoutedEventHandler FollowClick
        {
            add { AddHandler(FollowClickEvent, value); }
            remove { RemoveHandler(FollowClickEvent, value); }
        }

        public static RoutedEvent ShareClickEvent =
            EventManager.RegisterRoutedEvent(nameof(ShareClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ArticleCard));

        public event RoutedEventHandler ShareClick
        {
            add { AddHandler(ShareClickEvent, value); }
            remove { RemoveHandler(ShareClickEvent, value); }
        }
        public ArticleCard()
        {
            InitializeComponent();
            PreviewMouseLeftButtonUp += (sender, args) => OnClick();
        }

        private void OnDownloadClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DownloadClickEvent));
        }
        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(SaveClickEvent));
        }

        private void OnFollowClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(FollowClickEvent));
        }

        private void OnShareClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ShareClickEvent));
        }


        public static readonly RoutedEvent ClickEvent =
            EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ArticleCard));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        void RaiseClickEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(ArticleCard.ClickEvent); 
            RaiseEvent(newEventArgs);
        }

        void OnClick()
        {
            RaiseClickEvent();
        }
    }
}
