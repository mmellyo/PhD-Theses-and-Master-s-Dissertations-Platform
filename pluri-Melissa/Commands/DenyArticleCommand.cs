using Project.Repos;
using Project.Stores;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Commands
{
    public class DenyArticleCommand : CommandBase
    {
        private int _article_id;
        private TheseRepo _repo;
        public DenyArticleCommand(int article_id)
        {
            _article_id = article_id;
            _repo = new TheseRepo();
            
        }

        public override void Execute(object parameter)
        {
            _repo.denyArticle(_article_id);
        }
    }
}
