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
    public class suggestionRechercheRepos : RepoBase, IsuggestionRecherche

    {
        public static List<suggestionRecherche> list = new List<suggestionRecherche>();


        public List<suggestionRecherche> recherche(string key)
        {
            List<suggestionRecherche> list = new List<suggestionRecherche>();

            using (var connection = GetConnection())
            {
                connection.Open();
                string sql = @"
                SELECT DISTINCT t.nomThese, u.user_name AS NomAuteur, k.keyword AS MotCles
                FROM theses t
                JOIN written_by_users w ON t.these_id = w.these_id
                JOIN user u ON u.user_id = w.user_id
                LEFT JOIN used_keywords uk ON uk.these_id = t.these_id
                LEFT JOIN keywords k ON k.keyword_id = uk.keyword_id
                WHERE t.nomThese LIKE @key 
                   OR u.user_name LIKE @key 
                   OR k.keyword LIKE @key";

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
                                NomAuteur = reader["NomAuteur"].ToString(),
                                MotCles = reader["MotCles"]?.ToString() ?? ""
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
