using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;

namespace ProgettoRDF
{
    // NOTA: è possibile utilizzare il comando "Rinomina" del menu "Refactoring" per modificare il nome di classe "DBService" nel codice e nel file di configurazione contemporaneamente.
    public class DBService : IDBService
    {
        myDBconnection con = new myDBconnection();

        public void acquistaBigliettoCEO(string app, int IDEvento, int IDUser, bool premium, int IDBiglietto, int costo, int prezzo)
        {
            DataTable dt = new DataTable();
            MySqlDataAdapter dt2;
            DataTable DataTablePosti = new DataTable();
            int numBig = Int32.Parse(app);

            con.cn.Open();

            string queryPosti = "SELECT Nposti " +
                                "FROM eventi " +
                                "WHERE ID = '" + LoginInfo.IdEvento + "'";

            dt2 = new MySqlDataAdapter(queryPosti, con.cn);
            dt2.Fill(DataTablePosti);
            DataRow[] righe = DataTablePosti.Select();
            string postiSelect = righe[0]["Nposti"].ToString(); //QUERY PER TROVARE IL NUMERO DI POSTI E LO CONRONTO CON I BIGLIETTI CHE VOGLIO ACQUISTARE
            int Nposti = Int32.Parse(postiSelect);

            if (Nposti >= numBig) //CONTROLLO CHE CI SIANO BIGLIETTI
            {
                for (int i = numBig; i > 0; i--)
                {
                    if (premium)//CONTROLLO CHE IL FLAG PREMIUM SIA SELEZIONATO OPPURE NO, IN BASE A QUESTO ACQUISTO BIGLIETTI PREMIUM O NORMALI
                    {
                        costo = costo + 50;
                        try
                        {
                            string query = $"INSERT INTO ceo_biglietti  (`ID`, `Premium`, `CODBiglietto`, `CODCeo`) VALUES ('', 'SI', '" + IDBiglietto + "', '" + IDUser + "');";
                            MySqlCommand command = new MySqlCommand(query, con.cn);
                            command.ExecuteNonQuery();
                        }
                        catch (Exception)
                        {
                            throw new Exception();
                        }
                    }
                    else
                    {
                        try
                        {
                            string query = $"INSERT INTO ceo_biglietti  (`ID`, `Premium`, `CODBiglietto`, `CODCeo`) VALUES ('', 'NO', '" + IDBiglietto + "', '" + IDUser + "');";
                            MySqlCommand command = new MySqlCommand(query, con.cn);
                            command.ExecuteNonQuery();
                        }
                        catch (Exception)
                        {
                            throw new Exception();
                        }
                    }
                }
                string queryE = "UPDATE eventi " +
                                 "SET Nposti = Nposti - " + numBig +
                                 " WHERE ID = '" + LoginInfo.IdEvento + "'";

                MySqlCommand commandE = new MySqlCommand(queryE, con.cn);
                commandE.ExecuteNonQuery();
                con.cn.Close();

                MessageBox.Show("Acquisto riusciuto con successo, costo totale: €" + costo.ToString());
            }
            else
            {
                MessageBox.Show("MI DISPIACE MA QUEST'EVENTO E' GIA' SOLD OUT");
            }
        }

        public void acquistaBigliettoUtente(string app, int IDEvento, int IDUser, bool premium, int IDBiglietto, int costo, int prezzo)
        {
            DataTable dt = new DataTable();
            MySqlDataAdapter dt2;
            DataTable DataTablePosti = new DataTable();

            int numBig = Int32.Parse(app);
            con.cn.Open();
            string queryPosti = "SELECT Nposti " +
                                "FROM eventi " +
                                "WHERE ID = '" + LoginInfo.IdEvento + "'";

            dt2 = new MySqlDataAdapter(queryPosti, con.cn);
            dt2.Fill(DataTablePosti);
            DataRow[] righe = DataTablePosti.Select();
            string postiSelect = righe[0]["Nposti"].ToString();
            int Nposti = Int32.Parse(postiSelect);


            if (Nposti >= numBig) //CONTROLLO CHE CI SIANO BIGLIETTI
            {
                for (int i = numBig; i > 0; i--) //CICLO UTILE NEL CASO SI ACQUISTI PIU' DI UN BIGLIETTO 
                {

                    if (premium)
                    {
                        costo = costo + 50;
                        try
                        {
                            string query = $"INSERT INTO utenti_biglietti  (`ID`, `Premium`, `CODUtente`, `CODBiglietto`) VALUES ('', 'SI', '" + IDUser + "', '" + IDBiglietto + "');";
                            MySqlCommand command = new MySqlCommand(query, con.cn); //INSERIMENTO IN CASO SIA UN BIGLIETTO PREMIUM
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            string query = $"INSERT INTO utenti_biglietti  (`ID`, `Premium`, `CODUtente`, `CODBiglietto`) VALUES ('', 'NO', '" + IDUser + "', '" + IDBiglietto + "');";
                            MySqlCommand command = new MySqlCommand(query, con.cn); //INSERIMENTO SE NON E' STATO FLAGGATO IL PREMIUM
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
                string queryE = "UPDATE eventi " +
                                 "SET Nposti = Nposti - " + numBig +
                                 " WHERE ID = '" + LoginInfo.IdEvento + "'"; //AGGIORNO IL  NUMERO DI BIGLIETTI DISPONIBILI PER L'EVENTO SCELTO

                MySqlCommand commandE = new MySqlCommand(queryE, con.cn);
                commandE.ExecuteNonQuery();
                con.cn.Close();

                MessageBox.Show("Acquisto riusciuto con successo, costo totale: €" + costo.ToString());
            }
            else
            {
                MessageBox.Show("Non ci sono abbastanza biglietti disponibili");
            }
        }

        public void impostazioniCEO(string nome, string cognome, string email, int IDUser)
        {
            DataTable dt = new DataTable();
            
            con.cn.Open();
            
            try
            {
                string query = "UPDATE ceo_organizzazioni " +
                               "SET Nome = '" + nome + "', Cognome = '" + cognome + "', Email = '" + email + "' " + //MODIFICA SU DATABASE
                               "WHERE ID = '" + IDUser + "'";

                MySqlDataAdapter sda = new MySqlDataAdapter(query, con.cn);
                sda.Fill(dt);

                MessageBox.Show("I tuoi dati sono stati modificati correttamente.");
            }
            catch (Exception)
            {
                throw new Exception();
            }
            finally
            {
                con.cn.Close();
            }
        }

        public void impostazioniUtente(string nome, string cognome, string username, string email, int IDUser)
        {
            DataTable dt = new DataTable();

            con.cn.Open();

            try
            {
                string query = "UPDATE utenti " +
                               "SET Nome = '" + nome + "', Cognome = '" + cognome + "', Username = '" + username + "', Email = '" + email + "' " +
                               "WHERE ID = '" + IDUser + "'";

                MySqlDataAdapter sda = new MySqlDataAdapter(query, con.cn);
                sda.Fill(dt);

                MessageBox.Show("I tuoi dati sono stati modificati correttamente.");
            }
            catch (Exception)
            {
                throw new Exception();
            }
            finally
            {
                con.cn.Close();
            }
        }

        public void infoEventi(DataTable dt, int IDEvento)
        {
            MySqlCommand command;
            MySqlDataAdapter da;

            con.cn.Open();
            try
            {
                string query = "SELECT u.Nome, u.Cognome, u.Username, u.Email " +
                             "FROM utenti u, biglietti b, utenti_biglietti ub " +    //CERCO LE INFORMAZIONI DEGLI UTENTI CHE CHE HANNO ACQUISTATO DEI  BIGLIETTI PER UN CERTO EVENTO SELEZIONATO
                             "WHERE b.ID=ub.CODBiglietto AND ub.CODUtente=u.ID " +   //TRAMITE L'ID DELL'EVENTO
                             "AND b.CODEvento = '" + IDEvento + "'";

                command = new MySqlCommand(query, con.cn);
                da = new MySqlDataAdapter(command);
                da.Fill(dt);

            }
            catch (Exception)
            {
                throw new Exception();
            }
            finally
            {
                con.cn.Close();
            }
        }

        public void login(string email, string password, bool organizzatore)
        {
            DataTable dt = new DataTable();
            DataTable id = new DataTable();
            DataTable dataTable = new DataTable();

            con.cn.Open();

            try
            {
                con.cn.Open();

                if (organizzatore)             //LOGIN CEO
                {
                    string queryCEO = "SELECT * FROM ceo_organizzazioni WHERE Email = '" + email + "' AND Password = MD5('" + password + "')";    //VERIFICA CHE I DATI INSERITI SIANO PRESENTI NEL DATABASE
                    MySqlDataAdapter da = new MySqlDataAdapter(queryCEO, con.cn);

                    da.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        string qId = "SELECT ID from ceo_organizzazioni WHERE Email = '" + email + "'";//QUERY PER TROVARE L'ID DELL'UTENTE
                        MySqlDataAdapter da1 = new MySqlDataAdapter(qId, con.cn);                  //CON LE VARIABILI DI SQL UTILI PER CIO'
                        da1.Fill(id);
                        LoginInfo.UserID = Int32.Parse(id.Rows[0]["ID"].ToString());            //SE I DATI INSERITI SONO CORRETTI ALLORA L'UTENTE VIENE LOGGATO E REINDIRIZZATO NELLA HOME DEL PROGRAMMA
                        FormInterfacciaCEO home = new FormInterfacciaCEO();
                        home.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Dati inseriti errati", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);       //SE I DATI INSERITI SONO ERRATI ESCE QUESTO MESSAGGIO DI ERRORE E I DATI VENGONO CANCELLATI 
                    }
                }
                else                                         //LOGIN UTENTI NORMALI
                {
                    string queryUtenti = "SELECT * FROM utenti WHERE Email = '" + email + "' AND Password = MD5('" + password + "')";
                    MySqlDataAdapter sda = new MySqlDataAdapter(queryUtenti, con.cn);

                    sda.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        string qId = "SELECT ID from utenti WHERE Email = '" + email + "'";//QUERY PER TROVARE L'ID DELL'UTENTE
                        MySqlDataAdapter da = new MySqlDataAdapter(qId, con.cn);                  //CON LE VARIABILI DI SQL UTILI PER CIO'
                        da.Fill(id);
                        LoginInfo.UserID = Int32.Parse(id.Rows[0]["ID"].ToString());
                        FormInterfacciaUtente home = new FormInterfacciaUtente();
                        home.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Dati inseriti errati", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
            finally
            {
                con.cn.Close();
            }
        }

        public void profiloCEO(int IDUser, string email, string nome, string cognome)
        {
            DataTable dt = new DataTable();

            con.cn.Open();

            try
            {
                string query = "SELECT * FROM ceo_organizzazioni WHERE ID = '" + LoginInfo.UserID + "'";

                MySqlDataAdapter sda = new MySqlDataAdapter(query, con.cn);
                sda.Fill(dt);
            }
            catch (Exception)
            {
                throw new Exception();
            }
            finally
            {
                con.cn.Close();
            }

            email = dt.Rows[0]["Email"].ToString();
            nome = dt.Rows[0]["Nome"].ToString();
            cognome = dt.Rows[0]["Cognome"].ToString();
        }

        public void profiloUtente(int IDUser, string email, string nome, string cognome, string username)
        {
            DataTable dt = new DataTable();

            con.cn.Open();
            try
            {
                string query = "SELECT * FROM utenti WHERE ID = '" + LoginInfo.UserID + "'"; //TRAMITE L'ID SALVATO UNA VOLTA EFFETTUATO IL LOGIN MOSTRO
                                                                                             //LE INFORMAZIONI DELL'UTENTE LOGGATO

                MySqlDataAdapter sda = new MySqlDataAdapter(query, con.cn);
                sda.Fill(dt);
            }
            catch (Exception)
            {
                throw new Exception();
            }
            finally
            {
                con.cn.Close();
            }

            username = dt.Rows[0]["Username"].ToString();
            email = dt.Rows[0]["Email"].ToString();
            nome = dt.Rows[0]["Nome"].ToString();
            cognome = dt.Rows[0]["Cognome"].ToString();
        }

        public void registrazione(string nome, string cognome, string email, string password, string username, bool organizzatore, string nomeOrganizzazione)
        {
            int i = 0;

            MySqlDataAdapter da;
            DataTable dt = new DataTable();

            if (organizzatore)          //REGISTRAZIONE ORGANIZZATORE
            {
                //Controllo che  tutti i campi siano compilati
                if ((String.IsNullOrEmpty(email)) || (String.IsNullOrEmpty(password)) || (String.IsNullOrEmpty(nome)) || (String.IsNullOrEmpty(cognome)))
                {
                    MessageBox.Show("E' necessario compilare tutti i campi");
                }
                else
                {
                    try
                    {
                        DataTable DataTableID = new DataTable();
                        string queryID = "SELECT o.ID " +
                                         "FROM organizzazione o " +
                                         "WHERE o.Nome = '" + nomeOrganizzazione + "'";            //ESTRAE L'ID DELL'ORGANIZZAZIONE
                        da = new MySqlDataAdapter(queryID, con.cn);
                        da.Fill(DataTableID);
                        DataRow[] righe = DataTableID.Select();
                        string idSelect = righe[0]["ID"].ToString();
                        int ID = Int32.Parse(idSelect);

                        string query = $"INSERT INTO `ceo_organizzazioni` (`ID`, `Nome`, `Cognome`, `Email`, `Password`, `CODOrganizzazione`) VALUES ('', '" + nome + "', '" + cognome + "', '" + email + "', MD5('" + password + "'), '" + ID + "');";     //INSERIAMO NEL DATABASE I DATI INSERITI NELLE TEXTBOX E SELEZIONATI NELLA COMBOBOX
                        MySqlCommand command = new MySqlCommand(query, con.cn);
                        command.ExecuteNonQuery();
                        Login loginReg = new Login();                       //SI VIENE PORTATI AL PORTALE DEL LOGIN
                        loginReg.Show();

                    }
                    catch (Exception)
                    {
                        throw new Exception();
                    }
                    finally
                    {
                        con.cn.Close();
                    }
                }
            }
            else                 //REGISTRAZIONE UTENTE NORMALE
            {
                //Controllo che  tutti i campi siano compilati
                if ((String.IsNullOrEmpty(email)) || (String.IsNullOrEmpty(password)) || (String.IsNullOrEmpty(nome)) || (String.IsNullOrEmpty(cognome)) || (String.IsNullOrEmpty(username)))
                {
                    MessageBox.Show("E' necessario compilare tutti i campi");
                }
                else
                {
                    try
                    {
                        string query = $"INSERT INTO `utenti` (`ID`, `Nome`, `Cognome`, `Username`, `Email`, `Password`) VALUES ('', '" + nome + "', '" + cognome + "', '" + username + "', '" + email + "', MD5('" + password + "'));";           //INSERIMENTO DATI
                        MySqlCommand command = new MySqlCommand(query, con.cn);
                        command.ExecuteNonQuery();
                        Login loginReg = new Login();
                        loginReg.Show();
                    }
                    catch (Exception)
                    {
                        throw new Exception();
                    }
                    finally
                    {
                        con.cn.Close();
                    }
                }
            }
        }

        public void ricercaCEO(string nomeEvento, DataTable dt)
        {
            MySqlCommand command;
            MySqlDataAdapter da;

            dt.Rows.Clear();

            string query = "SELECT e.* " +
                           "FROM eventi e, organizzazione o, ceo_organizzazioni c " +
                           "WHERE c.CODOrganizzazione=o.ID AND o.ID=e.CODOrganizzazione " + //CERCA EVENTO UGUALE A QUELLO PER UTENTE
                           "AND e.Nome = '" + nomeEvento + "' " +
                           "AND c.ID != '" + LoginInfo.UserID + "'";
            command = new MySqlCommand(query, con.cn);
            da = new MySqlDataAdapter(command);
            da.Fill(dt);
        }

        public void ricercaUtente(string nomeEvento, DataTable dt)
        {
            MySqlCommand command;
            MySqlDataAdapter da;

            dt.Rows.Clear();

            string query = "SELECT e.* " +
                           "FROM eventi e, organizzazione o, ceo_organizzazioni c " +         //RICERCA DI UN EVENTO TRAMITE NOME SCRITTO NELLA TEXTBOX
                           "WHERE c.CODOrganizzazione=o.ID AND o.ID=e.CODOrganizzazione " +
                           "AND e.Nome = '" + nomeEvento + "'";
            command = new MySqlCommand(query, con.cn);
            da = new MySqlDataAdapter(command);
            da.Fill(dt);
        }
    }
}
