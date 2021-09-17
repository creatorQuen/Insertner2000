// See https://aka.ms/new-console-template for more information

using System.Collections.Generic;
using Insertner2000.ZLooLibs;

FIOs.GetFIOsFemale(out IReadOnlyDictionary<string, IReadOnlyList<string>> females);
FIOs.GetFIOsMale(out IReadOnlyDictionary<string, IReadOnlyList<string>> males);
Cities.GetCities(out IReadOnlyList<string> cities);
Types.GetTypes(out IReadOnlyList<string> currenciesType, out IReadOnlyList<string> transactionsType);
Emails.GetEmails(out IReadOnlyList<string> emailsList);
var ttt = "";