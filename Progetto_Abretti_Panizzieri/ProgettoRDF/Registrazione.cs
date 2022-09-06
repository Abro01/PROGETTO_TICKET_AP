using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ProgettoRDF
{
    public partial class Registrazione : Form
    {
        myDBconnection con = new myDBconnection();
        MySqlDataAdapter da;
        DataTable dt = new DataTable();

        public Registrazione()
        {
            InitializeComponent();
            con.Connect();
            this.Text = string.Empty;                                           //QUESTI COMANDI PERMETTONO DI TRASCINARE LA SCHERMATA PRINCIPALE ATTRAVERSO IL PANNELLO IN CUI E' PRESENTE IL TITOLO DI OGNI PAGINA
            this.ControlBox = false;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]                 //UTILIZZATI PER NASCONDERE LA BARRA IN ALTO
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void RegistrazioneUtenti_Load(object sender, EventArgs e)
        {
            con.cn.Open();
            this.cbOrg.DropDownStyle = ComboBoxStyle.DropDownList; //Non permette di scrivere nella combobox facendola diventare di fatto una ddlist
            int i = 0;
            string queryOrganizzazioni = "SELECT nome " +
                                         "FROM organizzazione";

            da = new MySqlDataAdapter(queryOrganizzazioni, con.cn);
            da.Fill(dt);
            DataRow[] righe = dt.Select();

            for (i = 0; i < dt.Rows.Count; i++)
            {
                cbOrg.Items.Add(righe[i]["nome"]);
            }
        }

        private void btnRegistrazione_Click(object sender, EventArgs e)
        {
            string nomeOrganizzazione = this.cbOrg.GetItemText(this.cbOrg.SelectedItem);

        }

        //Cancella i caratteri nelle textbox per permettere di effettuare una nuova registrazione
        private void btnClear_Click(object sender, EventArgs e)
        {
            textEmail.Clear();
            textPassword.Clear();

            textEmail.Focus();
        }
        //Permette di effettuare la registrazione premendo il tasto invio
        private void textPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                btnRegistrazione.PerformClick();
        }

        private void rbUtente_CheckedChanged(object sender, EventArgs e)            //SELEZIONA LA REGISTRAZIONE PER GLI UTENTI
        {
            cbOrg.Hide();
            lCodOrg.Hide();
            lbUser.Show();
            textUser.Show();

        }

        private void rbOrganizzatore_CheckedChanged(object sender, EventArgs e)     //SELEZIONA LA REGISTRAZIONE PER GLI ORGANIZZATORI
        {
            lbUser.Hide();
            textUser.Hide();
            cbOrg.Show();
            lCodOrg.Show();
        }

        //CHIUDE IL PROGRAMMA
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMaximize_Click(object sender, EventArgs e)                  //GRAZIE A QUESTO BOTTONE E' POSSIBILE MASSIMIZZARE L'ESTENSIONE DELLA PAGINA TANTO QUANTO LA GRANDEZZA DELLO SCHERMO
        {
            if (WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)                  //QUESTO BOTTONE PERMETTE DI RIDURRE IL PROGRAMMA A TENDINA
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pnlTitolo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}
