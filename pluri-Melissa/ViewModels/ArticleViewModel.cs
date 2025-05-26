using Project.Commands;
using Project.Models;
using Project.Repos;
using Project.Stores;
using Project.Utils;
using Project.View.userControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Project.ViewModels
{
    public class ArticleViewModel : ViewModelBase
    {
        private readonly ArticleModel _article;
        private TheseRepo TheseRepo;
        public string Title => _article.title;
        public string Summary => _article.description;
        public string Author1_name => _article.uploader;
        public ImageSource Author1_pfp => ByteArrayToImageConverter.LoadImageSourceFromBytes(_article.uploader_pic);
        public string Date => _article.date;
        public string Type => _article.type;


        public ICommand Download {  get; set; }
        public ICommand Save { get; set; }
        public ICommand GoToThesis { get; set; }
        public ArticleViewModel(int theseId, NavigationStore navigationStore, int userid)
        {
            TheseRepo = new TheseRepo();

            _article = TheseRepo.GetThesisDetails(theseId);

            Download = new DownloadCommand(navigationStore, theseId);
            Save = new SaveCommand(navigationStore, theseId, userid);

            GoToThesis = new NavigateCommand<ThesePageViewModel>(navigationStore, () => new ThesePageViewModel(navigationStore, userid, theseId));
        }
 
    }
}
