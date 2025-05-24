using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class ArticleViewModel : ViewModelBase
    {
        private readonly ArticleModel _article;

        public string title => _article.title;
        public string description => _article.description;
        public List<string> keywords => _article.keywords;
        public string date => _article.date;
        public string type => _article.type;

        public ArticleViewModel(ArticleModel article)
        {
            _article = article;
        }
 
    }
}
