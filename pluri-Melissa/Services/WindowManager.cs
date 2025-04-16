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
            }
        }


        public void CloseWindow()
        {
            Application.Current.Windows
         .OfType<Window>()
         .SingleOrDefault(w => w.IsActive)?.Close();
        }
    }
   

}
