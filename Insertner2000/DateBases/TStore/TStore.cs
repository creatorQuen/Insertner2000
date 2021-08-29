using Insertner2000.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Insertner2000.DateBases.TStore
{
    public class TStore
    {
        private static string _transactionTable = "[TStore].[dbo].[Transaction]";
        private const string _dateFormat = "dd.MM.yyyy HH:mm:ss.fffffff";
        private const int _countEnd = 10;
        private readonly Random _random = new Random();

        public void CreateTStores(Dictionary<int, CurrencyType> dictionary, string connectionForTransaction)
        {
            using (SqlConnection _connection = new SqlConnection(connectionForTransaction))
            {
                //Console.WriteLine("Starting..");

                var dataSet = new DataSet();
                var table = dataSet.Tables.Add("MockTransaction");

                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("AccountId", typeof(int));
                table.Columns.Add("Amount", typeof(decimal));
                table.Columns.Add("Currency", typeof(int));
                table.Columns.Add("TransactionType", typeof(int));
                table.Columns.Add("Date", typeof(DateTime));

                var ammount = 0;
                for (var intRow = 0; intRow <= _countEnd; intRow++)
                {

                    // var listId = Enumerable.ToList(dictionary.Keys);
                    var transactionType = GetTransactionTypeByAmount(ammount);
                    var currency = GetCurrencyByDictionaryKeys(dictionary);
                    var dateTimeTransaction = DateTime.Now.AddMilliseconds(ammount).ToString(_dateFormat);
                    var key = dictionary.FirstOrDefault(x => x.Value == currency).Key;
                    ammount = GetRandomAmountByTransactionType(transactionType);

                    table.Rows.Add(
                        intRow,
                        key,//dictionary.key
                        ammount,
                        dictionary[key],//dictionary.value
                        transactionType,
                        dateTimeTransaction
                        );
                    ammount = _random.Next(-50, 100);
                }

                var bulkCopy = new SqlBulkCopy(_connection);
                _connection.Open();
                bulkCopy.DestinationTableName = _transactionTable;
                bulkCopy.BulkCopyTimeout = 0;
                bulkCopy.WriteToServer(table);
            }
        }

        private TransactionType GetTransactionTypeByAmount(int amount)
        {
            //foreach (var dic in Dictionary.Value)
            //{
            //if( 1 < Dictionary.Count && 0 < dic)
            if (0 < amount)
            {
                return (TransactionType)_random.Next(2, 4);
            }
            //}

            return TransactionType.Deposit;
        }

        private CurrencyType GetCurrencyByDictionaryKeys(Dictionary<int,CurrencyType> dictionary)
        {
            //var listId = Enumerable.ToList(dictionary.Keys);
            //var index = _random.Next(listId.Count);
            //var currency = dictionary.FirstOrDefault(x => x.Key == index).Value;
            //return currency;

            //CurrencyType one = dictionary[0];
            //CurrencyType last = dictionary[dictionary.Count - 1];

            int i = 0;
            var dicClone = new Dictionary<int, CurrencyType>();
            foreach (var d in dictionary)
            {
                dicClone.Add(i++, d.Value);
            }
            var index = _random.Next(dicClone.Count);
            var currency = dicClone.FirstOrDefault(x => x.Key == index).Value;
            return currency;


        }
        // 3, RUB
        // 4, usd
        // 5, eur


        private int GetRandomAmountByTransactionType(TransactionType type)
        {
            switch (type)
            {
                case TransactionType.Deposit: return _random.Next(100, 10000);
                case TransactionType.Withdraw: return _random.Next(-100, -10);
                case TransactionType.Transfer: return _random.Next(100, 1000);
                default: throw new Exception("this type doesn't have in TransactionType");
            }
        }
    }
}
