using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Project.ViewModels;
using Project.Services;


namespace Project.Services
{
    public interface IWindowManager
    {
        void ShowWindow(ViewModelBase viewModel);
        void CloseWindow();
    }

    public class WindowManager : IWindowManager
    {
        private readonly WindowMapper _windodwMapper;
        private object args;
        private Window _currentWindow;

        //constructor
        public WindowManager(WindowMapper windowMapper)
        {
            _windodwMapper = windowMapper;
        }




        //implementation 
        public void ShowWindow(ViewModelBase viewModel)
        {


            var windowType = _windodwMapper.GetWindowTypeForViewModel(viewModel.GetType());

            if (windowType != null)
            {
                var window = Activator.CreateInstance(windowType) as Window;
                window.DataContext = viewModel;
                window.Show();
                window.Closed += (object? sender, EventArgs args) => CloseWindow();

                // Track the current window
                _currentWindow = window;


            }
        }


        public void CloseWindow()
        {
            if (_currentWindow != null)
            {
                var windowToClose = _currentWindow;
                _currentWindow = null;


                // this exits the whole app...
                //  windowToClose.Close();


                // to improve : 
                 windowToClose.Hide();   //This keeps the window instance alive but invisible

               
                 
                
            }
        }

        
    }
}
   

