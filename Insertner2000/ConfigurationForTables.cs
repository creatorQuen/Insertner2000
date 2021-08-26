using Insertner2000.DateBases.CRM;
using Insertner2000.DateBases.TStore;
using Insertner2000.Tables;
using System.Data.SqlClient;

namespace Insertner2000
{
    public class ConfigurationForTables
    {
        public const string _conStringCRM = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LeadA; Persist Security Info=False;";
        public const string _conStringTStore = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TStore; Persist Security Info=False;";

        public void LeadsDataBase()
        {
            Lead listLeads = new Lead();
            listLeads.CreateLeads(1, 1000000, _conStringCRM);
        }

        public void CitiesDataBase()
        {
            City listCities = new City();
            listCities.CreateCities(1, _conStringCRM);
        }

        public void AccountsDataBase()
        {
            Account listAccounts = new Account();
            //listAccounts.CreateAcounts(1, 100, 1, _conStringCRM);
            //listAccounts.CreateAcounts(101, 150, 2, _conStringCRM);
            //listAccounts.CreateAcounts(151, 200, 3, _conStringCRM);
            //listAccounts.CreateAcounts(201, 300, 4, _conStringCRM);
            listAccounts.CreateAcounts(1, 500, _conStringCRM, _conStringTStore);
        }

        public void TransactionsDataBase()
        {
            TStore listTransactions = new TStore();
            //listTransactions.CreateTStores(1, 100, _conStringTStore);
            //listTransactions.CreateTStores(1, 100, _conStringTStore);
        }
    }
}
