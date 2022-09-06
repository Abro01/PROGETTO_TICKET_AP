﻿using MySql.Data.MySqlClient;
using ProgettoRDF.Forms.Utente;
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
    public partial class FormRicercaCEO : Form
    {
        myDBconnection con = new myDBconnection();
        DataTable dt = new DataTable();
        string id;
        public FormRicercaCEO()
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

            lblNomeEvento.ForeColor = ThemeColor.SecondaryColor;
            btnCerca.ForeColor = ThemeColor.PrimaryColor;
            btnCerca.Colore_bordo = ThemeColor.PrimaryColor;
            btnCerca.TextColor = ThemeColor.PrimaryColor;
        }

        public void ricercaEventi(string nomeEvento)
        {
            dtRisultati.DataSource = dt;
        }

        private void btnCerca_Click(object sender, EventArgs e)
        {
            ricercaEventi(txtNomeEvento.Text);
        }

        private void dtRisultati_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string idEvento = dtRisultati.Rows[e.RowIndex].Cells["Id"].Value.ToString();
            LoginInfo.IdEvento = Int16.Parse(idEvento);
            FormAcquistoBigliettoCEO fa = new FormAcquistoBigliettoCEO();
            fa.Show();
            this.Hide();
        }

        private void FormRicercaCEO_Load(object sender, EventArgs e)
        {
            LoadTheme();

            string query = "SELECT e.ID, e.nome, e.genere, e.luogo, e.Nposti " +
                           "FROM eventi e, organizzazione o, ceo_organizzazioni c " +
                           "WHERE c.CODOrganizzazione=o.ID AND o.ID=e.CODOrganizzazione " + //MOSTRO TUTTI GLI EVENTI NON ORGANIZZATI DALLE ORGANIZZAZIONI DEL CEO LOGGATO
                           "AND c.ID != '" + LoginInfo.UserID + "'";

            command = new MySqlCommand(query, con.cn);
            da = new MySqlDataAdapter(command);
            da.Fill(dt);

            dtRisultati.DataSource = dt;
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            dtRisultati.Columns.Add(btn);
            btn.HeaderText = "Click Data";
            btn.Text = "ACQUISTA";
            btn.Name = "btn";
            btn.UseColumnTextForButtonValue = true;
        }
    }
}
