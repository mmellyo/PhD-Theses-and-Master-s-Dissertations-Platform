using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gestion.Model;
using MySql.Data.MySqlClient;
using Project.Models;

namespace Project.Repos
{
    public class suggestionRechercheRepos : RepoBase , IsuggestionRecherche
 
    {
        public static List<suggestionRecherche> list = new List<suggestionRecherche>();


        public List<suggestionRecherche> recherche(string key)
        {
            List<suggestionRecherche> list = new List<suggestionRecherche>();

            using (var connection = GetConnection())
            {
                connection.Open();
                string sql = @"SELECT * FROM theses
                       WHERE nomThese LIKE @key 
                          OR nomAuteur LIKE @key 
                          OR MotCles LIKE @key";
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@key", key + "%");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
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
                }
            }

            return list;
        }
    }
}
