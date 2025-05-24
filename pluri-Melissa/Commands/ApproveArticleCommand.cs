using Project.Repos;
using Project.Stores;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project.Commands
{
    public class ApproveArticleCommand : CommandBase
    {
        private int _article_id;
        private TheseRepo _repo;
        public ApproveArticleCommand(int article_id)
        {
            _article_id = article_id;
            _repo = new TheseRepo();
            
        }

        public override void Execute(object parameter)
        {
            _repo.approveArticle(_article_id);
        }
    }
}
