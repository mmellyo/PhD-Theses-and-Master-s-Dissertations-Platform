using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace gestion.Model
{
    public class suggestionRecherche
    {

        string connstring = "server=LOCALHOST;user=root;password=#Yacinestpasla3;database=rechercheinfo;";
        public string NomThese { get; set; }
        public string NomAuteur { get; set; }
        public string MotCles { get; set; }

        public static List<suggestionRecherche> list = new List<suggestionRecherche>();

        public void recherche(string key)
        {
            MySqlConnection conn = new MySqlConnection(connstring);
            conn.Open();
            string sql = @"SELECT * FROM theseinfo 
               WHERE nomThese LIKE @key 
                  OR nomAuteur LIKE @key 
                  OR MotCles LIKE @key";
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText=sql;
            cmd.Parameters.AddWithValue("@key", key + "%");
            MySqlDataReader reader = cmd.ExecuteReader();
            list.Clear();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    suggestionRecherche data = new suggestionRecherche
                    {
                        NomThese = reader["nomThese"].ToString(),
                        NomAuteur = reader["nomAuteur"].ToString(),
                        MotCles = reader["MotCles"].ToString(),
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
