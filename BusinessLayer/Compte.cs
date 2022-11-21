using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Compte
    {
        private int idCompte;
        public int IdCompte
        {
            get { return idCompte; }
            set { idCompte = value; }
        }

        private double solde;
        public double Solde
        {
            get { return solde; }
            set { solde = value; }
        }

        public Compte() { }
        public Compte(int idCompte, double solde)
        {
            IdCompte = idCompte;
            Solde = solde;
        }

        public override bool Equals(object? obj)
        {
            return obj is Compte compte &&
                   IdCompte == compte.IdCompte &&
                   Solde == compte.Solde;
        }

        public static bool operator ==(Compte? left, Compte? right)
        {
            return EqualityComparer<Compte>.Default.Equals(left, right);
        }

        public static bool operator !=(Compte? left, Compte? right)
        {
            return !(left == right);
        }
    }
}
