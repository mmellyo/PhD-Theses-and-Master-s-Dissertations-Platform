using Project.Models;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class HomePageModel
    {
        //a home page is dedicated to a user
        private readonly UserModel _user;

        public HomePageModel(UserModel user)
        {
            _user = user;
        }

        //i forgot what this is for ngl
        public int /*IEnumerable<ArticleModel>*/ GetSuggestedArticles()
        {
            return 0;
        }

        //method for generating articles recommended based on the faculty of a new user
        public List<ArticleModel> FindSuggestedArticles(string faculty)
        {
            List<ArticleModel> _articles = new List<ArticleModel>();
            foreach (var article in _articles)
            {
                foreach (var fac in article.faculties) 
                { 
                    if(fac == faculty)
                    {
                        _articles.Add(article);
                    }
                }
            }
            return _articles;
        }

        //method for generating articles recommended based on the saved list of a user using ML
        public List<ArticleModel> FindSuggestedArticles()
        {
            List<ArticleModel> _articles = new List<ArticleModel>();
           // _articles = articlesRecommender(_user.user_id);
            return _articles;
        }

    }
}
