﻿using System.Collections.Generic;

namespace Insertner2000.ZLooLibs
{
    public partial struct Types
    {
        private static readonly List<string> CurrenciesType = GetCurrenciesType();
        private static readonly List<string> TransactionsType = GetTransactionsType();

        public static void GetTypes(out IReadOnlyList<string> currenciesType, out IReadOnlyList<string> transactionsType)
        {
            currenciesType = CurrenciesType;
            transactionsType = TransactionsType;
        }
    }
}