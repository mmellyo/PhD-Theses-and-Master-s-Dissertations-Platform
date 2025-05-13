using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using Project.Models;


namespace Project.Repos
{
    public class TheseRepo : RepoBase, ITheseRepo
    {
        public void ShowPdf(int these_id)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "SELECT these_pdf FROM `theses` WHERE these_id = @these_id";

                command.Parameters.AddWithValue("@these_id", these_id);
                var result = command.ExecuteScalar();

                if (result != DBNull.Value && result is byte[] pdfBytes && pdfBytes.Length > 0)
                {
                    string tempFilePath = Path.Combine(Path.GetTempPath(), $"thesis_{these_id}.pdf");
                    File.WriteAllBytes(tempFilePath, pdfBytes);
                    Process.Start(new ProcessStartInfo(tempFilePath) { UseShellExecute = true });
                }
                else
                {
                    // Optional: show a message to the user
                    MessageBox.Show("No PDF data found for this thesis.");
                }
            }
        }


        public string GetNomEncadrantFromTheseId(int these_id)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT NomEncadrant FROM `theses` WHERE these_id = @these_id";
                command.Parameters.AddWithValue("@these_id", these_id);

                var result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    return result.ToString();
                }
                else
                {
                    MessageBox.Show("No encadrant name found for this thesis.");
                    return null;
                }
            }
        }










        public string GetNomAuteurFromTheseId(int these_id)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                // Adjusted query with JOINs
                command.CommandText = @"
                            SELECT u.user_name
                            FROM user u
                            JOIN written_by_users wbu ON u.user_id = wbu.user_id
                            JOIN these t ON wbu.these_id = t.these_id
                            WHERE t.these_id = @these_id";

                command.Parameters.AddWithValue("@these_id", these_id);

                var result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return result.ToString();
                }
                else
                {
                    MessageBox.Show("No author name found for this thesis.");
                    return null;
                }
            }
        }

    }
}
