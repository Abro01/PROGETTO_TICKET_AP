using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ProgettoRDF
{
    // NOTA: è possibile utilizzare il comando "Rinomina" del menu "Refactoring" per modificare il nome di interfaccia "IDBService" nel codice e nel file di configurazione contemporaneamente.
    [ServiceContract]
    public interface IDBService
    {
        [OperationContract]
        void registrazione(string nome, string cognome, string email, string password, string username, bool organizzatore, string nomeOrganizzazione);

        [OperationContract]
        void login(string email, string password, bool organizzatore);

        [OperationContract]
        void acquistaBigliettoUtente(string app, int IDEvento, int IDUser, bool premium, int IDBiglietto, int costo, int prezzo);

        [OperationContract]
        void acquistaBigliettoCEO(string app, int IDEvento, int IDUser, bool premium, int IDBiglietto, int costo, int prezzo);

        [OperationContract]
        void impostazioniUtente(string nome, string cognome, string username, string email, int IDUser);

        [OperationContract]
        void impostazioniCEO(string nome, string cognome, string email, int IDUser);

        [OperationContract]
        void ricercaUtente(string nomeEvento, DataTable dt);

        [OperationContract]
        void ricercaCEO(string nomeEvento, DataTable dt);

        [OperationContract]
        void profiloUtente(int IDUser, string email, string nome, string cognome, string username);

        [OperationContract]
        void profiloCEO(int IDUser, string email, string nome, string cognome);

        [OperationContract]
        void infoEventi(DataTable dt, int IDEvento);
    }
}
