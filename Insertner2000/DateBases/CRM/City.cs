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

                var dataSet = new DataSet();
                var countCity = Enum.GetNames(typeof(CityList)).Length;

                Console.WriteLine("Creating dataTable..");

                var table = dataSet.Tables.Add("MockCity");
                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("Name", typeof(string));
                
                Console.WriteLine("Adding data to dataTable [Cities]");

                for (var intRow = countStart; intRow <= countCity; intRow++)
                {
                    if (intRow % (countCity / 100) == 0)
                    {
                        Console.WriteLine($"Cities done: {100 * intRow / countCity}%");
                    }

                    table.Rows.Add(intRow, $"{(CityList)intRow}");
                }

                Console.WriteLine("Open database..");
                var bulkCopy = new SqlBulkCopy(_connection);

                _connection.Open();
                bulkCopy.DestinationTableName = ConfigurationForTables.CityTable;
                bulkCopy.BulkCopyTimeout = 0;

                Console.WriteLine("Writing data...");
                bulkCopy.WriteToServer(table);
            }
        }
    }
}