using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class ArticleModel
    {
        public string title { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public List<UserModel> authors { get; set; }
        public List<String> unregistered_authors { get; set; }
        public List<string> keywords { get; set; }
        public List<string> faculties { get; set; }
        public string date { get; set; }
        public bool IsSaved { get; set; }

        public ArticleModel(string title, string description, string type, List<UserModel> authors, List<string> unregistered_authors, List<string> keywords, List<string> faculties, string date)
        {
            this.title = title;
            this.description = description;
            this.type = type;
            this.authors = authors;
            this.unregistered_authors = unregistered_authors;
            this.keywords = keywords;
            this.faculties = faculties;
            this.date = date;
        }
    }
}
