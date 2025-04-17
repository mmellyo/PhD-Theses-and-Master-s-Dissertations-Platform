using Project.Models;
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
    internal class HomePageViewModel
    {
        public ObservableCollection<ArticleModel> Articles { get; set; }

        public HomePageViewModel()
        {
            Articles = new ObservableCollection<ArticleModel>(LoadArticlesFromCsv("articles_final.csv"));
        }

        private List<ArticleModel> LoadArticlesFromCsv(string path)
        {
            var lines = File.ReadAllLines(path).Skip(1); // skip header
            return lines.Select(line =>
            {
                var parts = line.Split(',');
                return new ArticleModel
                {
                    title = parts[1],
                    description = parts[2],
                    type = parts[5],
                    faculties = new List<string> { parts[4] }, // Assuming only one faculty per article here
                    IsSaved = parts[6] == "1",
                    keywords = parts[3].Split(';').ToList(),
                    authors = new List<UserModel>(), // Add authors if you have that data elsewhere
                    unregistered_authors = new List<string>() // Add unregistered authors if you have that data elsewhere
                };
            }).ToList();
        }
    }
}
