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

        public void CreateAcounts(int countStart, int countEnd, string connectionForLeadAccount, string connectionForTransaction)
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

                AddRowsInTableAccount(countStart, countEnd, timeCreated, table, connectionForTransaction);

                Console.WriteLine("Open database..");
                var bulkCopy = new SqlBulkCopy(_connection);
                _connection.Open();
                bulkCopy.DestinationTableName = _accountTable;
                bulkCopy.BulkCopyTimeout = 0;
                Console.WriteLine("Writing data...");
                bulkCopy.WriteToServer(table);
            }
        }

        private void AddRowsInTransactionForAccount(string connectionForTransaction, int accountId, List<CurrencyType> list)
        {
            using (SqlConnection _connectionForTransaction = new SqlConnection(connectionForTransaction))
            {
                var time = DateTime.Now.AddDays(_random.Next(-_dayPearHalfYear, -_dayPearTwoWeek));
                var ammount = 0;

                var dictionary = new Dictionary<CurrencyType, int>();

                foreach (var l in list)
                {
                    dictionary.Add(l, ammount);
                }

                var dataSet = new DataSet();
                var table = dataSet.Tables.Add("MockTransaction");

                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("AccountId", typeof(int));
                table.Columns.Add("Amount", typeof(int));
                table.Columns.Add("Currency", typeof(int));
                table.Columns.Add("TransactionType", typeof(decimal));
                table.Columns.Add("Date", typeof(DateTime));

                var listCurrency = Enumerable.ToList(dictionary.Keys);

                foreach (var randomCurrency in listCurrency)
                {
                    var type = TransactionType.Deposit;
                    var randomAmount = GetQuantityOperation(type);
                    for (int intRow = 1; intRow <= _transactionCount; intRow++)
                    {
                        AddRowsInTableTransaction(accountId, time, out type, dictionary, table, randomCurrency, out randomAmount);
                    }
                    
                    switch (type)
                    {
                        case TransactionType.Deposit: dictionary[randomCurrency] += randomAmount; break;
                        case TransactionType.Withdraw: dictionary[randomCurrency] -= randomAmount; break;
                        case TransactionType.Transfer:

                            var dictionaryClone = new Dictionary<CurrencyType, int>();

                            foreach (var dic in dictionary)
                            {
                                dictionaryClone.Add(dic.Key, dic.Value);
                            }

                            dictionaryClone.Remove(randomCurrency);
                            var payee = _random.Next(1, dictionaryClone.Count);
                            dictionary[(CurrencyType)payee] += randomAmount;
                            dictionary[randomCurrency] -= randomAmount;

                            table.Rows.Add(
                               _globalcount,
                               accountId,
                               -randomAmount,
                               (CurrencyType)payee,
                               type,
                                _dateTimeTransaction
                              );

                            _globalcount++;
                            break;
                        default: throw new Exception("This type has not transaction in enum TransactionType");
                    }

                }





                var bulkCopy = new SqlBulkCopy(_connectionForTransaction);
                _connectionForTransaction.Open();
                bulkCopy.DestinationTableName = _transactionTable;
                bulkCopy.BulkCopyTimeout = 0;
                bulkCopy.WriteToServer(table);
            }
        }

        private void AddRowsInTableTransaction(int accountId, DateTime time, out TransactionType type, Dictionary<CurrencyType, int> dictionary, DataTable table, CurrencyType randomCurrency, out int randomAmount)
        {
            type = (TransactionType)GetTransactionType(dictionary);
            table.Rows.Add(
                _globalcount,
                accountId,
                randomAmount = GetQuantityOperation(type),
                randomCurrency,
                type,
                 _dateTimeTransaction = time.AddDays(_random.Next(-_dayPearHalfYear, -_dayPearTwoWeek)).ToString(_dateFormat)
               );
            _globalcount++;
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

        private void AddRowsInTableAccount(int countStart, int countEnd, DateTime timeCreated, DataTable table, string connectionForTransaction)
        {
            for (var intRow = countStart; intRow <= countEnd; intRow++)
            {
                var array = Enum.GetValues(typeof(CurrencyType));
                var countCurrency = array.Length;
                var crntList = new List<CurrencyType>();
                var listCurrency = new List<CurrencyType> {
                    CurrencyType.RUB,
                    CurrencyType.USD,
                    CurrencyType.EUR,
                    CurrencyType.JPY };

                GenerateRandomCurrency(CurrencyType.RUB, timeCreated, table, intRow);
                crntList.Add(CurrencyType.RUB);

                for (var i = 0; i < countCurrency; i++)
                {
                    if (countCurrency > 1)
                    {
                        var currencyRandom = (CurrencyType)array.GetValue(_random.Next(array.Length));
                        if (!crntList.Contains(currencyRandom))
                        {
                            GenerateRandomCurrency(currencyRandom, timeCreated, table, intRow);
                            crntList.Add(currencyRandom);
                            listCurrency.Remove(currencyRandom);
                        }
                    }
                }

                AddRowsInTransactionForAccount(connectionForTransaction, intRow, crntList);
            }
        }

        private void GenerateRandomCurrency(CurrencyType currencyType, DateTime timeCreated, DataTable table, int rowNumber)
        {
            var timeClosed = DateTime.Now.AddDays(_random.Next(-_dayPearTwoWeek, 0)).ToString(_dateFormat);
            var isAccountDeleted = _random.Next(0, 2) == 1;

            table.Rows.Add(
                rowNumber,
                rowNumber,
                (int)currencyType,
                timeCreated.AddDays(_random.Next(-_dayPearYear, -_dayPearHalfYear)).ToString(_dateFormat),
                isAccountDeleted ? timeClosed : null,
                isAccountDeleted);
        }
    }
}
