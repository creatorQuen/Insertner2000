// See https://aka.ms/new-console-template for more information

using System.Collections.Generic;
using Insertner2000.ZLooLibs;
using Insertner2000.ZLooMod;


////City city = new City();
//city.CreateCities(0, ConfigurationForTables._conStringCrm);

Lead lead = new();
lead.CreateTableLeads();


Types.GetTypes(out IReadOnlyList<string> currenciesType, out IReadOnlyList<string> transactionsType);
Emails.GetEmails(out IReadOnlyList<string> emailsList);
var ttt = "";