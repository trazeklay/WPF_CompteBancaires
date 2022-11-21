using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace BusinessLayer.Tests
{
    [TestClass()]
    public class ServiceCompteTests
    {
        ServiceCompte sc;
        ObservableCollection<Compte> comptes;

        [TestInitialize]
        public void InitialisationDesTests()
        {
            sc = new ServiceCompte();
            comptes = sc.GetAllComptes();
        }

        [TestMethod()]
        public void TestGetAllComptes_Compte1()
        {
            Compte c = new Compte(1234567, 1000);
            Assert.AreEqual(c, comptes[0]);
        }

        [TestMethod()]
        public void TestGetAllComptes_Compte2()
        {
            Compte c = new Compte(2345678, 2000);
            Assert.AreEqual(c, comptes[1]);
        }
    }
}