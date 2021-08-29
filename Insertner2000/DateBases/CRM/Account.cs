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
        private const int _dayPearTwoWeek = 14;
        private const int _dayPearYear = 365;
        private const int _dayPearHalfYear = 180;

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
                   // Enum.GetValues(_random.Next(listCurrency.Count + 1));
                   // var currencyRandom = (CurrencyType)array.GetValue(_random.Next(array.Length));

                    var array = Enum.GetValues(typeof(CurrencyType));

                    var isDeleted = GetIsDeletedRandom();
                    var closed = GetClosedDataTimeByIsDeleted(isDeleted);
                    var currencyCount = (int)array.GetValue(_random.Next(array.Length));
                    var timeCreated = DateTime.Now.AddDays(_random.Next(-_dayPearYear, -_dayPearHalfYear)).ToString(_dateFormat);

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

        private string GetClosedDataTimeByIsDeleted(bool isDeleted)
        {
            return isDeleted ? DateTime.Now.AddDays(_random.Next(-_dayPearTwoWeek, 0)).ToString(_dateFormat) : null;
        }

        private bool GetIsDeletedRandom()
        {
            return _random.Next(0, 2) == 1;
        }
    }
}
