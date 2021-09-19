using System;
using System.Collections.Generic;
using System.Data;
using Insertner2000.ZLooLibs;

namespace Insertner2000.ZLooMod
{
    public class Lead
    {
        private const string Id = "Id";
        private const string LastName = "LastName";
        private const string FirstName = "FirstName";
        private const string Patronymic = "Patronimic";

        public void CreateTable()
        {
            int countStart = 0;
            int countEnd = 1000;
            
            FIOs.GetFIOs(out IReadOnlyDictionary<int, IReadOnlyDictionary<string, string>> FIOsData);
            Random random = new();
            DataSet dataSet = new();
            DataTable table = dataSet.Tables.Add("MockLead");

            table.Columns.Add(Id, typeof(int));
            table.Columns.Add(FirstName, typeof(string));
            table.Columns.Add(LastName, typeof(string));
            table.Columns.Add(Patronymic, typeof(string));
            //table.Columns.Add("RegistrationDate", typeof(DateTime));
            //table.Columns.Add("Email", typeof(string));
            //table.Columns.Add("PhoneNumber", typeof(string));
            //table.Columns.Add("Password", typeof(string));
            //table.Columns.Add("Role", typeof(int));
            //table.Columns.Add("CityId", typeof(int));
            //table.Columns.Add("IsDeleted", typeof(bool));
            //table.Columns.Add("BirthDay", typeof(DateTime));

            for (var rowId = countStart; rowId <= countEnd; rowId++)
            {
                int percent = countEnd / 100 == 0 ? 1 : countEnd / 100;
                if (rowId % percent == 0)
                {
                    Console.WriteLine($"Leads done: {100 * rowId / countEnd}%");
                }

                int idFIO = random.Next(0, FIOsData.Count);

                table.Rows.Add
                (
                    rowId,
                    FIOsData[idFIO][FirstName],
                    FIOsData[idFIO][LastName],
                    FIOsData[idFIO][Patronymic]
                );
            }


            
        }
    }
}