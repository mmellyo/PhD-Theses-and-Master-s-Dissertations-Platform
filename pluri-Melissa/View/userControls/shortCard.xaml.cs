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

namespace Project.View.userControls
{
    /// <summary>
    /// Interaction logic for shortCard.xaml
    /// </summary>
    public partial class shortCard : UserControl
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(shortCard), new PropertyMetadata(string.Empty));



        public string Type
        {
            get { return (string)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(string), typeof(shortCard), new PropertyMetadata(string.Empty));



        public string Date
        {
            get { return (string)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(string), typeof(shortCard), new PropertyMetadata(string.Empty));



        public string Author1_name
        {
            get { return (string)GetValue(Author1_nameProperty); }
            set { SetValue(Author1_nameProperty, value); }
        }

        public static readonly DependencyProperty Author1_nameProperty =
            DependencyProperty.Register("Author1_name", typeof(string), typeof(shortCard), new PropertyMetadata(string.Empty));
        public ImageSource Author1_pfp
        {
            get { return (ImageSource)GetValue(Author1_pfpProperty); }
            set { SetValue(Author1_pfpProperty, value); }
        }

        public static readonly DependencyProperty Author1_pfpProperty =
            DependencyProperty.Register("Author1_pfp", typeof(ImageSource), typeof(shortCard), new PropertyMetadata(null));

        public string Author2_name
        {
            get { return (string)GetValue(Author2_nameProperty); }
            set { SetValue(Author2_nameProperty, value); }
        }

        public static readonly DependencyProperty Author2_nameProperty =
            DependencyProperty.Register("Author2_name", typeof(string), typeof(shortCard), new PropertyMetadata(string.Empty));
        public ImageSource Author2_pfp
        {
            get { return (ImageSource)GetValue(Author2_pfpProperty); }
            set { SetValue(Author2_pfpProperty, value); }
        }

        public static readonly DependencyProperty Author2_pfpProperty =
            DependencyProperty.Register("Author2_pfp", typeof(ImageSource), typeof(shortCard), new PropertyMetadata(null));

        public string Author3_name
        {
            get { return (string)GetValue(Author3_nameProperty); }
            set { SetValue(Author3_nameProperty, value); }
        }

        public static readonly DependencyProperty Author3_nameProperty =
            DependencyProperty.Register("Author3_name", typeof(string), typeof(shortCard), new PropertyMetadata(string.Empty));
        public ImageSource Author3_pfp
        {
            get { return (ImageSource)GetValue(Author3_pfpProperty); }
            set { SetValue(Author3_pfpProperty, value); }
        }

        public static readonly DependencyProperty Author3_pfpProperty =
            DependencyProperty.Register("Author3_pfp", typeof(ImageSource), typeof(shortCard), new PropertyMetadata(null));

        public string Author4_name
        {
            get { return (string)GetValue(Author4_nameProperty); }
            set { SetValue(Author4_nameProperty, value); }
        }

        public static readonly DependencyProperty Author4_nameProperty =
            DependencyProperty.Register("Author4_name", typeof(string), typeof(shortCard), new PropertyMetadata(string.Empty));
        public ImageSource Author4_pfp
        {
            get { return (ImageSource)GetValue(Author4_pfpProperty); }
            set { SetValue(Author4_pfpProperty, value); }
        }

        public static readonly DependencyProperty Author4_pfpProperty =
            DependencyProperty.Register("Author4_pfp", typeof(ImageSource), typeof(shortCard), new PropertyMetadata(null));
        public string Summary
        {
            get { return (string)GetValue(SummaryProperty); }
            set { SetValue(SummaryProperty, value); }
        }

        public static readonly DependencyProperty SummaryProperty =
            DependencyProperty.Register("Summary", typeof(string), typeof(shortCard), new PropertyMetadata(string.Empty));


        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(shortCard), new PropertyMetadata(Brushes.Transparent));
        public shortCard()
        {
            InitializeComponent();
        }
    }
}
