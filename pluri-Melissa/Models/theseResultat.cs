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

        string connstring = "server=LOCALHOST;user=root;password=#Yacinestpasla3;database=rechercheinfo;";
        public string NomThese { get; set; }
        public string NomAuteur { get; set; }
        public string MotCles { get; set; }
        public string Filiere { get; set; }
        public string NomEncadrant { get; set; }
        public string Langue { get; set; }
        public string Diplome { get; set; }
        public string AnneeUniversitaire { get; set; }

        public static List<theseResultat> list = new List<theseResultat>();

        public void rechercheThese(string key)
        {
            MySqlConnection conn = new MySqlConnection(connstring);
            conn.Open();
            string sql = @"SELECT * FROM theseinfo 
               WHERE nomThese LIKE @key 
                  OR nomAuteur LIKE @key 
                  OR MotCles LIKE @key
                  OR Filiere LIKE @key
                  OR NomEncadrant LIKE @key
                  OR Langue LIKE @key
                  OR Diplome LIKE @key
                  OR AnneeUniversitaire LIKE @key";
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@key", key + "%");
            MySqlDataReader reader = cmd.ExecuteReader();
            list.Clear();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    theseResultat data = new theseResultat
                    {
                        NomThese = reader["nomThese"].ToString(),
                        NomAuteur = reader["nomAuteur"].ToString(),
                        MotCles = reader["MotCles"].ToString(),
                        Filiere = reader["Filiere"].ToString(),
                        NomEncadrant = reader["NomEncadrant"].ToString(),
                        Langue = reader["Langue"].ToString(),
                        Diplome = reader["Diplome"].ToString(),
                        AnneeUniversitaire = reader["AnneeUniversitaire"].ToString(),
                    };
                    list.Add(data);
                }
            }
            reader.DisposeAsync();
            cmd.Dispose();
            conn.Close();
        }

    }
}
