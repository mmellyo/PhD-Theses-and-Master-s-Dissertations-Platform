using Project.Models;
using Project.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        string userid;
        private readonly NavigationStore _navigationStore;
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;


        private string _role;

        private readonly List<UserModel> authors = new List<UserModel>();
        private readonly List<string> keywords = new List<string>();
        private readonly List<string> unregistered_authors = new List<string>();
        private readonly List<string> faculties = new List<string>();

        public readonly ObservableCollection<ArticleViewModel> _articles;
        public IEnumerable<ArticleViewModel> ArticleViewModels => _articles;

        public HomePageViewModel(NavigationStore navigationStore, string userId)
        {
            _navigationStore = navigationStore;
            userid = userId;

            UserModel user1 = new UserModel("malakbenzahia", "Malak Benzahia");
            authors.Add(user1);
            unregistered_authors.Add("John Doe");
            keywords.Add("blah blah");
            faculties.Add("Faculty of blah");

            ArticleViewModel article1 = new ArticleViewModel(
                                                    new ArticleModel("these bla bla", "blabla bla bla bla", "these de master",
                                                    authors, unregistered_authors, keywords, faculties, "september 2023")
                                        );
            ArticleViewModel article2 = new ArticleViewModel(
                                                    new ArticleModel("these bla bla", "blabla bla bla bla", "these de master",
                                                    authors, unregistered_authors, keywords, faculties, "september 2023")
                                        );
            _articles = new ObservableCollection<ArticleViewModel>()
            {
                article1,
                article2
            };
        }

        
    }
}
