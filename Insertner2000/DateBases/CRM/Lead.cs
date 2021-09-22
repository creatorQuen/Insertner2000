using Insertner2000.Entity;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Insertner2000.Tables
{
    public class Lead
    {
        private const string _dateFormat = "dd.MM.yyyy HH:mm:ss.fffffff";
        private const int _daysPearTwoYear = 730;
        private const int _daysPearYear = 365;
        private Random _random = new Random();

        public void CreateLeads(int countStart, int countEnd, string connectionString)
        {
            using (SqlConnection _connection = new SqlConnection(connectionString))
            {
                Console.WriteLine("Starting..");

                var lengthRandomString = 6;
                var countCity = Enum.GetNames(typeof(CityList)).Length;
                var countFirstName = Enum.GetNames(typeof(FirstName)).Length;
                var countLastName = Enum.GetNames(typeof(LastName)).Length;
                var countEmail = Enum.GetNames(typeof(Email_EndString)).Length;
                var middleFirstNameCount = countFirstName / 2;
                var time = DateTime.Now;
                var birthDay = new DateTime(1980, 01, 01);

                var dataSet = new DataSet();

                Console.WriteLine("Creating datatable..");
                var table = dataSet.Tables.Add("MockLead");

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
                table.Columns.Add("BirthDate", typeof(DateTime));
                table.Columns.Add("BirthYear", typeof(int));
                table.Columns.Add("BirthMonth", typeof(int));
                table.Columns.Add("BirthDay", typeof(int));

                Console.WriteLine("Adding data to dataTable [Leads]");

                for (var intRow = countStart; intRow <= countEnd; intRow++)
                {
                    if (intRow % (countEnd / 100) == 0)
                    {
                        Console.WriteLine($"Leads done: {100 * intRow / countEnd}%");
                    }

                    var tmpFirstName = (_random.Next(1, countFirstName + 1));
                    var tmpLastName = (_random.Next(1, countLastName + 1));

                    var randomString = GenerateStringNotThreadSafe(lengthRandomString);
                    var randomCreateDate = time.AddDays(_random.Next(-_daysPearTwoYear, -_daysPearYear)).ToString(_dateFormat);

                    var phoneStart = _random.Next(1, 100001).ToString();
                    var phoneEnd = _random.Next(1, 100001).ToString();
                    var phoneString = phoneStart + phoneEnd;

                    var emailEnd = (Email_EndString)(_random.Next(1, countEmail));
                    var emailDomain = (Email_Domain)(_random.Next(1, countEmail));
                    var emailString = ((FirstName)tmpFirstName).ToString() + ((LastName)tmpLastName).ToString() + randomString + phoneStart + "@" + emailEnd + "." + emailDomain;

                    var numberForBirthDay = _random.Next(-5000, 8200);
                    var roleId = _random.Next(1, 4);
                    var cityId = _random.Next(1, countCity + 1);
                    var boolId = _random.Next(0, 2);
                    
                    var birtday = birthDay.AddDays(numberForBirthDay);

                    table.Rows.Add(
                        intRow,
                        (FirstName)tmpFirstName,
                        tmpFirstName <= middleFirstNameCount ? $"{(LastName)(tmpLastName)}" + nameof(Name_End.a) : $"{(LastName)(tmpLastName)}",
                        tmpFirstName <= middleFirstNameCount ? $"{(Patronomic_Begin)(_random.Next(1, countFirstName + 1))}" + nameof(Name_End.na) : $"{(Patronomic_Begin)(_random.Next(1, countFirstName + 1))}" + nameof(Name_End.ich),
                        randomCreateDate,
                        emailString,
                        phoneString,
                        $"{randomString + phoneStart}",
                        roleId,
                        cityId,
                        boolId,
                        birtday,
                        birtday.Year,
                        birtday.Month,
                        birtday.Day
                        );
                }

                Console.WriteLine("Open database..");
                var bulkCopy = new SqlBulkCopy(_connection);

                _connection.Open();
                bulkCopy.DestinationTableName = ConfigurationForTables.LeadTable;
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

        public string GenerateStringNotThreadSafe(int length)
        {
            return GenerateString(_random, length);
        }
    }
}