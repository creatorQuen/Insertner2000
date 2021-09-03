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
            //tables.LeadsDataBase();
            tables.AccountsDataBase();

            tables.TransactionsDataBase();

            stopWatch.Stop();
            Console.WriteLine();
            Console.WriteLine($"Total time hour:{TimeSpan.FromMilliseconds(stopWatch.ElapsedMilliseconds).Hours}");
            Console.WriteLine($"      time minutes:{TimeSpan.FromMilliseconds(stopWatch.ElapsedMilliseconds).Minutes}");
            Console.WriteLine($"      time seconds:{TimeSpan.FromMilliseconds(stopWatch.ElapsedMilliseconds).Seconds}");
            Console.WriteLine();
            Console.WriteLine("Done. Press any key to exit.");
            Console.ReadKey();

        }
    }
}
