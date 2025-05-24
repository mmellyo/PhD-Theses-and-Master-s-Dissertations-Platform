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
        public int id;
        private string v1;
        private string v2;
        private string v3;
        public List<UserModel> authors1;
        public List<string> faculties;
        private string v4;
        internal string language;
        internal int visits;
        internal int saves;

        public ArticleModel()
        {
        }

        public ArticleModel(string v1, string v2, string v3, List<UserModel> authors1, List<string> unregistered_authors, List<string> keywords, List<string> faculties, string v4)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.authors1 = authors1;
            this.unregistered_authors = unregistered_authors;
            this.keywords = keywords;
            this.faculties = faculties;
            this.v4 = v4;
        }



        public string title { get; set; }
        public string description { get; set; }
        public string department { get; set; }

        public string supervisor { get; set; }
        public string uploader { get; set; }
        public string uploader_role { get; set; }
        public string uploader_email { get; set; }
        public Byte[] uploader_pic { get; set; }
        public string type { get; set; }
        public List<string> authors { get; set; } = new List<string>();
        public List<String> unregistered_authors { get; set; }
        public List<string> keywords { get; set; } = new List<string>();
        public string faculty { get; set; }
        public string date { get; set; }

       
    }
}
