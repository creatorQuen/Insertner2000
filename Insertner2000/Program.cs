using System;
using System.Diagnostics;

namespace Insertner2000
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            ConfigurationForTables tables = new ConfigurationForTables();

            //tables.CitiesDataBase();
            tables.LeadsDataBase();
            //tables.AccountsDataBase();

            stopWatch.Stop();
            Console.WriteLine();
            Console.WriteLine($"Total time in minutes:{TimeSpan.FromMilliseconds(stopWatch.ElapsedMilliseconds).Minutes}");
            Console.WriteLine($"      time in seconds:{TimeSpan.FromMilliseconds(stopWatch.ElapsedMilliseconds).Seconds}");
            Console.WriteLine();
            Console.WriteLine("Done. Press any key to exit.");
            Console.ReadKey();

        }
    }
}
