using Insertner2000.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Insertner2000.DateBases.CRM
{
    public class Account
    {
        private string _accountTable = "[LeadA].[dbo].[Account]";
        private static string _transactionTable = "[TStore].[dbo].[Transaction]";
        private const string _dateFormat = "dd.MM.yyyy HH:mm:ss.fffffff";
        private int _globalcount = 1;
        private const int _transactionCount = 10;
        private const int _dayPearYear = 365;
        private const int _dayPearHalfYear = 180;
        private const int _dayPearTwoWeek = 14;
        private string _dateTimeTransaction;
        private readonly Random _random = new Random();

        public void CreateAccounts(int countStart, int countEnd, string connectionForLeadAccount, string connectionForTransaction)
        {
            using (SqlConnection _connection = new SqlConnection(connectionForLeadAccount))
            {
                Console.WriteLine("Starting..");
                var timeCreated = DateTime.Now;

                var dataSet = new DataSet();
                Console.WriteLine("Creating datatable..");
                var table = dataSet.Tables.Add("ttmpData");

                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("LeadId", typeof(int));
                table.Columns.Add("Currency", typeof(int));
                table.Columns.Add("CreatedOn", typeof(DateTime));
                table.Columns.Add("Closed", typeof(DateTime));
                table.Columns.Add("IsDeleted", typeof(bool));

                Console.WriteLine("Adding data to datatable..");

                for (var intRow = countStart; intRow <= countEnd;)
                {
                    var timeClosed = DateTime.Now.AddDays(_random.Next(-_dayPearTwoWeek, 0)).ToString(_dateFormat);
                    var isAccountDeleted = _random.Next(0, 2) == 1;

                    table.Rows.Add(
                        intRow,
                        intRow,
                        (int)CurrencyType.RUB,
                        timeCreated.AddDays(_random.Next(-_dayPearYear, -_dayPearHalfYear)).ToString(_dateFormat),
                        isAccountDeleted ? timeClosed : null,
                        isAccountDeleted
                        );

                    var dictionary = CreateMultipleCurrency(timeCreated, table, intRow);

                    foreach (var currency in dictionary.Keys.ToList())
                    {
                        AddRowsInTransactionForAccount(connectionForTransaction, intRow, currency, dictionary);
                        intRow++;
                    }
                }

                Console.WriteLine("Open database..");
                var bulkCopy = new SqlBulkCopy(_connection);
                _connection.Open();
                bulkCopy.DestinationTableName = _accountTable;
                bulkCopy.BulkCopyTimeout = 0;
                Console.WriteLine("Writing data...");
                bulkCopy.WriteToServer(table);
            }
        }

        private void AddRowsInTransactionForAccount(string connectionForTransaction, int accountId, CurrencyType currency, Dictionary<CurrencyType, int> dictionary)
        {
            using (SqlConnection _connectionForTransaction = new SqlConnection(connectionForTransaction))
            {
                var time = DateTime.Now.AddDays(_random.Next(-_dayPearHalfYear, -_dayPearTwoWeek));
                var dataSet = new DataSet();
                var table = dataSet.Tables.Add("MockTransaction");

                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("AccountId", typeof(int));
                table.Columns.Add("Amount", typeof(decimal));
                table.Columns.Add("Currency", typeof(int));
                table.Columns.Add("TransactionType", typeof(int));
                table.Columns.Add("Date", typeof(DateTime));

                for (var intRow = 1; intRow <= _transactionCount; intRow++)
                {
                    var type = GetTransactionType(dictionary);
                    var randomAmount = GetQuantityOperation((TransactionType)type);
                    _dateTimeTransaction = time.AddDays(_random.Next(-_dayPearHalfYear, -_dayPearTwoWeek))
                        .ToString(_dateFormat);

                    switch (type)
                    {
                        case (int)TransactionType.Deposit: dictionary[currency] += randomAmount; break;
                        case (int)TransactionType.Withdraw: dictionary[currency] -= randomAmount; break;
                        case (int)TransactionType.Transfer:

                            var dictionaryClone = new Dictionary<CurrencyType, int>();

                            foreach (var dic in dictionary)
                            {
                                dictionaryClone.Add(dic.Key, dic.Value);
                            }

                            dictionaryClone.Remove(currency);
                            var payee = _random.Next(1, dictionaryClone.Count);

                            dictionary[(CurrencyType)payee] += randomAmount;
                            dictionary[currency] -= randomAmount;

                            table.Rows.Add(
                               _globalcount,
                               accountId,// TODO
                               -randomAmount,
                               (CurrencyType)payee,
                               type,
                                _dateTimeTransaction
                              );
                            _globalcount++;

                            break;
                        default: throw new Exception("This type has not transaction in enum TransactionType");
                    }

                    table.Rows.Add(
                        _globalcount,
                        accountId,
                        randomAmount,
                        currency,
                        type,
                        _dateTimeTransaction
                    );
                    _globalcount++;
                }

                var bulkCopy = new SqlBulkCopy(_connectionForTransaction);
                _connectionForTransaction.Open();
                bulkCopy.DestinationTableName = _transactionTable;
                bulkCopy.BulkCopyTimeout = 0;
                bulkCopy.WriteToServer(table);
            }
        }

        private int GetQuantityOperation(TransactionType type)
        {
            switch (type)
            {
                case TransactionType.Deposit: return _random.Next(100, 10000);
                case TransactionType.Withdraw: return _random.Next(-1000, 0);
                case TransactionType.Transfer: return _random.Next(100, 10000);
                default: throw new Exception("This type has not transaction in enum TransactionType");
            }
        }

        private int GetTransactionType(Dictionary<CurrencyType, int> dictionary)
        {
            foreach (var d in dictionary.Values)
            {
                if (1 < dictionary.Count && 0 < d)
                {
                    return _random.Next(1, 4);
                }

                if (0 < d)
                {
                    return _random.Next(1, 3);
                }
            }

            return (int)TransactionType.Deposit;
        }

        private Dictionary<CurrencyType, int> CreateMultipleCurrency(DateTime timeCreated, DataTable table, int intRow)
        {
            var amount = 0;
            var dictionary = new Dictionary<CurrencyType, int> { { CurrencyType.RUB, amount } };
            var currencyCount = _random.Next(1, 5);
            var listCurrency = new List<CurrencyType>() {
                CurrencyType.RUB,
                CurrencyType.USD,
                CurrencyType.EUR,
                CurrencyType.JPY };

            for (int i = 0; i < currencyCount; i++)
            {
                if (dictionary.Count != currencyCount)
                {
                    var currencyRandom = _random.Next(1, listCurrency.Count + 1);
                    if (!dictionary.ContainsKey((CurrencyType)currencyRandom))
                    {
                        var timeClosed = DateTime.Now.AddDays(_random.Next(-_dayPearTwoWeek, 0)).ToString(_dateFormat);
                        var IsAccountDeleted = _random.Next(0, 2) == 1;

                        table.Rows.Add(
                            intRow,
                            intRow,
                            currencyRandom,
                            timeCreated.AddDays(_random.Next(-_dayPearYear, -_dayPearHalfYear)).ToString(_dateFormat),
                            IsAccountDeleted == true ? timeClosed : null,
                            IsAccountDeleted);

                        dictionary.Add((CurrencyType)currencyRandom, amount);
                        listCurrency.Remove((CurrencyType)currencyRandom);
                    }
                }
            }

            return dictionary;
        }
    }
}
