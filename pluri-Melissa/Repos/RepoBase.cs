using System;
using System.Windows;
using MySql.Data.MySqlClient; 
namespace Project.Repos
{
    public abstract class RepoBase
    {
        private readonly string _myConnectionString;

        public RepoBase()
        {
            _myConnectionString = "server=127.0.0.1;uid=root;pwd=#Yacinestpasla3;database=pluri_bd";
        }

        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(_myConnectionString); //  MySQL connection instance
        }
    }
}
