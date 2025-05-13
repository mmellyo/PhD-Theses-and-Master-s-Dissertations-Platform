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
                    conditions.Add("nomAuteur LIKE @auteur");
                    cmd.Parameters.AddWithValue("@auteur", auteur + "%");
                }
                if (!string.IsNullOrWhiteSpace(encadrant))
                {
                    conditions.Add("NomEncadrant LIKE @encadrant");
                    cmd.Parameters.AddWithValue("@encadrant", encadrant + "%");
                }
                if (!string.IsNullOrWhiteSpace(these))
                {
                    conditions.Add("nomThese LIKE @these");
                    cmd.Parameters.AddWithValue("@these", these + "%");
                }
                if (!string.IsNullOrWhiteSpace(motcle))
                {
                    conditions.Add("MotCles LIKE @motcle");
                    cmd.Parameters.AddWithValue("@motcle", motcle + "%");
                }
                if (!string.IsNullOrWhiteSpace(langue))
                {
                    conditions.Add("Langue = @langue");
                    cmd.Parameters.AddWithValue("@langue", langue);
                }
                if (!string.IsNullOrWhiteSpace(dep))
                {
                    conditions.Add("Departement = @dep");
                    cmd.Parameters.AddWithValue("@dep", dep);
                }
                if (!string.IsNullOrWhiteSpace(annee))
                {
                    conditions.Add("AnneeUniversitaire = @annee");
                    cmd.Parameters.AddWithValue("@annee", annee);
                }
                if (!string.IsNullOrWhiteSpace(fac))
                {
                    conditions.Add("Faculte = @faculte");
                    cmd.Parameters.AddWithValue("@faculte", fac);
                }

                string sql = "SELECT * FROM theses";
                if (conditions.Count > 0)
                    sql += " WHERE " + string.Join(" AND ", conditions);

                cmd.CommandText = sql;

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        resultats.Add(new theseResultat
                        {
                            TheseId = Convert.ToInt32(reader["these_id"]),
                            NomThese = reader["nomThese"].ToString(),
                            NomAuteur = reader["nomAuteur"].ToString(),
                            MotCles = reader["MotCles"].ToString(),
                            Faculte = reader["Faculte"].ToString(),
                            Departement = reader["Departement"].ToString(),
                            NomEncadrant = reader["NomEncadrant"].ToString(),
                            Langue = reader["Langue"].ToString(),
                            Diplome = reader["Diplome"].ToString(),
                            AnneeUniversitaire = reader["AnneeUniversitaire"].ToString(),
                            Resume = reader["Resume"].ToString(),
                        });
                    }
                }
            }

            return resultats;
        }





        public List<theseResultat> rechercheThese(string key)
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
        }
    }
    }
