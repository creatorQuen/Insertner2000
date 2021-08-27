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
            listAccounts.CreateAcounts(1, 1000000, _conStringCRM, _conStringTStore);
        }
    }
}
