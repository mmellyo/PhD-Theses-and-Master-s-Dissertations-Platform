using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;

namespace Project.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>


    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }



        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();

        private void toggleTheme(Object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();

            if (IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                IsDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                IsDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
            }

            paletteHelper.SetTheme(theme);
        }





        private void exitApp(Object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void click_Minimize(Object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }






        private void Window_MouseDown(Object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

      
        
    }
}
