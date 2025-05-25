using MySql.Data.MySqlClient;
using Project.Commands;
using Project.Models;
using Project.Repos;
using Project.Stores;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PdfiumViewer;

namespace Project.ViewModels
{
    public class MarkedThesesViewModel : ViewModelBase
    {
        private TheseRepo _theseRepo;

        private NavigationStore navigationStore;
        private ArticleModel _article { get; }
        public string These_title => _article.title;
        public string authorName => _article.uploader;
        public string facultyName => _article.faculty;
        public string Diplome => _article.type;
        public string Langue => _article.language;
        public string AnneeUniversitaire => _article.date;
        public string Department => _article.department;

        public ImageSource PdfFirstPageImage => LoadPdfImage(_article.id);

        public ICommand TheseClickCommand { get; set; }

        public MarkedThesesViewModel(NavigationStore navigationStore, ArticleModel article, int userid)
        {
            this._theseRepo = new TheseRepo();
            this.navigationStore = navigationStore;
            _article = article;
            TheseClickCommand = new NavigateCommand<ThesePageViewModel>(navigationStore, () => new ThesePageViewModel(navigationStore, userid, _article.id));
        }

        private ImageSource LoadPdfImage(int articleId)
        {
            byte[]? pdfBytes = _theseRepo.GetPdfBytesFromDatabase(articleId);
            ImageSource PdfFirstPageImage;

            PdfFirstPageImage = GetFirstPageImage(pdfBytes); 

            return PdfFirstPageImage;
        }



        private ImageSource GetFirstPageImage(byte[] pdfBytes)
        {
            using var stream = new MemoryStream(pdfBytes);
            using var document = PdfDocument.Load(stream);
            using var image = document.Render(0, 300, 300, true); // Render first page
            return BitmapToImageSource((Bitmap)image);
        }



        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using MemoryStream memory = new();
            bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
            memory.Position = 0;

            BitmapImage bitmapImage = new();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = memory;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            return bitmapImage;
        }
    }
}









    
