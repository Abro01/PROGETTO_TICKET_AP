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

namespace ProgettoRDF.Forms.CEO
{
    public partial class FormInfoEventi : Form
    {
        myDBconnection con = new myDBconnection();
        DataTable dt = new DataTable();
        public FormInfoEventi()
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

            lblTitolo.ForeColor = ThemeColor.SecondaryColor;
        }

        private void FormInfoEventi_Load(object sender, EventArgs e)
        {
            int IDEvento = LoginInfo.IdEvento;

            LoadTheme();



            dtUtenti.DataSource = dt;
        }
    }
}
