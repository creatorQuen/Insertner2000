using Insertner2000.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Insertner2000.DateBases.TStore
{
    public class TStore
    {
        private static string _transactionTable = "[TStore].[dbo].[Transaction]";
        private const string _dateFormat = "dd.MM.yyyy HH:mm:ss.fffffff";
        private const int _countEnd = 10;
        private readonly Random _random = new Random();

        public void CreateTStores(int accountId, string connectionForTransaction)
        {
            using (SqlConnection _connection = new SqlConnection(connectionForTransaction))
            {
                //Console.WriteLine("Starting..");

                var dataSet = new DataSet();
                var table = dataSet.Tables.Add("MockTransaction");

                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("AccountId", typeof(int));
                table.Columns.Add("Amount", typeof(decimal));
                table.Columns.Add("Currency", typeof(int));
                table.Columns.Add("TransactionType", typeof(int));
                table.Columns.Add("Date", typeof(DateTime));

                var ammount = 0;
                var transactionType = GetTransactionTypeByAmount(ammount);
                var currency = GetCurrencyByAccountId(accountId);
                var dateTimeTransaction = DateTime.Now.AddMilliseconds(ammount).ToString(_dateFormat);

                for (var intRow = 0; intRow <= _countEnd; intRow++)
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

        private TransactionType GetTransactionTypeByAmount(int amount)
        {
            //foreach (var dic in Dictionary.Value)
            //{
            //if( 1 < Dictionary.Count && 0 < dic)
            if (0 < amount)
            {
                return (TransactionType)_random.Next(2, 4);
            }
            //}

            return TransactionType.Deposit;
        }

        private int GetCurrencyByAccountId(int id)
        {
            return id;
        }

        private int GetRandomAmountByTransactionType(TransactionType type)
        {
            switch (type)
            {
                case TransactionType.Deposit: return _random.Next(100,10000);
                case TransactionType.Withdraw: return _random.Next(-100,-10);
                case TransactionType.Transfer: return _random.Next(100,1000);
                default: throw new Exception("this type doesn't have in TransactionType");
            }
        }
    }
}
