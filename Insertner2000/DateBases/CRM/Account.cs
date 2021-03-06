using Insertner2000.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Insertner2000.DateBases.CRM
{
    public class Account
    {
        private const string messageStart = "Start write.\n";
        private const string messageEnd = "\nCongratulation. Write is done";
        private const string _dateFormat = "dd.MM.yyyy HH:mm:ss.fffffff";
        private Random _random = new Random();
        private const int _dayPearYear = 365;
        private const int _dayPearHalfYear = 180;
        private const int _dayPearTwoWeek = 14;

        public void CreateAccounts(string connectionForLeadAccount, string connectionForTransaction)
        {
            using (SqlConnection _connection = new SqlConnection(connectionForLeadAccount))
            {
                Console.WriteLine("Starting..");

                var LeadId = ConfigurationForTables.LeadIdStart;
                var dataSet = new DataSet();
                var table = dataSet.Tables.Add("ttmpData");

                Console.WriteLine("Creating dataTable..");

                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("LeadId", typeof(int));
                table.Columns.Add("Currency", typeof(int));
                table.Columns.Add("CreatedOn", typeof(DateTime));
                table.Columns.Add("Closed", typeof(DateTime));
                table.Columns.Add("IsDeleted", typeof(bool));

                Console.WriteLine("Adding data to dataTable [Accounts]");

                var difference = ConfigurationForTables.AccountIdEnd - ConfigurationForTables.AccountIdStart;
                var percent = 0;
                Console.Write(messageStart);
                using (var progress = new ProgressBar(/*В место коментария можно указать колличество решеточек*/))
                {
                    for (var id = ConfigurationForTables.AccountIdStart; id <= ConfigurationForTables.AccountIdEnd; LeadId++)
                    {
                        var delta = LeadId + 1 - ConfigurationForTables.AccountIdStart;
                        var currentPercent = delta * 100 / difference;
                        if (currentPercent != percent)
                        {
                            percent = currentPercent;
                            progress.Report((double)percent / 100);
                        }

                        var listCurrency = new List<CurrencyType> { CurrencyType.RUB, CurrencyType.USD, CurrencyType.EUR, CurrencyType.JPY };
                        var listCurrencyAccount = new List<CurrencyType>();
                        var array = Enum.GetValues(typeof(CurrencyType));
                        var dictionary = new Dictionary<int, CurrencyType>();

                        var isDeleted = GetIsDeletedRandom();
                        var closed = GetClosedDataTimeByIsDeleted(isDeleted);
                        var currencyCount = (int)array.GetValue(_random.Next(array.Length));
                        var timeCreated = DateTime.Now
                            .AddDays(_random.Next(-_dayPearYear * 3 / 2, -_dayPearYear))
                            .AddHours(_random.Next(24))
                            .AddMinutes(_random.Next(60))
                            .AddSeconds(_random.Next(60))
                            .AddMilliseconds(_random.Next(1_0000_000))
                            .ToString(_dateFormat);

                        table.Rows.Add(
                            id,
                            LeadId,
                            CurrencyType.RUB,
                            timeCreated,
                            closed,
                            isDeleted
                            );
                        dictionary.Add(id, CurrencyType.RUB);
                        listCurrencyAccount.Add(CurrencyType.RUB);

                        for (int i = 0; i < currencyCount; i++)
                        {
                            if (1 < currencyCount)
                            {
                                var currencyRandom = _random.Next(1, listCurrency.Count + 1);
                                if (!listCurrencyAccount.Contains((CurrencyType)currencyRandom))
                                {
                                    var isDeletedInCurrecyList = GetIsDeletedRandom();
                                    var closedInCurrecyList = GetClosedDataTimeByIsDeleted(isDeletedInCurrecyList);
                                    timeCreated = DateTime.Now
                                        .AddDays(_random.Next(-_dayPearYear, -_dayPearHalfYear))
                                        .AddHours(_random.Next(24))
                                        .AddMinutes(_random.Next(60))
                                        .AddSeconds(_random.Next(60))
                                        .AddMilliseconds(_random.Next(1_0000_000))
                                        .ToString(_dateFormat);

                                    table.Rows.Add(
                                        id++,
                                        LeadId,
                                        (CurrencyType)currencyRandom,
                                        timeCreated,
                                        closedInCurrecyList,
                                        isDeletedInCurrecyList);

                                    dictionary.Add(id, (CurrencyType)currencyRandom);
                                    listCurrencyAccount.Add((CurrencyType)currencyRandom);
                                    listCurrency.Remove((CurrencyType)currencyRandom);
                                }
                            }
                        }
                        id++;

                        var store = new TStore.TStore();
                        store.CreateTStores(dictionary, connectionForTransaction);
                    }
                }
                Console.WriteLine(messageEnd);

                Console.WriteLine("Open dataBase..");

                var bulkCopy = new SqlBulkCopy(_connection);
                _connection.Open();
                bulkCopy.DestinationTableName = ConfigurationForTables.AccountTable;
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