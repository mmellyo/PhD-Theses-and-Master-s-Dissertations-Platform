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
                                SELECT DISTINCT 
                                a.title AS NomThese,
                                COALESCE(CONCAT(w.first_name, ' ', w.last_name), u.user_name) AS NomAuteur,
                                k.keyword AS MotCles
                                FROM articles a
                                LEFT JOIN users u ON a.poster_id = u.user_id
                                LEFT JOIN written_by w ON a.article_id = w.article_id
                                LEFT JOIN used_keywords uk ON uk.article_id = a.article_id
                                LEFT JOIN keywords k ON k.keyword_id = uk.keyword_id
                                WHERE 
                                a.title LIKE @key
                                OR u.user_name LIKE @key
                                OR CONCAT(w.first_name, ' ', w.last_name) LIKE @key
                                OR k.keyword LIKE @key";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@key", "%" + key + "%");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            suggestionRecherche data = new suggestionRecherche
                            {
                                NomThese = reader["NomThese"]?.ToString() ?? "",
                                NomAuteur = reader["NomAuteur"]?.ToString() ?? "",
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
