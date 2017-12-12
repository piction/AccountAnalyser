using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountAnalyser
{
    public static class BackupTransactionsRepoWriter 
    {
        public static void WriteToCsv(TransactionInfoRepo repo, string csv)
        {
            if (repo == null || repo.transactions.Count == 0) return;
            using (StreamWriter writer = new StreamWriter(csv))
            {
                StringBuilder line = new StringBuilder();
                // write header
                line.Append("Ref. v/d verrichting,Beschrijving,Bedrag v/d verrichting,");
                line.Append("Datum v. verrichting,FactuurDatum,Rekening tegenpartij,Naam v/d tegenpartij :,");
                line.Append("Mededeling 1 :,Surplas Info,linkedFile,Is geboekt");

                if (repo.tagKeys.Count > 0)
                {
                    line.Append("," + string.Join(",", repo.tagKeys));
                }

                writer.WriteLine(line.ToString());
                // write contents 
                foreach ( var trans in repo.transactions.Values)
                {
                    line.Clear();
                    line.Append($"{trans.refId},{trans.type},{trans.amount},");
                    line.Append($"{trans.date.ToString(@"d-MM-yyyy")},{trans?.invoiceDate?.ToString(@"d-MM-yyyy")??""},{trans.account},{trans.accountName},");
                    line.Append($"{trans.description},{trans.surplasDescription},{string.Join(" ",trans.linkedFile)},");
                    line.Append($"{trans.IsProcessed}");
                    if (repo.tagKeys.Count > 0)
                    {
                        foreach ( var k in repo.tagKeys)
                        {
                            line.Append("," + (trans.tags.ContainsKey(k) ? trans.tags[k].ToString() : "0"));
                        }
                    }
                    writer.WriteLine(line.ToString());
                }

            }
        }
    }
}
