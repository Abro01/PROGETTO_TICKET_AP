﻿using MySql.Data.MySqlClient;
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
    public partial class FormImpostazioniUtente : Form
    {
        myDBconnection con = new myDBconnection();
        public FormImpostazioniUtente()
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

            lblTitoloImpo.ForeColor = ThemeColor.SecondaryColor;
            lblNome.ForeColor = ThemeColor.SecondaryColor;
            lblCognome.ForeColor = ThemeColor.SecondaryColor;
            lblUsername.ForeColor = ThemeColor.SecondaryColor;
            lblEmail.ForeColor = ThemeColor.SecondaryColor;
            txtNome.ForeColor = ThemeColor.PrimaryColor;
            txtCognome.ForeColor = ThemeColor.PrimaryColor;
            txtUsername.ForeColor = ThemeColor.PrimaryColor;
            txtEmail.ForeColor = ThemeColor.PrimaryColor;
            btnModifica.Colore_bordo = ThemeColor.PrimaryColor;
            btnModifica.TextColor = ThemeColor.PrimaryColor;
        }

        private void FormImpostazioniUtente_Load(object sender, EventArgs e)
        {
            int IDUser = LoginInfo.UserID;

            LoadTheme();
        }

        private void btnModifica_Click(object sender, EventArgs e) //MODIFICA I DATI DELL'UTENTE LOGGATO NEL DATABASE 
        {

            txtNome.Clear();
            txtCognome.Clear(); //PULISCO LE TEXTBOX
            txtUsername.Clear();
            txtEmail.Clear();
        }

    }
}
