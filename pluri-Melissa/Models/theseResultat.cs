using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace gestion.Model
{
    public class theseResultat
    {
        public string NomThese { get; set; }
        public int TheseId { get; set; }
        public string NomAuteur { get; set; }
        public string MotCles { get; set; }
        public string Faculte { get; set; }
        public string NomEncadrant { get; set; }
        public string Langue { get; set; }
        public string Diplome { get; set; }
        public string AnneeUniversitaire { get; set; }
        public string Departement { get; set; }
        public string Resume { get; set; }


    }
}
