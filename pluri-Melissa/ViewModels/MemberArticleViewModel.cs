using Microsoft.Xaml.Behaviors.Media;
using Project.Commands;
using Project.Models;
using Project.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project.ViewModels
{
    public class MemberArticleViewModel :ViewModelBase
    {
        private NavigationStore navigationStore;
        public ArticleModel _article { get; }
        public string Title => _article.title;

        public string PosterName => _article.uploader;

        public ICommand GoToArticle { get; }
        public ICommand GoToPosterProfile { get; }
        public ICommand ApproveCommand { get; }
        public ICommand DenyCommand { get; }


        public MemberArticleViewModel(int user_id, NavigationStore navigationStore, ArticleModel article)
        {
            this.navigationStore = navigationStore;
            _article = article;

            GoToArticle = new NavigateCommand<ThesePageViewModel>(navigationStore,()=> new ThesePageViewModel(navigationStore, user_id ,_article.id));
            GoToPosterProfile = new NavigateCommand<UserProfileViewModel>(navigationStore, () => new UserProfileViewModel(navigationStore, user_id ));

            ApproveCommand = new ApproveArticleCommand(article.id);
            DenyCommand = new DenyArticleCommand(article.id);

        }


    }
}
