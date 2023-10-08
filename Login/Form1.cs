using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace Login
{
    public partial class Form1 : Form
    {
        MySqlConnectionManager connexion;
        public Form1()
        {
            InitializeComponent();
            connexion = new MySqlConnectionManager("localhost", "test", "root", "");
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            connexion.ShowUsers(txtLogin, txtMdp, lblTest);
        }
    }
}
