using System.Collections.Generic;

namespace Insertner2000.ZLooLibs
{
    public partial struct Types
    {
        private const string Rub = "RUB";
        private const string Usd = "USD";
        private const string Eur = "EUR";
        private const string Jpy = "JPY";

        private static List<string> GetCurrenciesType() =>
        new()
        {
            Rub,
            Usd,
            Eur,
            Jpy
        };
    }
}