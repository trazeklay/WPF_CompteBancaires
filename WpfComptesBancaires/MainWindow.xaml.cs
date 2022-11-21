using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BusinessLayer;

namespace WpfComptesBancaires
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

        private ObservableCollection<string> lesTypesOp;
        public ObservableCollection<string> LesTypesOp
        {
            get { return lesTypesOp; }
            set { lesTypesOp = value; }
        }

        private double montant;
        public double Montant
        {
            get { return montant; }
            set { montant = value; }
        }


        public MainWindow()
        {

            InitializeComponent();
            this.LesComptes = sc.GetAllComptes();
            this.LesTypesOp = new ObservableCollection<string>() { "Retrait", "Dépôt" };
            this.DataContext = this;
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            this.cbxTOP.SelectedIndex = -1;
            this.cbxCompte.SelectedIndex = -1;
            this.Montant = 0;
        }

        private void btnValider_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.cbxTOP.SelectedIndex == -1) 
                    throw new Exception("Veuillez sélectionner un type d'opération");
                if (this.cbxCompte.SelectedIndex == -1) 
                    throw new Exception("Veuillez sélectionner un compte");

                if ((string)cbxTOP.Text == "Retrait")
                    this.Montant *= -1;

                if (!this.SC.SetDebitCredit((Compte)this.cbxCompte.SelectedItem, this.Montant))
                    throw new Exception("Erreur dans la transaction");

                MessageBox.Show("Transaction effectuée", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                this.cbxTOP.SelectedIndex = -1;
                this.cbxCompte.SelectedIndex = -1;
                this.Montant = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
