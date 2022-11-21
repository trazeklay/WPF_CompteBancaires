using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using BusinessLayer;
using DataLayer;
using System.Data;
using System.ComponentModel;

namespace WpfVirement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged 
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private ServiceCompte sc = new ServiceCompte();
        public ServiceCompte SC
        {
            get { return sc; }
            set { sc = value; }
        }

        private ObservableCollection<Compte> lesComptes;
        public ObservableCollection<Compte> LesComptes
        {
            get { return lesComptes; }
            set { lesComptes = value; }
        }

        private double montant;
        public double Montant
        {
            get { return montant; }
            set { 
                montant = value;
                OnPropertyChanged(nameof(Montant));
            }
        }

        private Compte? compteDebit;
        public Compte? CompteDebit
        {
            get { return compteDebit; }
            set { 
                compteDebit = value;
                OnPropertyChanged(nameof(CompteDebit));
            }
        }

        private Compte? compteCredit;
        public Compte? CompteCredit
        {
            get { return compteCredit; }
            set { 
                compteCredit = value;
                OnPropertyChanged(nameof(CompteCredit));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.LesComptes = sc.GetAllComptes();
            this.DataContext = this;
        }

        private void btnValider_Click(object sender, RoutedEventArgs e)
        {
            DataAccess access = new DataAccess();
            try
            {
                if (CompteDebit is null)
                    throw new Exception("Veuillez sélectionner un compte à débiter");
                if (CompteCredit is null)
                    throw new Exception("Veuillez sélectionner un compte à créditer");
                if (CompteDebit == CompteCredit)
                    throw new Exception("Veuillez sélectionner deux comptes différents");
                if (Montant <= 0)
                    throw new Exception("Veuillez entrer un montant positif");
                if(sc.GetComptesBySelection($"SELECT * FROM vComptes WHERE idCompte = {CompteDebit.IdCompte} AND solde >= {Montant}").Count != 1)
                    throw new Exception($"Compte {((Compte)this.cbxCompteDebit.SelectedItem).IdCompte} : Solde insuffisant");
                if (!access.VirementBancaire(CompteDebit.IdCompte, CompteCredit.IdCompte, Montant))
                    throw new Exception("Erreur lors du virement");
                
                MessageBox.Show($"Virement entre {CompteDebit.IdCompte} et {CompteCredit.IdCompte} de {Montant}€ effectué");
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            CompteDebit = null;
            CompteCredit = null;
            Montant = 0;
        }
    }
}
