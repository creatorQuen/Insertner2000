using Insertner2000.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Insertner2000.DateBases.CRM
{
    public class Account
    {
        private string _accountTable = "[LeadA].[dbo].[Account]";
        private static string _transactionTable = "[TStore].[dbo].[Transaction]";
        private const string _dateFormat = "dd.MM.yyyy HH:mm:ss.fffffff";
        private int GLOBALCOUNT = 1;
        private const int _transactionCount = 10;
        private const int _dayPearYear = 365;
        private const int _dayPearHalfYear = 180;
        private const int _dayPearTwoWeek = 14;
        private string _dateTimeTransaction;
        private Random random = new Random();

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

                AddRowsInTableAccount(countStart, countEnd, random, timeCreated, table, connectionForTransaction);

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
                var time = DateTime.Now.AddDays(random.Next(-120, -14));
                var ammount = 0;
                var currencyType = 0;
                var dictionary = new Dictionary<CurrencyType, int>();

                foreach (var l in list)
                {
                    dictionary.Add(l, ammount);
                }

                var dataSet = new DataSet();
                var table = dataSet.Tables.Add("MockTransaction");

                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("AccountId", typeof(int));
                table.Columns.Add("Amount", typeof(int)); //Amount
                table.Columns.Add("Currency", typeof(int)); //currency
                table.Columns.Add("TransactionType", typeof(decimal)); //TRType
                table.Columns.Add("Date", typeof(DateTime));

                for (int intRow = 1; intRow <= _transactionCount; intRow++)
                {
                    var randomCurrency = list[random.Next(list.Count)];

                    AddRowsInTableTransaction(accountId, time, out var type, dictionary, table, randomCurrency, out var randomAmount);

                    switch (type)
                    {
                        case (int)TransactionType.Deposit: dictionary[randomCurrency] += randomAmount; break;
                        case (int)TransactionType.Withdraw: dictionary[randomCurrency] -= randomAmount; break;
                        case (int)TransactionType.Transfer:

                            var dictionaryClone = new Dictionary<CurrencyType, int>();

                            foreach (var dic in dictionary)
                            {
                                dictionaryClone.Add(dic.Key, dic.Value);
                            }

                            dictionaryClone.Remove(randomCurrency);
                            var payee = random.Next(1, dictionaryClone.Count);
                            dictionary[(CurrencyType)payee] += randomAmount;
                            dictionary[randomCurrency] -= randomAmount;

                            table.Rows.Add(
                               GLOBALCOUNT,
                               accountId,
                               -randomAmount,
                               (CurrencyType)payee,
                               type,
                                _dateTimeTransaction
                              );

                            GLOBALCOUNT++;
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

        private void AddRowsInTableTransaction(int accountId, DateTime time, out int type, Dictionary<CurrencyType, int> dictionary, DataTable table, CurrencyType randomCurrency, out int randomAmount)
        {
            type = GetTransactionType(dictionary);
            table.Rows.Add(
                GLOBALCOUNT,
                accountId,
                randomAmount = GetQuantityOperation((TransactionType)type),
                randomCurrency,
                type,
                 _dateTimeTransaction = time.AddDays(random.Next(-_dayPearHalfYear, -_dayPearTwoWeek)).ToString(_dateFormat)
               );
            GLOBALCOUNT++;
        }

        private int GetQuantityOperation(TransactionType type)
        {
            switch (type)
            {
                case TransactionType.Deposit: return random.Next(100, 10000);
                case TransactionType.Withdraw: return random.Next(-1000, 0);
                case TransactionType.Transfer: return random.Next(100, 10000);
                default: throw new Exception("This type has not transaction in enum TransactionType");
            }
        }

        private int GetTransactionType(Dictionary<CurrencyType, int> dictionary)
        {

            foreach (var d in dictionary.Values)
            {
                if (1 < dictionary.Count && 0 < d)
                {
                    return random.Next(1, 4);
                }

                if (0 < d)
                {
                    return random.Next(1, 3);
                }
            }

            return (int)TransactionType.Deposit;
        }

        private void AddRowsInTableAccount(int countStart, int countEnd, Random random, DateTime timeCreated, DataTable table, string connectionForTransaction)
        {
            for (int intRow = countStart; intRow <= countEnd; intRow++)
            {
                var currencyCount = random.Next(1, 5);
                var crntList = new List<CurrencyType>();
                var listCurrency = new List<CurrencyType>() {
                    CurrencyType.RUB,
                    CurrencyType.USD,
                    CurrencyType.EUR,
                    CurrencyType.JPY };

                GenerateRandomCurrency((int)CurrencyType.RUB, timeCreated, table, intRow);
                crntList.Add(CurrencyType.RUB);

                CreateMultipleCurrency(random, timeCreated, table, intRow, currencyCount, crntList, listCurrency);

                AddRowsInTransactionForAccount(connectionForTransaction, intRow, crntList);
            }
        }

        private void CreateMultipleCurrency(Random random, DateTime timeCreated, DataTable table, int intRow, int currencyCount, List<CurrencyType> crntList, List<CurrencyType> listCurrency)
        {
            for (int i = 0; i < currencyCount; i++)
            {
                if (currencyCount > (int)CurrencyType.RUB)
                {
                    var currencyRandom = random.Next(1, listCurrency.Count + 1);
                    if (!crntList.Contains((CurrencyType)currencyRandom))
                    {
                        GenerateRandomCurrency(currencyRandom, timeCreated, table, intRow);
                        crntList.Add((CurrencyType)currencyRandom);
                        listCurrency.Remove((CurrencyType)currencyRandom);
                    }
                }
            }
        }

        private void GenerateRandomCurrency(int currencyType, DateTime timeCreated, DataTable table, int rowNumber)
        {
            var timeClosed = DateTime.Now.AddDays(random.Next(-_dayPearTwoWeek, 0)).ToString(_dateFormat);
            var IsAccountDeleted = false;

            if (random.Next(0, 2) == 1)
            {
                IsAccountDeleted = true;
            }

            table.Rows.Add(
            rowNumber,
            rowNumber,
            currencyType,
            timeCreated.AddDays(random.Next(-_dayPearYear, -_dayPearHalfYear)).ToString(_dateFormat),
            IsAccountDeleted == true ? timeClosed : null,
            IsAccountDeleted);
        }
    }
}
