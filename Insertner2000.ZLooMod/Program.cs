// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Linq;
using Insertner2000.ZLooLibs;
using Insertner2000.ZLooMod;



Lead lead = new();
lead.CreateTable();


Cities.GetCities(out IReadOnlyList<string> cities);
Types.GetTypes(out IReadOnlyList<string> currenciesType, out IReadOnlyList<string> transactionsType);
Emails.GetEmails(out IReadOnlyList<string> emailsList);
var ttt = "";