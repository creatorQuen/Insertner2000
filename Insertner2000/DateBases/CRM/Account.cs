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
        private static int GLOBALCOUNT = 1;
        private static int _transactionCount = 10;
        private static int _dayPearYear = 365;
        private static int _dayPearHalfYear = 180;
        private static int _dayPearTwoWeek = 14;
        private static string _dateTimeTransaction;
        private static Random random = new Random();

        public void CreateAcounts(int countStart, int countEnd, string connectionForLeadAccount, string connectionForTransaction)
        {
            using (SqlConnection _connection = new SqlConnection(connectionForLeadAccount))
            {
                Console.WriteLine("Starting..");
                var timeCreated = DateTime.Now;
                DateTime timeClosed = new DateTime();

                DataSet dataSet = new DataSet();
                Console.WriteLine("Creating datatable..");
                DataTable table;
                table = dataSet.Tables.Add("ttmpData");
                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("LeadId", typeof(int));
                table.Columns.Add("Currency", typeof(int));
                table.Columns.Add("CreatedOn", typeof(DateTime));
                table.Columns.Add("Closed", typeof(DateTime));
                table.Columns.Add("IsDeleted", typeof(bool));

                Console.WriteLine("Adding data to datatable..");

                AddRowsInTable(countStart, countEnd, random, timeCreated, table, connectionForTransaction);
               

                Console.WriteLine("Open database..");
                SqlBulkCopy bulkCopy = new SqlBulkCopy(_connection);
                _connection.Open();
                bulkCopy.DestinationTableName = _accountTable;
                bulkCopy.BulkCopyTimeout = 0;
                Console.WriteLine("Writing data...");
                bulkCopy.WriteToServer(table);
            }
        }

        private static void AddRowsInTransactionForAccount(string connectionForTransaction, int accountId, List<CurrencyType> list)
        {
            using (SqlConnection _connectionForTransaction = new SqlConnection(connectionForTransaction))
            {
                var time = DateTime.Now.AddDays(random.Next(-120, -14));
                var type = (int)TransactionType.Deposit;
                var ammount = 0;
                var currencyType = 0;
                Dictionary<CurrencyType, int> dictionary = new Dictionary<CurrencyType, int>();
                foreach (var l in list)
                {
                    dictionary.Add(l, ammount);
                }

                DataSet dataSet = new DataSet();
                DataTable table;
                table = dataSet.Tables.Add("MockTransaction");
                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("AccountId", typeof(int));
                table.Columns.Add("Currency", typeof(int));
                table.Columns.Add("TransactionType", typeof(int));
                table.Columns.Add("Amount", typeof(decimal));
                table.Columns.Add("Date", typeof(DateTime));

                for (int intRow = 1; intRow <= _transactionCount; intRow++)
                {
                    var randomCurrency = list[random.Next(list.Count)];
                    var randomAmount = 0;

                    AddRowsIntoTable(accountId, time, out type, dictionary, table, randomCurrency, out randomAmount);
                    
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
                               (CurrencyType)payee,
                               type,
                               -randomAmount,
                                _dateTimeTransaction
                              );

                            GLOBALCOUNT++;

                            break;
                        default: throw new Exception("This type has not transaction in enum TransactionType");
                    }
                }

                SqlBulkCopy bulkCopy = new SqlBulkCopy(_connectionForTransaction);
                _connectionForTransaction.Open();
                bulkCopy.DestinationTableName = _transactionTable;
                bulkCopy.BulkCopyTimeout = 0;
                bulkCopy.WriteToServer(table);

            }
        }

        private static void AddRowsIntoTable(int accountId, DateTime time, out int type, Dictionary<CurrencyType, int> dictionary, DataTable table, CurrencyType randomCurrency, out int randomAmount)
        {
            table.Rows.Add(
                GLOBALCOUNT,
                accountId,
                randomCurrency,
                type = GetTransactionType(dictionary),
                randomAmount = GetQuantityOperation((TransactionType)type),
                 _dateTimeTransaction = ((DateTime)(time.AddDays(random.Next(-_dayPearHalfYear, -_dayPearTwoWeek)))).ToString(_dateFormat)
               );
            GLOBALCOUNT++;
        }

        private static int GetQuantityOperation(TransactionType type)
        {
            switch (type)
            {
                case TransactionType.Deposit: return random.Next(100, 10000);
                case TransactionType.Withdraw : return random.Next(-1000, 0); 
                case TransactionType.Transfer : return random.Next(100, 10000);
                default: throw new Exception("This type has not transaction in enum TransactionType");
            }
        }

        private static int GetTransactionType(Dictionary<CurrencyType, int> dictionary)
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

        private static void AddRowsInTable(int countStart, int countEnd, Random random, DateTime timeCreated, DataTable table, string connectionForTransaction)
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

        private static void CreateMultipleCurrency(Random random, DateTime timeCreated, DataTable table, int intRow, int currencyCount, List<CurrencyType> crntList, List<CurrencyType> listCurrency)
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

        private static void GenerateRandomCurrency(int currenyType, DateTime timeCreated, DataTable table, int rowNuber)
        {
            var timeClosed = DateTime.Now.AddDays(random.Next(-_dayPearTwoWeek, 0)).ToString(_dateFormat); //AddDays(120).ToString(_dateFormat);
            var IsAccountDeleted = false;

            if (random.Next(0, 2) == 1)
            {
                IsAccountDeleted = true;
            }

            table.Rows.Add(
            rowNuber,
            rowNuber,
            currenyType,
            ((DateTime)(timeCreated.AddDays(random.Next(-_dayPearYear, -_dayPearHalfYear)))).ToString(_dateFormat),
            IsAccountDeleted == true ? timeClosed : null,
            IsAccountDeleted);
        }
    }
}
