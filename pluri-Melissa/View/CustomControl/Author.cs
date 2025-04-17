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

namespace Project.View.CustomControl
{
    public class Author : Control
    {


        public ImageSource authorImage
        {
            get { return (ImageSource)GetValue(authorImageProperty); }
            set { SetValue(authorImageProperty, value); }
        }

        public static readonly DependencyProperty authorImageProperty =
            DependencyProperty.Register("authorImage", typeof(ImageSource), typeof(Author), new PropertyMetadata(null));



        public string authorName
        {
            get { return (string)GetValue(authorNameProperty); }
            set { SetValue(authorNameProperty, value); }
        }

        public static readonly DependencyProperty authorNameProperty =
            DependencyProperty.Register("authorName", typeof(string), typeof(Author), new PropertyMetadata(string.Empty));


        static Author()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Author), new FrameworkPropertyMetadata(typeof(Author)));
        }
    }
}
