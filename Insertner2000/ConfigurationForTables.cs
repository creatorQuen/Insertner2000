using Insertner2000.DateBases.CRM;
using Insertner2000.DateBases.TStore;
using Insertner2000.Tables;

namespace Insertner2000
{
    public class ConfigurationForTables
    {
        //private const string _conStringCrm = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LeadA; Persist Security Info=False;";
        //private const string _conStringTStore = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TStore; Persist Security Info=False;";
        private const string _conStringCrm = @"Data Source=80.78.240.16;Initial Catalog = CRM.Db; Persist Security Info=True;User ID = student;Password=qwe!23;";
        private const string _conStringTStore = @"Data Source=80.78.240.16;Initial Catalog = TransactionStore.Db; Persist Security Info=True;User ID = student;Password=qwe!23;";
        public const int _startCountRows = 1;
        public const int _endCountRows = 100;

        public void LeadsDataBase()
        {
            Lead listLeads = new Lead();
            listLeads.CreateLeads(_startCountRows, _endCountRows, _conStringCrm);
        }

        public void CitiesDataBase()
        {
            City listCities = new City();
            listCities.CreateCities(_startCountRows, _conStringCrm);
        }

        public void AccountsDataBase()
        {
            Account listAccounts = new Account();
            listAccounts.CreateAccounts(_startCountRows, _endCountRows, _conStringCrm,_conStringTStore);
        }
    }
}
