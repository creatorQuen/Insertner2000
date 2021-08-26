using Insertner2000.Entity;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Insertner2000.DateBases.TStore
{
    public class TStore
    {
        private string _nameTable = "[TStore].[dbo].[Transaction]";
        private const string _dateFormat = "dd.MM.yyyy HH:mm:ss.fffffff";
        public void CreateTStores(int countStart, int countEnd, string connectionForLead, string connectionForTransaction)
        {
            using (SqlConnection _connection = new SqlConnection(connectionForLead))
            {
                Console.WriteLine("Starting..");

                Random randomAmount = new Random();

                Random randomTransactionType = new Random();
                var transactionType = 1;

                var time = DateTime.Now;

                DataSet dataSet = new DataSet();
                Console.WriteLine("Creating datatable..");
                DataTable table;
                table = dataSet.Tables.Add("MockTransaction");
                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("AccountId", typeof(int));
                table.Columns.Add("TransactionType", typeof(int));
                table.Columns.Add("Date", typeof(DateTime));
                table.Columns.Add("Amount", typeof(decimal));

                Console.WriteLine("Adding data to datatable..");

                //using (SqlConnection _sss = new SqlConnection(connectionForTransaction))
                //{

                //}

                for (int intRow = countStart; intRow <= countEnd; intRow++)
                {
                    table.Rows.Add(
                        intRow,
                        intRow,
                        transactionType,
                         ((DateTime)(time.AddMilliseconds(intRow))).ToString(_dateFormat),
                        transactionType);
                }

                Console.WriteLine("Open database..");
                SqlBulkCopy bulkCopy = new SqlBulkCopy(_connection);

                _connection.Open();
                bulkCopy.DestinationTableName = _nameTable;
                bulkCopy.BulkCopyTimeout = 0;

                Console.WriteLine("Writing data...");
                bulkCopy.WriteToServer(table);
            }
        }

    }
}
