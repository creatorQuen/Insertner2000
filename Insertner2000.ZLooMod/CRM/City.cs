using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Insertner2000.ZLooLibs;

namespace Insertner2000.ZLooMod
{
    public class City
    {
        private const string NameTable = "MockCity";
        private const string Id = "Id";
        private const string Name = "Name";

        public void CreateTableCities(int start, int count, out DataTable tableCities)
        {
            DataSet dataSet = new();
            tableCities = dataSet.Tables.Add(NameTable);
            tableCities.Columns.Add(Id, typeof(int));
            tableCities.Columns.Add(Name, typeof(string));

            Cities.GetCities(out IReadOnlyList<string> cities);
            int countCity = cities.Count;
            int difference = count - start;
            int percent = 0;
            using (ProgressBar progress = new())
            {
                for (int rowId = start; rowId < countCity; rowId++)
                {
                    int delta = rowId + 1 - ConfigurationForTables.AccountIdStart;
                    int currentPercent = delta * 100 / difference;
                    if (currentPercent != percent)
                    {
                        percent = currentPercent;
                        progress.Report((double)percent / 100);
                    }

                    tableCities.Rows.Add(rowId, cities[rowId]);
                }
            }
        }
    }
}