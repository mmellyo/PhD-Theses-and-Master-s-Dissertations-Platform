using Project.ViewModels;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Project.ViewModels
{
    public class ArticleCardViewModel : ViewModelBase
    {
		private ArticleModel _article;

		private string _title => _article.title;
		public string Title
		{
			get
			{
				return _title;
			}
			
		}

		private string _description => _article.description;
		public string Description
		{
			get
			{
				return _description;
			}
			
		}

		private string _imagePath ;
		public string ImagePath
		{
			get
			{
				return _imagePath;
			}
			set
			{
				_imagePath = value;
				OnPropertyChanged(nameof(ImagePath));
			}
		}

		private string _type => _article.type;
		public string Type
		{
			get
			{
				return _type;
			}
			
		}

		private string _date => _article.date;
		public string Date
		{
			get
			{
				return _date;
			}
			
		}

		private Brush _color;
        public Brush Color
		{
			get
			{
				return _color;
			}
			set
			{
				_color = value;
				OnPropertyChanged(nameof(Color));
			}
		}

		public ICommand DownloadCommand { get; }

		public ICommand SaveCommand { get; }

		public ICommand ShareCommand { get; }

        public ICommand FollowCommand { get; }

        public ArticleCardViewModel()
        {

        }

    }
}
