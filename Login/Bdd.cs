using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace Login
{
    public class MySqlConnectionManager
    {
        private MySqlConnection connexion;
        private string connexionString;

        public MySqlConnectionManager(string server, string database, string username, string password)
        {
            // Construisez la chaîne de connexion MySQL
            connexionString = $"Server={server};Database={database};User ID={username};Password={password};";
            connexion = new MySqlConnection(connexionString);
        }

        public bool OpenConnection()
        {
            try
            {
                if (connexion.State == System.Data.ConnectionState.Closed)
                {
                    connexion.Open();
                }
                return true;
            }
            catch (MySqlException ex)
            {
                // Gérez les erreurs de connexion ici
                Console.WriteLine("Erreur de connexion à la base de données : " + ex.Message);
                return false;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                if (connexion.State == System.Data.ConnectionState.Open)
                {
                    connexion.Close();
                }
                return true;
            }
            catch (MySqlException ex)
            {
                // Gérez les erreurs de fermeture de connexion ici
                Console.WriteLine("Erreur de fermeture de connexion : " + ex.Message);
                return false;
            }
        }

        // Vous pouvez ajouter des méthodes pour exécuter des requêtes SQL ici
        // Par exemple, une méthode pour exécuter une requête SELECT ou une méthode pour exécuter une requête INSERT/UPDATE/DELETE.

        public MySqlConnection GetConnection()
        {
            return connexion;
        }

        public void ShowUsers(TextBox textBox1, TextBox textBox2, System.Windows.Forms.Label label3)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                label3.Text = "Veuillez remplir tous les champs.";
            }
            else if (!login.Contains("@") && !login.Contains("."))
            {
                label3.Text = "Votre email doit au moins contenir un @ et un .";
            }
            else if (password.Length < 4)
            {
                    label3.Text = "Votre mot de passe doit contenir plus 4 caractères";
            }
            else
            {
                OpenConnection();
                string query = "SELECT * FROM users WHERE email = @login AND mdp = @mdp";
                MySqlCommand command = new MySqlCommand(query, connexion);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@mdp", password);

                try
                {
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        label3.Text = "Vous êtes connecté";
                    }
                    else
                    {
                        label3.Text = "Identifiants incorrects";
                    }
                    reader.Close();
                }
                catch (MySqlException ex)
                {
                    // Gérer l'erreur de base de données ici, par exemple afficher un message d'erreur
                    label3.Text = "Erreur de base de données : " + ex.Message;
                }

                CloseConnection();
            } 
        }
    }
}
