using MySql.Data.MySqlClient;
using Project.Models;
using Project.Repos;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project.Commands
{
    public class UploadThesisCommand : CommandBase
    {
        private UploadViewModel uploadViewModel;
        private int userId;
        private ArticleModel articleModel;
        private UserRepos userRepos;
        private TheseRepo TheseRepo;

        private string _myConnectionString = "server=127.0.0.1;uid=root;pwd=0000;database=projet_pluri";

        public UploadThesisCommand(UploadViewModel uploadViewModel, int userId)
        {
            this.uploadViewModel = uploadViewModel;
            this.userId = userId;
            userRepos = new UserRepos();
            TheseRepo = new TheseRepo();
        }

        public override void Execute(object parameter)
        {
            var vm = uploadViewModel;

            if (vm.FileContent == null)
            {
                MessageBox.Show("You must select a thesis file.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!(vm.IsLicence || vm.IsMaster || vm.IsDoctorat))
            {
                MessageBox.Show("You must select a thesis type (License, Master, or Doctorate).", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(vm.Title))
            {
                MessageBox.Show("Title is required.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(vm.Encadrant))
            {
                MessageBox.Show("Supervisor is required.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(vm.Writers))
            {
                MessageBox.Show("At least one writer is required.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(vm.Keywords))
            {
                MessageBox.Show("Keywords are required.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(vm.Faculty))
            {
                MessageBox.Show("Faculty is required.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(vm.Year))
            {
                MessageBox.Show("Year is required.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(vm.Language))
            {
                MessageBox.Show("Language is required.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(vm.Department))
            {
                MessageBox.Show("Department is required.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(vm.Summary))
            {
                MessageBox.Show("Summary is required.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

   

       
            // 2. Resolve supervisor
            var supervisor = userRepos.GetByUsername(vm.Encadrant.Trim());
            if (supervisor == null || supervisor.user_role.ToLower() != "member")
            {
                MessageBox.Show("Supervisor not found or not a member.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (var connection = new MySqlConnection(_myConnectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        // 3. Insert article
                        var cmd = new MySqlCommand("INSERT INTO articles (title, summary, article_date, posted_date, poster_id, article_file, article_type, article_state, department, faculty, language, visit_number, saves_number) VALUES (@title, @summary, @articledate, @posteddate, @userid, @file, @type, @state, @department, @faculty, @language, @visit_number, @saves_number)", connection, transaction);
                        cmd.Parameters.AddWithValue("@title", vm.Title);
                        cmd.Parameters.AddWithValue("@summary", vm.Summary);
                        cmd.Parameters.AddWithValue("@articledate", vm.Year);
                        cmd.Parameters.AddWithValue("@department", vm.Department);
                        cmd.Parameters.AddWithValue("@faculty", vm.Faculty);
                        cmd.Parameters.AddWithValue("@language", vm.Language);
                        cmd.Parameters.AddWithValue("@visit_number", 0);
                        cmd.Parameters.AddWithValue("@saves_number", 0);
                        cmd.Parameters.AddWithValue("@posteddate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@userid", userId);
                        cmd.Parameters.AddWithValue("@file", vm.FileContent);
                        string thesisType = vm.IsLicence ? "Licence" : vm.IsMaster ? "Master" : "Doctorat";
                        cmd.Parameters.AddWithValue("@type", thesisType);
                        cmd.Parameters.AddWithValue("@state", "Not Approved");
                        cmd.ExecuteNonQuery();

                        long articleId = cmd.LastInsertedId;

                        // 4. Insert keywords
                        string[] keywordList = vm.Keywords.Split(',').Select(k => k.Trim()).Where(k => k.Length > 0).ToArray();
                        foreach (var kw in keywordList)
                        {
                            long keywordId = TheseRepo.GetOrInsertKeyword(connection, transaction, kw);
                            var linkCmd = new MySqlCommand("INSERT INTO used_keywords (keyword_id, article_id) VALUES (@kid, @aid)", connection, transaction);
                            linkCmd.Parameters.AddWithValue("@kid", keywordId);
                            linkCmd.Parameters.AddWithValue("@aid", articleId);
                            linkCmd.ExecuteNonQuery();
                        }

                        // 5. Insert authors
                        string[] writers = vm.Writers.Split(',').Select(w => w.Trim()).Where(w => w.Length > 0).ToArray();
                        foreach (var writer in writers)
                        {
                            string[] names = writer.Split(' ');
                            string firstname = names.First();
                            string lastname = names.Length > 1 ? string.Join(" ", names.Skip(1)) : "";
                            var authorCmd = new MySqlCommand("INSERT INTO written_by (first_name, last_name, article_id) VALUES (@fn, @ln, @aid)", connection, transaction);
                            authorCmd.Parameters.AddWithValue("@fn", firstname);
                            authorCmd.Parameters.AddWithValue("@ln", lastname);
                            authorCmd.Parameters.AddWithValue("@aid", articleId);
                            authorCmd.ExecuteNonQuery();
                        }

                        // 6. Insert supervisor relationship
                        var supCmd = new MySqlCommand("INSERT INTO supervised_by (supervisor_id, article_id) VALUES (@sid, @aid)", connection, transaction);
                        supCmd.Parameters.AddWithValue("@sid", supervisor.user_id);
                        supCmd.Parameters.AddWithValue("@aid", articleId);
                        supCmd.ExecuteNonQuery();

                        transaction.Commit();

                        MessageBox.Show("Thesis uploaded successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error uploading thesis: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    
}
