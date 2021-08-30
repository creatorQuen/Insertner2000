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
        private static string _transactionTable = "[TransactionStore.Db].[dbo].[Transaction]";
        private const string _dateFormat = "dd.MM.yyyy HH:mm:ss.fffffff";
        private const int _countEnd = 10;
        private const int _dayPearHalfYear = 180;
        private const int _dayPearTwoWeek = 14;
        private readonly Random _random = new Random();

        public void CreateTStores(Dictionary<int, CurrencyType> dictionary, string connectionForTransaction)
        {
            using (SqlConnection _connection = new SqlConnection(connectionForTransaction))
            {
                var dataSet = new DataSet();
                var table = dataSet.Tables.Add("MockTransaction");

                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("AccountId", typeof(int));
                table.Columns.Add("Amount", typeof(decimal));
                table.Columns.Add("Currency", typeof(int));
                table.Columns.Add("TransactionType", typeof(int));
                table.Columns.Add("Date", typeof(DateTime));

                var amount = 0;
                var dictionaryCurrencyAmount = new Dictionary<CurrencyType, int>();
                foreach (var d in dictionary)
                {
                    dictionaryCurrencyAmount.Add(d.Value, 0);
                }

                for (var intRow = 0; intRow <= _countEnd; intRow++)
                {
                    var currency = GetCurrencyByDictionaryKeys(dictionary);//todo
                    var transactionType = GetTransactionTypeByAmount(dictionaryCurrencyAmount, currency);
                    var dateTimeTransaction = DateTime.Now.AddDays(_random.Next(-_dayPearHalfYear ,- _dayPearTwoWeek)).ToString(_dateFormat);
                    var key = dictionary.FirstOrDefault(x => x.Value == currency).Key;
                    amount = GetRandomAmountByTransactionType(transactionType, dictionaryCurrencyAmount, currency);

                    if (TransactionType.Transfer == transactionType)
                    {
                        var dictionaryClone = new Dictionary<int, CurrencyType>();
                        foreach (var dic in dictionary)
                        {
                            dictionaryClone.Add(dic.Key, dic.Value);
                        }

                        dictionaryClone.Remove(key);

                        var index = dictionaryClone.ElementAt(_random.Next(dictionaryClone.Count)).Key;
                        var payee = dictionaryClone.FirstOrDefault(x => x.Key == index).Value;

                        table.Rows.Add(
                            intRow,
                            index,//dictionary.key
                            amount,
                            payee,//dictionary.value
                            transactionType,
                            dateTimeTransaction
                            );
                        dictionaryCurrencyAmount[payee] += amount;

                        table.Rows.Add(
                            intRow,
                            key,//dictionary.key
                            -amount,
                            dictionary[key],//dictionary.value
                            transactionType,
                            dateTimeTransaction
                        );
                        dictionaryCurrencyAmount[dictionary[key]] -= amount;

                        amount = _random.Next(-50, 100);
                        continue;
                    }

                    table.Rows.Add(
                        intRow,
                        key,//dictionary.key
                        amount,
                        dictionary[key],//dictionary.value
                        transactionType,
                        dateTimeTransaction
                        );
                    dictionaryCurrencyAmount[dictionary[key]] += amount;

                    amount = _random.Next(-50, 100);
                }

                var bulkCopy = new SqlBulkCopy(_connection);
                _connection.Open();
                bulkCopy.DestinationTableName = _transactionTable;
                bulkCopy.BulkCopyTimeout = 0;
                bulkCopy.WriteToServer(table);
            }
        }

        private TransactionType GetTransactionTypeByAmount(Dictionary<CurrencyType, int> dictionary, CurrencyType currency)
        {
            var amount = dictionary[currency];

            if (1 < dictionary.Count)
            {
                if (0 < amount)
                {
                    return (TransactionType)_random.Next(1, 4);
                }
                return TransactionType.Deposit;
            }
            if (0 < amount)
            {
                return (TransactionType)_random.Next(1, 3);
            }
            return TransactionType.Deposit;
        }

        private CurrencyType GetCurrencyByDictionaryKeys(Dictionary<int, CurrencyType> dictionary)
        {
            var listId = dictionary.Keys.ToList();
            var index = _random.Next(listId[0], listId[listId.Count - 1]);
            var currency = dictionary.FirstOrDefault(x => x.Key == index).Value;
            return currency;
        }

        private int GetRandomAmountByTransactionType(TransactionType type, Dictionary<CurrencyType, int> dictionary, CurrencyType currency)
        {
            switch (type)
            {
                case TransactionType.Deposit: return _random.Next(100, 10000);
                case TransactionType.Withdraw: return _random.Next(-dictionary[currency], -1);
                case TransactionType.Transfer: return _random.Next(1, dictionary[currency]);
                default: throw new Exception("this type doesn't have in TransactionType");
            }
        }
    }
}
