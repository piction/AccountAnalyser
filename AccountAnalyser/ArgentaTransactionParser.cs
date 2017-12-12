using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AccountAnalyser
{
    public class ArgentaTransactionParser : ICsvLineObjectHandler
    {
        public List<TransactionInfo> transactions = new List<TransactionInfo>();

        public void HandleLine(Dictionary<string, string> line)
        {
            CultureInfo french = new CultureInfo("nl-NL"); // use this to have the comma as decimal seperator
            try
            {
                TransactionInfo i = new TransactionInfo();
                string amountToParse;
                line.TryGetValue("Bedrag v/d verrichting", out amountToParse);
                i.amount = double.Parse(amountToParse, french); 

                string dateToParse;
                line.TryGetValue("Datum v. verrichting", out dateToParse);
                i.date = DateTime.Parse(dateToParse);
                line.TryGetValue("Rekening tegenpartij", out i.account);
                line.TryGetValue("Naam v/d tegenpartij :", out i.accountName);
                line.TryGetValue("Mededeling 1 :", out i.description);
                line.TryGetValue("Beschrijving", out i.type);
                line.TryGetValue("Ref. v/d verrichting", out i.refId);

                i.description = i.description.Replace(',', ' ');
                i.accountName = i.accountName.Replace(',', ' ');
                i.type = i.type.Replace(',', ' ');

                transactions.Add(i);
            }
            catch (Exception e)
            {
                StringBuilder lineString = new StringBuilder();
                foreach (var l in line)
                {
                    lineString.Append($"[{l.Key}]: {l.Value}");
                }
                Console.WriteLine("Failed to parse : " + lineString);
                Console.WriteLine(e.Message);
            }
           
        }
    }


}
