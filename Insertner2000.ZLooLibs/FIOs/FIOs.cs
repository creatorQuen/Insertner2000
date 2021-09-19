using System.Collections.Generic;
using System.Linq;

namespace Insertner2000.ZLooLibs
{
    public partial struct FIOs
    {
        private static IReadOnlyDictionary<int, IReadOnlyDictionary<string, string>>? FIOsData;
        private static readonly List<string> FirstNamesMale = CreateFirstNamesMale();
        private static readonly List<string> FirstNamesFemale = CreateFirstNamesFemale();
        private static readonly List<string> LastNames = CreateLastNames();
        private static readonly List<string> Patronymics = CreatePatronymics();

        private const string EndingFormat = "{0}{1}";

        private const string EndingIch = "ich";
        private const string EndingNa = "na";
        private const string EndingA = "a";

        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string Patronymic = "Patronimic";
        private static int _countFIOs = 0;
        

        public static void GetFIOs(out IReadOnlyDictionary<int, IReadOnlyDictionary<string, string>> FIOs)
        {
            FIOs = FIOsData ?? SetFIOs();
        }

        private static IReadOnlyDictionary<int, IReadOnlyDictionary<string, string>> SetFIOs()
        {
            SetFIOsMale(out IReadOnlyDictionary<int, IReadOnlyDictionary<string, string>> males);
            SetFIOsFemale(out IReadOnlyDictionary<int, IReadOnlyDictionary<string, string>> females);
            return females.Concat(males).ToDictionary(x => x.Key, x => x.Value);
        }

        private static void SetFIOsMale(out IReadOnlyDictionary<int, IReadOnlyDictionary<string, string>> males)
        {
            IReadOnlyList<string> lastNames = LastNames;
            IReadOnlyList<string> firstNames = FirstNamesMale;
            IReadOnlyList<string> patronymics = Patronymics.Select(patronymic => string.Format(EndingFormat, patronymic, EndingIch)).ToList();
            GenerateFIO(lastNames, firstNames, patronymics, out Dictionary<int, IReadOnlyDictionary<string, string>> FIOs);
            males = FIOs;
        }

        private static void SetFIOsFemale(out IReadOnlyDictionary<int, IReadOnlyDictionary<string, string>> females)
        {
            IReadOnlyList<string> lastNames = LastNames.Select(lastNames => string.Format(EndingFormat, lastNames, EndingA)).ToList();
            IReadOnlyList<string> firstNames = FirstNamesFemale;
            IReadOnlyList<string> patronymics = Patronymics.Select(patronymic => string.Format(EndingFormat, patronymic, EndingNa)).ToList();
            GenerateFIO(lastNames, firstNames, patronymics, out Dictionary<int, IReadOnlyDictionary<string, string>> FIOs);
            females = FIOs;
        }

        private static void GenerateFIO(IReadOnlyList<string> lastNames, IReadOnlyList<string> firstNames, IReadOnlyList<string> patronymics, out Dictionary<int, IReadOnlyDictionary<string, string>> FIOs)
        {
            FIOs =
            (
                from lastName in lastNames
                from firstName in firstNames
                from patronymic in patronymics
                select CreateDictionaryFIO(lastName, firstName, patronymic)
            ).ToDictionary(_ => _countFIOs++);
        }

        private static IReadOnlyDictionary<string, string> CreateDictionaryFIO(string lastName, string firstName, string patronymic)
        {
            return new Dictionary<string, string>
            {
                { LastName, lastName },
                { FirstName, firstName },
                { Patronymic, patronymic }
            };
        }
    }
}