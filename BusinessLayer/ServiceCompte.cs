using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataLayer;
using System.Windows;
using System.Collections.ObjectModel;
using System.Numerics;

namespace BusinessLayer
{
    public class ServiceCompte
    {
        public ObservableCollection<Compte> GetComptesBySelection(string query)
        {
            ObservableCollection<Compte> comptes = new ObservableCollection<Compte>();
            DataAccess access = new DataAccess();
            DataTable? dataTable = access.GetData(query);

            try
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    bool parseId = Int32.TryParse(row[0].ToString(), out int idCompte);
                    bool parseSolde = double.TryParse(row[1].ToString(), out double solde);
                    comptes.Add(new Compte(idCompte, solde));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Impossible de récupérer les données", "Service Comptes");
            }

            return comptes;
        }

        /// <summary>
        /// Récupère la liste de tous les comptes (id et solde) de la base de données
        /// </summary>
        /// <returns>Liste des comptes bancaires</returns>
        /// 
        public ObservableCollection<Compte> GetAllComptes()
        {
            return this.GetComptesBySelection("SELECT * FROM compte ORDER BY idCompte");
        }

        public bool SetDebitCredit(Compte compte, double montant)
        {
            DataAccess access = new DataAccess();
            if (access.SetData($"UPDATE Compte SET solde = solde + {montant} WHERE idCompte = {compte.IdCompte}") == 1)
                return true;
            return false;
        }

        /*public bool Virement(Compte c1, Compte c2, double montant)
        {
            DataAccess access = new DataAccess();

            string sql =
                $"BEGIN; " +
                $"UPDATE Compte SET solde = solde - {montant} where idcompte= {c1.IdCompte}; " +
                $"Update compte set solde = solde + {montant} where idcompte = {c2.IdCompte}; " +
                $"commit;";

            if (access.SetData(sql) != 2)
                return false;
            return true;
        }*/
    }
}
