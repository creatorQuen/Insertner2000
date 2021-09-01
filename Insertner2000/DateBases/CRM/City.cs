using System;
using System.Data;
using System.Data.SqlClient;
using Insertner2000.Entity;

namespace Insertner2000.Tables
{
    public class City
    {
        private string _nameTable = "City";
        public void CreateCities(int countStart, string connectionString)
        {
            using (SqlConnection _connection = new SqlConnection(connectionString))
            {
                Console.WriteLine("Starting..");
                DataSet dataSet = new DataSet();
                int countCity = Enum.GetNames(typeof(CityList)).Length;

                Console.WriteLine("Creating datatable..");
                DataTable table;
                table = dataSet.Tables.Add("MockCity");
                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("Name", typeof(string));

                Console.WriteLine("Adding data to datatable..");
                for (int intRow = countStart; intRow <= countCity; intRow++)
                {
                    table.Rows.Add(intRow, $"{(CityList)intRow}");
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
