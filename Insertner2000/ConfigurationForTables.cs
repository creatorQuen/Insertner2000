﻿using Insertner2000.DateBases.CRM;
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

        //public const string LeadTable = "[LeadA].[dbo].[Lead]";
        //public const string AccountTable = "[LeadA].[dbo].[Account]";
        //public const string TransactionTable = "[TStore].[dbo].[Transaction]";

        private const string _conStringCrm = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CRM.DB; Persist Security Info=False;";
        private const string _conStringTStore = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TransactionStore.DB; Persist Security Info=False;";
        //private const string _conStringTStore = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TransactionStore; Persist Security Info=False;";
        //private const string _conStringCrm = @"Data Source=80.78.240.16;Initial Catalog = CRM.Db; Persist Security Info=True;User ID = student;Password=qwe!23;";

        //private const string _conStringCrm = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LeadA; Persist Security Info=False;";
        //private const string _conStringTStore = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TStore; Persist Security Info=False;";

        private const int _startCountRows = 1;
        private const int _endCountRows = 100;

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