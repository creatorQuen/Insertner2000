using System.Collections.Generic;

namespace Insertner2000.ZLooLibs
{
    public partial struct Emails
    {
        private const string Com = "com";
        private const string Ru = "ru";
        private const string Uk = "uk";
        private const string Az = "az";
        private const string Org = "org";
        private const string Biz = "biz";
        private const string Nf = "nf";
        private const string Fr = "fr";
        private const string De = "de";
        private const string Dj = "dj";
        private const string Cx = "cx";
        private const string Net = "net";
        private const string Me = "me";
        private const string Info = "info";
        private const string Co = "co";
        private const string La = "la";

        private static List<string> CreateDomains() =>
        new()
        {
            Com,
            Ru,
            Uk,
            Az,
            Org,
            Biz,
            Nf,
            Fr,
            De,
            Dj,
            Cx,
            Net,
            Me,
            Info,
            Co,
            La
        };
    }
}