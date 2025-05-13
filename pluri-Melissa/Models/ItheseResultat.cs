using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gestion.Model;

namespace Project.Models
{
    public interface ItheseResultat
    {
        public List<theseResultat> rechercherAvecFiltres(string auteur, string encadrant, string these, string motcle, string langue, string dep, string annee, string fac);
        public List<theseResultat> rechercheThese(string key);
    }
}
