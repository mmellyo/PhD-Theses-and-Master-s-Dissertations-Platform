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
        List<UserModel> authors1 = new List<UserModel>();
        List<string> unregistered_authors = new List<string>();
        List<string> keywords = new List<string>();
        List<string> faculties = new List<string>();

        //ArticleModel article1 = new ArticleModel("these bla bla", "blabla bla bla bla", "these de master", authors, unregistered_authors, keywords, faculties,"september 2023");
       public ObservableCollection<ArticleModel> Articles { get; set; }

        public HomePageViewModel()
        {
            Articles = new ObservableCollection<ArticleModel>();
        }

        
    }
}
