using Insertner2000.Entity;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Insertner2000.Tables
{
    public class Lead
    {
        private string _nameTable = "Lead";
        private const string _dateFormat = "dd.MM.yyyy HH:mm:ss.fffffff";

        public void CreateLeads(int countStart, int countEnd, string connectionString)
        {
            using (SqlConnection _connection = new SqlConnection(connectionString))
            {
                Console.WriteLine("Starting..");

                Random random = new Random();

                int lengthRandomString = 6;
                int countCity = Enum.GetNames(typeof(CityList)).Length;
                int countFirstName = Enum.GetNames(typeof(FirstName)).Length;
                int countLastName = Enum.GetNames(typeof(LastName)).Length;
                int countEmail = Enum.GetNames(typeof(Email_EndString)).Length;
                var middleFirstNameCount = countFirstName / 2;
                var time = DateTime.Now;
                var birthDay = new DateTime(1980, 01, 01);

                DataSet dataSet = new DataSet();

                Console.WriteLine("Creating datatable..");
                DataTable table;
                table = dataSet.Tables.Add("MockLead");
                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("FirstName", typeof(string));
                table.Columns.Add("LastName", typeof(string));
                table.Columns.Add("Patronymic", typeof(string));
                table.Columns.Add("RegistrationDate", typeof(DateTime));
                table.Columns.Add("Email", typeof(string));
                table.Columns.Add("PhoneNumber", typeof(string));
                table.Columns.Add("Password", typeof(string));
                table.Columns.Add("Role", typeof(int));
                table.Columns.Add("CityId", typeof(int));
                table.Columns.Add("IsDeleted", typeof(bool));
                table.Columns.Add("BirthDay", typeof(DateTime));

                Console.WriteLine("Adding data to datatable..");

                for (int intRow = countStart; intRow <= countEnd; intRow++)
                {
                    int tmpFirstName = (random.Next(1, countFirstName + 1));
                    int tmpLastName = (random.Next(1, countLastName + 1));

                    var randomString = GenerateStringNotThreadSafe(lengthRandomString);

                    var phoneStart = random.Next(1, 100001).ToString();
                    var phoneEnd = random.Next(1, 100001).ToString();
                    string phoneString = phoneStart + phoneEnd;

                    var emailEnd = (Email_EndString)(random.Next(1, countEmail));
                    var emailDomain = (Email_Domain)(random.Next(1, countEmail));
                    string emailString = ((FirstName)tmpFirstName).ToString() + ((LastName)tmpLastName).ToString() + randomString + phoneStart + "@" + emailEnd + "." + emailDomain;

                    int nuberForBirthDay = random.Next(-5000, 8200);
                    int roleId = random.Next(1, 4);
                    int cityId = random.Next(1, countCity + 1);
                    int boolId = random.Next(0, 2);
                    
                    table.Rows.Add(
                        intRow,
                        (FirstName)tmpFirstName,
                        tmpFirstName <= middleFirstNameCount ? $"{(LastName)(tmpLastName)}" + nameof(Name_End.a) : $"{(LastName)(tmpLastName)}",
                        tmpFirstName <= middleFirstNameCount ? $"{(Patronomic_Begin)(random.Next(1, countFirstName + 1))}" + nameof(Name_End.na) : $"{(Patronomic_Begin)(random.Next(1, countFirstName + 1))}" + nameof(Name_End.ich),
                         ((DateTime)(time.AddMinutes(intRow))).ToString(_dateFormat),
                        emailString,
                        phoneString,
                        $"{randomString + phoneStart}",
                        roleId,
                        cityId,
                        boolId,
                        birthDay.AddDays(nuberForBirthDay));
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

        public char GenerateChar(Random rng)
        {
            return (char)(rng.Next('a', 'z' + 1));
        }

        public string GenerateString(Random rng, int length)
        {
            char[] letters = new char[length];
            for (int i = 0; i < length; i++)
            {
                letters[i] = GenerateChar(rng);
            }
            return new string(letters);
        }

        private static readonly Random SingleRandom = new Random();

        public string GenerateStringNotThreadSafe(int length)
        {
            return GenerateString(SingleRandom, length);
        }
    }
}
