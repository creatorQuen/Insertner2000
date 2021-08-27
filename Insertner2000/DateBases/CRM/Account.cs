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
        private static Random random = new Random();

        public void CreateAcounts(int countStart, int countEnd, string connectionForLeadAccount, string connectionForTransaction)
        {
            using (SqlConnection _connection = new SqlConnection(connectionForLeadAccount))
            {
                Console.WriteLine("Starting..");

                //Random random = new Random();

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
                var time = DateTime.Now;
                var type = (int)TransactionType.Deposit;
                var ammount = 0;
                var currencyType = 0;

                DataSet dataSet = new DataSet();
                DataTable table;
                table = dataSet.Tables.Add("MockTransaction");
                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("AccountId", typeof(int));
                table.Columns.Add("Amount", typeof(decimal));
                table.Columns.Add("Currency", typeof(int));
                table.Columns.Add("TransactionType", typeof(int));
                table.Columns.Add("Date", typeof(DateTime));

                for (int intRow = 1; intRow <= 5; intRow++)
                {
                    table.Rows.Add(
                        GLOBALCOUNT,
                        accountId,
                        ammount = GetQuantityOperation((TransactionType)type),
                        list[random.Next(list.Count)],
                        type = GetTransactionType(ammount, list),
                         ((DateTime)(time.AddMilliseconds(intRow))).ToString(_dateFormat)
                       );
                    GLOBALCOUNT++;
                }

                SqlBulkCopy bulkCopy = new SqlBulkCopy(_connectionForTransaction);
                _connectionForTransaction.Open();
                bulkCopy.DestinationTableName = _transactionTable;
                bulkCopy.BulkCopyTimeout = 0;
                bulkCopy.WriteToServer(table);

            }
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
            //return random.Next(100, 10000);
        }

        private static int GetTransactionType(int ammount, List<CurrencyType> list)
        {
            if (0 <= ammount)
            { 
                return random.Next(2, 4);
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
            var timeClosed = DateTime.Now;
            var time = timeClosed.AddDays(120).ToString(_dateFormat);
            var IsAccountDeleted = false;

            if (random.Next(0, 2) == 1)
            {
                IsAccountDeleted = true;
            }

            table.Rows.Add(
            rowNuber,
            rowNuber,
            currenyType,
            ((DateTime)(timeCreated.AddMilliseconds(rowNuber))).ToString(_dateFormat),
            IsAccountDeleted == true ? time : null,
            IsAccountDeleted);
        }
    }
}
