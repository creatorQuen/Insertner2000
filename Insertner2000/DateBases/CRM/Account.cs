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
        private const string _dateFormat = "dd.MM.yyyy HH:mm:ss.fffffff";
        private Random _random = new Random();

        public void CreateAccounts(int countStart, int countEnd, string connectionForLeadAccount)
        {
            using (SqlConnection _connection = new SqlConnection(connectionForLeadAccount))
            {
                Console.WriteLine("Starting..");

                var dataSet = new DataSet();
                var table = dataSet.Tables.Add("ttmpData");

                Console.WriteLine("Creating dataTable..");

                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("LeadId", typeof(int));
                table.Columns.Add("Currency", typeof(int));
                table.Columns.Add("CreatedOn", typeof(DateTime));
                table.Columns.Add("Closed", typeof(DateTime));
                table.Columns.Add("IsDeleted", typeof(bool));

                Console.WriteLine("Adding data to dataTable..");

                for (var intRow = countStart; intRow <= countEnd; intRow++)
                {

                    var listCurrency = new List<CurrencyType> { CurrencyType.RUB, CurrencyType.USD, CurrencyType.EUR, CurrencyType.JPY };
                    var listCurrencyAccount = new List<CurrencyType>();

                    var isDeleted = GetIsDeletedRandom();
                    var currencyCount = _random.Next(1, 5);
                    var timeCreated = DateTime.Now.AddMilliseconds(intRow).ToString(_dateFormat);
                    var closed = GetClosedDataTimeByIsDeleted(isDeleted);

                    table.Rows.Add(
                        intRow,
                        intRow,
                        CurrencyType.RUB,
                        timeCreated,
                        closed,
                        isDeleted);

                    listCurrencyAccount.Add(CurrencyType.RUB);

                    for (int i = 0; i < currencyCount; i++)
                    {
                        if (1 < currencyCount)
                        {
                            var currencyRandom = _random.Next(1, listCurrency.Count + 1);
                            if (!listCurrencyAccount.Contains((CurrencyType)currencyRandom))
                            {
                                table.Rows.Add(
                                    intRow,
                                    intRow,
                                    (CurrencyType)currencyRandom,
                                    timeCreated,
                                    closed,
                                    isDeleted);

                                listCurrencyAccount.Add((CurrencyType)currencyRandom);
                                listCurrency.Remove((CurrencyType)currencyRandom);
                            }
                        }
                    }
                }

                Console.WriteLine("Open dataBase..");

                var bulkCopy = new SqlBulkCopy(_connection);
                _connection.Open();
                bulkCopy.DestinationTableName = _accountTable;
                bulkCopy.BulkCopyTimeout = 0;
                Console.WriteLine("Writing data...");
                bulkCopy.WriteToServer(table);
            }
        }

        private DateTime GetClosedDataTimeByIsDeleted(bool isDeleted)
        {
            return DateTime.Now;
        }

        private bool GetIsDeletedRandom()
        {
            return _random.Next(0, 2) == 1;
        }
    }
}
