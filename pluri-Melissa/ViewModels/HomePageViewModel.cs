using Project.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
       public ObservableCollection<ArticleModel> Articles { get; set; }

        public HomePageViewModel()
        {
            Articles = new ObservableCollection<ArticleModel>();
        }

        
    }
}
