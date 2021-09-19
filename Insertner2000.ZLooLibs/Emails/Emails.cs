using System.Collections.Generic;

namespace Insertner2000.ZLooLibs
{
    public partial struct Emails
    {
        private const string FormatEmails = "@{0}.{1}";
        private static readonly List<string> EmailsList = CreateEmails();

        public static void GetEmails(out IReadOnlyList<string> emailsList)
        {
            emailsList = EmailsList;
        }

        private static List<string> CreateEmails()
        {
            List<string> domainList = CreateDomains();
            List<string> prefixList = CreatePrefixs();
            List<string> emails = new();
            for (int i = 0; i < prefixList.Count; i++)
            {
                for (int j = 0; j < domainList.Count; j++)
                {
                    string email = string.Format(FormatEmails, prefixList[i], domainList[j]);
                    emails.Add(email);
                }
            }

            return emails;
        }
    }
}