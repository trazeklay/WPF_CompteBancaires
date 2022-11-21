using Npgsql;
using System;
using System.Data;


namespace DataLayer
{
    public class DataAccess // A MODIFIER SI VOTRE PROJET A UN AUTRE NOM
    {
        public NpgsqlConnection? NpgSQLConnect { get; set; }

        /// <summary>
        /// Connexion à la base de données
        /// </summary>
        /// <returns> Retourne un booléen indiquant si la connexion est ouverte ou fermée</returns>
        private bool OpenConnection()
        {
            try
            {
                NpgSQLConnect = new NpgsqlConnection
                {
                    ConnectionString = "Server=localhost;port=5432;Database=BdComptesBancaires;uid=postgres;password=postgres;" // A MODIFIER SI VOTRE BD A UN AUTRE NOM
                };
                NpgSQLConnect.Open();
                return NpgSQLConnect.State.Equals(System.Data.ConnectionState.Open);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Déconnexion de la base de données
        /// </summary>
        private void CloseConnection()
        {
            if (NpgSQLConnect != null)
                if (NpgSQLConnect.State.Equals(System.Data.ConnectionState.Open))
                {
                    NpgSQLConnect.Close();
                }
        }

        /// <summary>
        /// Accès à des données en lecture
        /// </summary>
        /// <param name="getQuery">Requête SELECT de sélection de données</param>
        /// <returns>Retourne un DataTable contenant les lignes renvoyées par le SELECT</returns>
        public DataTable? GetData(string getQuery)
        {
            try
            {
                if (OpenConnection())
                {
                    NpgsqlCommand npgsqlCommand = new NpgsqlCommand(getQuery, NpgSQLConnect);
                    NpgsqlDataAdapter npgsqlAdapter = new NpgsqlDataAdapter
                    {
                        SelectCommand = npgsqlCommand
                    };
                    DataTable dataTable = new DataTable();
                    npgsqlAdapter.Fill(dataTable);
                    CloseConnection();
                    return dataTable;
                }
                else
                    return null;
            }
            catch
            {
                CloseConnection();
                return null;
            }
        }

        /// <summary>
        /// Permet d'insérer, supprimer ou modifier des données
        /// </summary>
        /// <param name="setQuery">Requête SQL permettant d'insérer (INSERT), supprimer (DELETE) ou modifier des données (UPDATE)</param>
        /// <returns>Retourne un entier contenant le nombre de lignes ajoutées/supprimées/modifiées</returns>
        public int SetData(string setQuery)
        {
            try
            {
                if (OpenConnection())
                {
                    NpgsqlCommand sqlCommand = new NpgsqlCommand(setQuery, NpgSQLConnect);
                    int modifiedLines = sqlCommand.ExecuteNonQuery();
                    CloseConnection();
                    return modifiedLines;
                }
                else
                    return 0;
            }
            catch
            {
                CloseConnection();
                return 0;
            }
        }

        public bool VirementBancaire(int idCompteDebit, int idCompteCredit, double montant)
        {
            try
            {
                bool res = false;
                if (OpenConnection())
                {
                    /*Création d'une commande de type procédure stockée*/
                    NpgsqlCommand npgsqlCommand = new NpgsqlCommand("sp_virement_append", NpgSQLConnect)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    /**/

                    /*Création des paramètres de la procédure stockée*/
                    NpgsqlParameter paramIdCompteDebit = npgsqlCommand.Parameters.Add("pIdCompteDebit", NpgsqlTypes.NpgsqlDbType.Integer);
                    paramIdCompteDebit.Direction = ParameterDirection.Input;

                    NpgsqlParameter paramIdCompteCredit = npgsqlCommand.Parameters.Add("pIdCompteCredit", NpgsqlTypes.NpgsqlDbType.Integer);
                    paramIdCompteCredit.Direction = ParameterDirection.Input;   

                    NpgsqlParameter paramMontant = npgsqlCommand.Parameters.Add("pMontant", NpgsqlTypes.NpgsqlDbType.Numeric);
                    paramIdCompteCredit.Direction = ParameterDirection.Input;
                    /**/

                    /*Initialisation de la valeur des paramètres aux arguments de la méthode*/
                    paramIdCompteDebit.Value = idCompteDebit;
                    paramIdCompteCredit.Value = idCompteCredit;
                    paramMontant.Value = montant;
                    /**/

                    //Execution de la fonction stockée et récupération du résultat
                    var reader = npgsqlCommand.ExecuteReader();

                    //Récupération de la valeur de retour. Normalement 3(2 Update + 1 Insert)
                    reader.Read();

                    if ((int?)reader[0] == 3)
                        res = true;
                    CloseConnection();
                    return res;
                } else return false;
            }
            catch (Exception)
            {
                CloseConnection();
                return false;
            }
        }
    }
}
