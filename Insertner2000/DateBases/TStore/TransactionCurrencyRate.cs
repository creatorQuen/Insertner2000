using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insertner2000.DateBases.TStore
{
    public class TransactionCurrencyRate
    {
        public string BaseCurrency { get; } = "RUB";
        public Dictionary<string, decimal> CurrencyPair { get; set; }

        public TransactionCurrencyRate()
        {
            CurrencyPair = new Dictionary<string, decimal> { 
                { "USDRUB", 73.965m }, 
                { "EURRUB", 86.594m }, 
                { "JPYRUB", 0.68m },
                { "RUBUSD", 0},
                { "EURUSD", 0},
                { "JPYUSD", 0},
                { "RUBEUR", 0},
                { "USDEUR", 0},
                { "JPYEUR", 0},
            
            
            };
            
        }
    }
}
