using Insertner2000.DateBases.CRM;
using Insertner2000.Tables; 

namespace Insertner2000
{
    public class ConfigurationForTables
    {
        public const string CityTable = "[CRM.DB].[dbo].[City]";
        public const string LeadTable = "[CRM.DB].[dbo].[Lead]";
        public const string AccountTable = "[CRM.DB].[dbo].[Account]";
        public const string TransactionTable = "[TransactionStore.DB].[dbo].[Transaction]";
        public const int TransactionCountByAccount = 10;

        //private const string _conStringCrm = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CRM; Persist Security Info=False;";
        private const string _conStringCrm = @"Data Source=151.248.118.251;Initial Catalog = CRM.Db; Persist Security Info=True;User ID = student;Password=qwe!23;";
        private const string _conStringTStore = @"Data Source=151.248.118.251;Initial Catalog = TransactionStore.DB; Persist Security Info=True;User ID = student;Password=qwe!23;";
        //private const string _conStringTStore = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TransactionStore; Persist Security Info=False;";
        //private const string _conStringCrm = @"Data Source=80.78.240.16;Initial Catalog = CRM.Db; Persist Security Info=True;User ID = student;Password=qwe!23;";
        //private const string _conStringTStore = @"Data Source=80.78.240.16;Initial Catalog = TransactionStore.Db; Persist Security Info=True;User ID = student;Password=qwe!23;";

        private const int _startCountRows = 600001;
        private const int _endCountRows = 3600000;

        public void LeadsDataBase()
        {
            var listLeads = new Lead();
            listLeads.CreateLeads(_startCountRows, _endCountRows, _conStringCrm);
        }

        public void CitiesDataBase()
        {
            var listCities = new City();
            listCities.CreateCities(_startCountRows, _conStringCrm);
        }

        public void AccountsDataBase()
        {
            var listAccounts = new Account();
            listAccounts.CreateAccounts(_startCountRows, _endCountRows, _conStringCrm, _conStringTStore);
        }
    }
}