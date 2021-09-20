using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Insertner2000.ZLooMod
{
    public class Account
    {
        public void CreateAccounts(string connectionForLeadAccount, string connectionForTransaction)
        {
            var LeadId = ConfigurationForTables.LeadIdStart;
            var dataSet = new DataSet();
            var table = dataSet.Tables.Add("ttmpData");

            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("LeadId", typeof(int));
            table.Columns.Add("Currency", typeof(int));
            table.Columns.Add("CreatedOn", typeof(DateTime));
            table.Columns.Add("Closed", typeof(DateTime));
            table.Columns.Add("IsDeleted", typeof(bool));

            var difference = ConfigurationForTables.AccountIdEnd - ConfigurationForTables.AccountIdStart;
            var percent = 0;

            using (var progress = new ProgressBar(/*В место коментария можно указать колличество решеточек*/))
            {
                for (var id = ConfigurationForTables.AccountIdStart; id <= ConfigurationForTables.AccountIdEnd; LeadId++)
                {
                   
                }
            }
        }
    }
}