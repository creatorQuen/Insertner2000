using Insertner2000.DateBases.CRM;
using Insertner2000.Tables; 

namespace Insertner2000
{
    public class ConfigurationForTables
    {
        public const string CityTable = "[CRM].[dbo].[City]";
        public const string LeadTable = "[CRM].[dbo].[Lead]";
        public const string AccountTable = "[CRM].[dbo].[Account]";
        public const string TransactionTable = "[TransactionStore].[dbo].[Transaction]";
        public const int TransactionCountByAccount = 10;

        private const string _conStringCrm = @"Server=localhost;Database=CRM;Trusted_Connection=True;";
        //private const string _conStringTStore = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TransactionStore; Persist Security Info=False;";
        //private const string _conStringCrm = @"Data Source=80.78.240.16;Initial Catalog = CRM.Db; Persist Security Info=True;User ID = student;Password=qwe!23;";
        //private const string _conStringTStore = @"Data Source=80.78.240.16;Initial Catalog = TransactionStore.Db; Persist Security Info=True;User ID = student;Password=qwe!23;";

        private const int _startCountRows = 1;
        private const int _endCountRows = 10000;

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

        //public void AccountsDataBase()
        //{
        //    var listAccounts = new Account();
        //    listAccounts.CreateAccounts(_startCountRows, _endCountRows, _conStringCrm, _conStringTStore);
        //}
    }
}