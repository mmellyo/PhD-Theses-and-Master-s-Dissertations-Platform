using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    }
}
