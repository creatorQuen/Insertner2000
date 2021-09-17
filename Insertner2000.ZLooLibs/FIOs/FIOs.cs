using System.Collections.Generic;
using System.Linq;

namespace Insertner2000.ZLooLibs
{
    public partial struct FIOs
    {
        private static List<string> _firstNamesMale = CreateFirstNamesMale();
        private static List<string> _firstNamesFemale = CreateFirstNamesFemale();
        private static List<string> _lastNames = CreateLastNames();
        private static List<string> _patronymics = CreatePatronymics();

        private const string EndingFormat = "{0}{1}";

        private const string EndingIch = "ich";
        private const string EndingNa = "na";
        private const string EndingA = "a";


        public static void GetFIOsMale(out IReadOnlyDictionary<string, IReadOnlyList<string>> males)
        {
            IReadOnlyList<string> patronimics = _patronymics.Select(patronymic => string.Format(EndingFormat, patronymic, EndingIch)).ToList();
            Dictionary<string, IReadOnlyList<string>> male = new()
            {
                { nameof(_firstNamesMale), _firstNamesMale },
                { nameof(_lastNames), _lastNames },
                { nameof(_patronymics), patronimics }
            };
            males = male;
        }

        public static void GetFIOsFemale(out IReadOnlyDictionary<string, IReadOnlyList<string>> females)
        {
            IReadOnlyList<string> lastNames = _lastNames.Select(lastNames => string.Format(EndingFormat, lastNames, EndingA)).ToList();
            IReadOnlyList<string> patronimics = _patronymics.Select(patronymic => string.Format(EndingFormat, patronymic, EndingNa)).ToList();
            Dictionary<string, IReadOnlyList<string>> female = new()
            {
                { nameof(_firstNamesFemale), _firstNamesFemale },
                { nameof(_lastNames), lastNames },
                { nameof(_patronymics), patronimics }
            };
            females = female;
        }


        private static List<string> CreateFirstNamesMale() =>
        new()
        {
            Aleksandr,
            Alexsey,
            Anton,
            Andrey,
            Artem,
            Artur,
            Boris,
            Brown,
            Evgeniy,
            Maksim,
            Kirill,
            Sergey,
            Peter,
            Stanislav,
            Stefan,
            Timur,
            Vadim,
            Valentin,
            Vladimir,
            Viktor
        };
        
        private static List<string> CreateFirstNamesFemale() =>
        new()
        {
            Aleksandra,
            Alla,
            Anastasia,
            Anna,
            Alina,
            Alissa,
            Ekaterina,
            Elizabeth,
            Kristina,
            Margarita,
            Karina,
            Maria,
            Irina,
            Oksana,
            Nina,
            Polina,
            Rosa,
            Snezhana,
            Susan,
            Vera
        };

        private static List<string> CreateLastNames() =>
        new()
        {
            Tolstov,
            Arkanov,
            Harlamov,
            Borisov,
            Savin,
            Fadeyev,
            Kozlov,
            Orehov,
            Chaikin,
            Glebov,
            Lysenko,
            Agunin,
            Smirnov,
            Kotov,
            Prohorov,
            Filimonov,
            Leontyev,
            Haritonov,
            Sviridov,
            Parfyonov,
            Goldberg,
            Kharlamov,
            Pavlov,
            Teterin,
            Trafinov,
            Akhmedov,
            Ilyin,
            Zubkov,
            Peredelkin,
            Tkachuk,
            Shmidt,
            Golubev,
            Vavilov,
            Gogolev,
            Dorohov,
            Dedov,
            Demin,
            Zharov,
            Zorin,
            Kuzmin,
            Vlasov
        };

        private static List<string> CreatePatronymics() =>
        new()
        {
            Aleksandrov,
            Alekseyev,
            Anatolyev,
            Andreyev,
            Antonov,
            Denisov,
            Dmitriye,
            Ernestov,
            Evgenyev,
            Fedorov,
            Filippov,
            Gennadyev,
            Georgiyev,
            Igorev,
            Innokentyev,
            Ivanov,
            Karpov,
            Kirillov,
            Konstantinov,
            Leonidov,
            Leybov,
            Maksimov,
            Maratov,
            Mironov,
            Nikolayev,
            Olegov,
            Petrov,
            Platonov,
            Rodionov,
            Ruslanov,
            Rustamov,
            Safronov,
            Sergeyev,
            Stanislavov,
            Timofeyev,
            Vasilyev,
            Viktorov,
            Yuryev
        };
    }
}