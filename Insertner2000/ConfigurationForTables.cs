using Insertner2000.DateBases.CRM;
using Insertner2000.DateBases.TStore;
using Insertner2000.Tables;
using System.Data.SqlClient;

namespace Insertner2000
{
    public class ConfigurationForTables
    {
        //public const string _conStringCRM = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LeadA; Persist Security Info=False;";
        public const string _conStringCRM = @"Data Source=80.78.240.16;Initial Catalog = CRM.Db; Persist Security Info=True;User ID = student;Password=qwe!23;";
        public const string _conStringTStore = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TStore; Persist Security Info=False;";
        public const int _countRows = 1000000;

        public void LeadsDataBase()
        {
            Lead listLeads = new Lead();
            listLeads.CreateLeads(1, _countRows, _conStringCRM);
        }

        public void CitiesDataBase()
        {
            City listCities = new City();
            listCities.CreateCities(1, _conStringCRM);
        }

        public void AccountsDataBase()
        {
            Account listAccounts = new Account();
            listAccounts.CreateAcounts(1, _countRows, _conStringCRM, _conStringTStore);
        }
    }
}
