using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgettoRDF.Forms.Utente
{
    public partial class FormProfiloUtente : Form
    {
        myDBconnection con = new myDBconnection();
        public FormProfiloUtente()
        {
            InitializeComponent();
            con.Connect();
        }

        private void LoadTheme()
        {
            foreach (Control btns in this.Controls)
            {
                if (btns.GetType() == typeof(Button))
                {
                    Button btn = (Button)btns;
                    btn.BackColor = ThemeColor.PrimaryColor;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
                }
            }

            lblTitoloAccount.ForeColor = ThemeColor.SecondaryColor;
            lblNome.ForeColor = ThemeColor.SecondaryColor;
            lblCognome.ForeColor = ThemeColor.SecondaryColor;
            lblUsername.ForeColor = ThemeColor.SecondaryColor;
            lblEmail.ForeColor = ThemeColor.SecondaryColor;
            lblNome2.ForeColor = ThemeColor.PrimaryColor;
            lblCognome2.ForeColor = ThemeColor.PrimaryColor;
            lblUsername2.ForeColor = ThemeColor.PrimaryColor;
            lblEmail2.ForeColor = ThemeColor.PrimaryColor;
        }

        private void FormProfiloUtente_Load(object sender, EventArgs e)
        {
            int IDUser = LoginInfo.UserID;

            LoadTheme();

        }
    }
}
