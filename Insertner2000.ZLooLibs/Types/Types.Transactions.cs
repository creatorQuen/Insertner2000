using System.Collections.Generic;

namespace Insertner2000.ZLooLibs
{
    public partial struct Types
    {
        private const string Deposit = "Deposit";
        private const string Withdraw = "Withdraw";
        private const string Transfer = "Transfer";

        private static List<string> GetTransactionsType() =>
        new()
        {
            Deposit,
            Withdraw,
            Transfer
        };
    }
}