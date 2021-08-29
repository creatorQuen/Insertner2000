using Insertner2000.DateBases.CRM;
using Insertner2000.DateBases.TStore;
using Insertner2000.Tables;

namespace Insertner2000
{
    public class ConfigurationForTables
    {
        private const string _conStringCrm = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LeadA; Persist Security Info=False;";
        private const string _conStringTStore = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TStore; Persist Security Info=False;";

        public void LeadsDataBase()
        {
            Lead listLeads = new Lead();
            listLeads.CreateLeads(1, 1000000, _conStringCrm);
        }

        public void CitiesDataBase()
        {
            City listCities = new City();
            listCities.CreateCities(1, _conStringCrm);
        }

        public void AccountsDataBase()
        {
            Account listAccounts = new Account();
            TStore listTransactions = new TStore();

            //listAccounts.CreateAcounts(1, 100, 1, _conStringCRM);
            //listAccounts.CreateAcounts(101, 150, 2, _conStringCRM);
            //listAccounts.CreateAcounts(151, 200, 3, _conStringCRM);
            //listAccounts.CreateAcounts(201, 300, 4, _conStringCRM);
            listAccounts.CreateAccounts(1, 500, _conStringCrm);
            for (int i = 0; i < 500; i++)
            {
                listTransactions.CreateTStores(i, _conStringTStore);
            }
        }

        public void TransactionsDataBase()
        {
            //listTransactions.CreateTStores(1, 100, _conStringTStore);
        }
    }
}
