using Insertner2000.Entity;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Insertner2000.DateBases.TStore
{
    public class TStore
    {
        private static string _transactionTable = "[TStore].[dbo].[Transaction]";
        private const string _dateFormat = "dd.MM.yyyy HH:mm:ss.fffffff";
        private const int countEnd = 10;

        public void CreateTStores(int accountId, string connectionForTransaction)
        {
            using (SqlConnection _connection = new SqlConnection(connectionForTransaction))
            {
                //Console.WriteLine("Starting..");

                var random = new Random();
                var dataSet = new DataSet();
                var table = dataSet.Tables.Add("MockTransaction");

                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("AccountId", typeof(int));
                table.Columns.Add("Amount", typeof(decimal));
                table.Columns.Add("Currency", typeof(int));
                table.Columns.Add("TransactionType", typeof(int));
                table.Columns.Add("Date", typeof(DateTime));

                var ammount = 0;
                var currency = GetCurrencyByAccountId(accountId);
                var transactionType = GetTransactionTypeByAmount(ammount);
                var dateTimeTransaction = DateTime.Now.AddMilliseconds(ammount).ToString(_dateFormat);

                for (var intRow = 0; intRow <= countEnd; intRow++)
                {
                    table.Rows.Add(
                        intRow,
                        accountId,
                        ammount,
                        currency,
                        transactionType,
                        dateTimeTransaction
                        );
                }

                var bulkCopy = new SqlBulkCopy(_connection);
                _connection.Open();
                bulkCopy.DestinationTableName = _transactionTable;
                bulkCopy.BulkCopyTimeout = 0;
                bulkCopy.WriteToServer(table);
            }
        }

        private int GetTransactionTypeByAmount(int amount)
        {
            if (amount != 0)
            {
                Random random = new Random();
                return random.Next(2, 4);
            }

            return (int)TransactionType.Deposit;
        }

        private int GetCurrencyByAccountId(int id)
        {
            return id;
        }
    }
}
