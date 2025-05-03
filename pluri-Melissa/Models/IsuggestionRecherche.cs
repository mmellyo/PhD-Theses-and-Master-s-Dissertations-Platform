using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gestion.Model;

namespace Project.Models
{
    interface IsuggestionRecherche
    {
        public List<suggestionRecherche> recherche(string key);
    }
}
