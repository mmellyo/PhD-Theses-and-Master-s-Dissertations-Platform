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
    }
}
