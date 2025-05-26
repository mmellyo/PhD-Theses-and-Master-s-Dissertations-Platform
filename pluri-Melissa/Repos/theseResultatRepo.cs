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
    public class theseResultatRepo : RepoBase, ItheseResultat
    {

        /// laterrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr  
        public List<theseResultat> rechercherAvecFiltres(string auteur, string encadrant, string these, string motcle, string langue, string dep, string annee, string fac)
        {
            List<theseResultat> resultats = new List<theseResultat>();

            using (var connection = GetConnection())
            {
                connection.Open();
                List<string> conditions = new List<string>();
                MySqlCommand cmd = connection.CreateCommand();

                if (!string.IsNullOrWhiteSpace(auteur))
                {
                    conditions.Add("(u.user_name LIKE @auteur OR CONCAT(w.first_name, ' ', w.last_name) LIKE @auteur)");
                    cmd.Parameters.AddWithValue("@auteur", "%" + auteur + "%");
                }
                if (!string.IsNullOrWhiteSpace(encadrant))
                {
                    conditions.Add("sup.user_name LIKE @encadrant");
                    cmd.Parameters.AddWithValue("@encadrant", "%" + encadrant + "%");
                }
                if (!string.IsNullOrWhiteSpace(these))
                {
                    conditions.Add("a.title LIKE @these");
                    cmd.Parameters.AddWithValue("@these", "%" + these + "%");
                }
                if (!string.IsNullOrWhiteSpace(motcle))
                {
                    conditions.Add("k.keyword LIKE @motcle");
                    cmd.Parameters.AddWithValue("@motcle", "%" + motcle + "%");
                }
                if (!string.IsNullOrWhiteSpace(langue))
                {
                    conditions.Add("a.language = @langue");
                    cmd.Parameters.AddWithValue("@langue", langue);
                }
                if (!string.IsNullOrWhiteSpace(dep))
                {
                    conditions.Add("a.department = @dep");
                    cmd.Parameters.AddWithValue("@dep", dep);
                }
                if (!string.IsNullOrWhiteSpace(annee))
                {
                    conditions.Add("a.article_date = @annee");
                    cmd.Parameters.AddWithValue("@annee", annee);
                }
                if (!string.IsNullOrWhiteSpace(fac))
                {
                    conditions.Add("a.faculty = @fac");
                    cmd.Parameters.AddWithValue("@fac", fac);
                }

                string sql = @"
                SELECT 
                a.article_id,
                a.title AS nomThese,
                a.summary AS resume,
                a.faculty AS Faculte,
                a.department AS Departement,
                a.language AS Langue,
                a.article_type AS Diplome,
                a.article_date AS AnneeUniversitaire,
                GROUP_CONCAT(DISTINCT k.keyword SEPARATOR ', ') AS MotCles,
                GROUP_CONCAT(DISTINCT 
                    COALESCE(CONCAT(w.first_name, ' ', w.last_name), u.user_name)
                    SEPARATOR ', ') AS nomAuteur,
                MAX(sup.user_name) AS NomEncadrant
            FROM articles a
            LEFT JOIN users u ON a.poster_id = u.user_id
            LEFT JOIN written_by w ON a.article_id = w.article_id
            LEFT JOIN supervised_by sb ON a.article_id = sb.article_id
            LEFT JOIN users sup ON sb.supervisor_id = sup.user_id
            LEFT JOIN used_keywords uk ON uk.article_id = a.article_id
            LEFT JOIN keywords k ON uk.keyword_id = k.keyword_id
        ";

                if (conditions.Count > 0)
                {
                    sql += " WHERE " + string.Join(" AND ", conditions);
                }

                sql += " GROUP BY a.article_id";

                cmd.CommandText = sql;

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        resultats.Add(new theseResultat
                        {
                            TheseId = Convert.ToInt32(reader["article_id"]),
                            NomThese = reader["nomThese"]?.ToString() ?? "",
                            Resume = reader["resume"]?.ToString() ?? "",
                            Faculte = reader["Faculte"]?.ToString() ?? "",
                            Departement = reader["Departement"]?.ToString() ?? "",
                            Langue = reader["Langue"]?.ToString() ?? "",
                            Diplome = reader["Diplome"]?.ToString() ?? "",
                            AnneeUniversitaire = reader["AnneeUniversitaire"]?.ToString() ?? "",
                            MotCles = reader["MotCles"]?.ToString() ?? "",
                            NomAuteur = reader["nomAuteur"]?.ToString() ?? "",
                            NomEncadrant = reader["NomEncadrant"]?.ToString() ?? ""
                        });
                    }
                }
            }

            return resultats;
        }





        /*public List<theseResultat> rechercheThese(string key)
        {
            List<theseResultat> results = new List<theseResultat>();

            using (var connection = GetConnection())
            {
                connection.Open();
                string sql = @"
                            SELECT t.*, 
                            MAX(u.user_name) AS nomAuteur,
                            MAX(u.user_faculte) AS user_faculte, 
                            MAX(u.user_departement) AS user_departement,
                            GROUP_CONCAT(k.keyword SEPARATOR ', ') AS MotCles
                            FROM theses t
                            JOIN written_by_users w ON t.these_id = w.these_id
                            JOIN user u ON w.user_id = u.user_id
                            LEFT JOIN used_keywords uk ON t.these_id = uk.these_id
                            LEFT JOIN keywords k ON uk.keyword_id = k.keyword_id
                            WHERE t.nomThese LIKE @key 
                            OR u.user_name LIKE @key 
                            OR k.keyword LIKE @key
                            OR u.user_faculte LIKE @key
                            OR u.user_departement LIKE @key
                            OR t.NomEncadrant LIKE @key
                            OR t.Langue LIKE @key
                            OR t.Diplome LIKE @key
                            OR t.AnneeUniversitaire LIKE @key
                            GROUP BY t.these_id
                            ";

                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@key", key + "%");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            theseResultat data = new theseResultat
                            {
                                TheseId = Convert.ToInt32(reader["these_id"]),
                                NomThese = reader["nomThese"].ToString(),
                                NomAuteur = reader["nomAuteur"].ToString(),
                                MotCles = reader["MotCles"].ToString(),
                                Faculte = reader["user_faculte"].ToString(),
                                Departement = reader["user_departement"].ToString(),
                                NomEncadrant = reader["NomEncadrant"].ToString(),
                                Langue = reader["Langue"].ToString(),
                                Diplome = reader["Diplome"].ToString(),
                                AnneeUniversitaire = reader["AnneeUniversitaire"].ToString(),
                                Resume = reader["resume"].ToString(),
                            };
                            results.Add(data);
                        }
                    }
                }
            }

            return results;
        }*/

        public List<theseResultat> rechercheThese(string key)
        {
            var results = new List<theseResultat>();
            if (string.IsNullOrWhiteSpace(key))
                return results;

            using (var connection = GetConnection())
            {
                connection.Open();

                string sql = @"
                                    SELECT 
                                        a.article_id,
                                        a.title AS nomThese,
                                        a.summary AS resume,
                                        a.faculty AS user_faculte,
                                        a.department AS user_departement,
                                        a.language AS Langue,
                                        a.article_type AS Diplome,
                                        a.article_date AS AnneeUniversitaire,
                                        GROUP_CONCAT(DISTINCT 
                                            COALESCE(CONCAT(w.first_name, ' ', w.last_name), u.user_name)
                                            SEPARATOR ', '
                                        ) AS nomAuteur,
                                        MAX(sup.user_name) AS NomEncadrant,
                                        GROUP_CONCAT(DISTINCT k.keyword SEPARATOR ', ') AS MotCles
                                    FROM articles a
                                    LEFT JOIN users u ON a.poster_id = u.user_id
                                    LEFT JOIN written_by w ON a.article_id = w.article_id
                                    LEFT JOIN supervised_by sb ON a.article_id = sb.article_id
                                    LEFT JOIN users sup ON sb.supervisor_id = sup.user_id
                                    LEFT JOIN used_keywords uk ON a.article_id = uk.article_id
                                    LEFT JOIN keywords k ON uk.keyword_id = k.keyword_id
                                    WHERE 
                                        a.title LIKE @key
                                        OR a.summary LIKE @key
                                        OR u.user_name LIKE @key
                                        OR CONCAT(w.first_name, ' ', w.last_name) LIKE @key
                                        OR sup.user_name LIKE @key
                                        OR a.faculty LIKE @key
                                        OR a.department LIKE @key
                                        OR a.language LIKE @key
                                        OR a.article_type LIKE @key
                                        OR a.article_date LIKE @key
                                        OR k.keyword LIKE @key
                                    GROUP BY a.article_id
                                    HAVING COUNT(*) > 0
                                            ";
                using (var cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@key", "%" + key + "%");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new theseResultat
                            {
                                TheseId = Convert.ToInt32(reader["article_id"]),
                                NomThese = reader["nomThese"]?.ToString() ?? "",
                                Resume = reader["resume"]?.ToString() ?? "",
                                Faculte = reader["user_faculte"]?.ToString() ?? "",
                                Departement = reader["user_departement"]?.ToString() ?? "",
                                Langue = reader["Langue"]?.ToString() ?? "",
                                Diplome = reader["Diplome"]?.ToString() ?? "",
                                AnneeUniversitaire = reader["AnneeUniversitaire"]?.ToString() ?? "",
                                MotCles = reader["MotCles"]?.ToString() ?? "",
                                NomAuteur = reader["nomAuteur"]?.ToString() ?? "",
                                NomEncadrant = reader["NomEncadrant"]?.ToString() ?? ""
                            });
                        }
                    }
                }
            }

            return results;
        }


    }


}
