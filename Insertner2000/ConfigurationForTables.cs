﻿using Insertner2000.DateBases.CRM;
using Insertner2000.Tables;

namespace Insertner2000
{
    public class ConfigurationForTables
    {
        public const string _conStringCRM = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LeadA; Persist Security Info=False;";
        public const string _conStringTStore = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TStore; Persist Security Info=False;";
        public const int _countAccounts = 500;

        public void LeadsDataBase()
        {
            Lead listLeads = new Lead();
            listLeads.CreateLeads(1, _countAccounts, _conStringCRM);
        }

        public void CitiesDataBase()
        {
            City listCities = new City();
            listCities.CreateCities(1, _conStringCRM);
        }

        public void AccountsDataBase()
        {
            Account listAccounts = new Account();
            listAccounts.CreateAcounts(1, _countAccounts, _conStringCRM, _conStringTStore);
        }
    }
}
