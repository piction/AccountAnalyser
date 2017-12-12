using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AccountAnalyser
{
    public class BackupTransactionParser : ICsvLineObjectHandler
    { 
        public List<TransactionInfo> transactions = new List<TransactionInfo>();

        public void HandleLine(Dictionary<string, string> line)
        {
            CultureInfo eng = new CultureInfo("en-En"); // use this to have the dot as decimal seperator
            var keysToIgnore = new List<string> { "Valutadatum", "Munt", "Mededeling 2 :", "Column1" };
            try
            {
                TransactionInfo i = new TransactionInfo();

                foreach (var linePart in line)
                {
                    if (keysToIgnore.Contains(linePart.Key )) continue;
                    if (linePart.Key == "Bedrag v/d verrichting")
                    {
                        i.amount = double.Parse(linePart.Value, eng);
                    }
                    else if (linePart.Key == "Datum v. verrichting")
                    {
                        i.date = DateTime.Parse(linePart.Value);
                    }
                    else if (linePart.Key == "FactuurDatum")
                    {
                        if(string.IsNullOrEmpty(linePart.Value))
                        {
                            i.invoiceDate = null;
                        } else
                        {
                            i.invoiceDate = DateTime.Parse(linePart.Value);
                        }
                    }
                    else if (linePart.Key == "Is geboekt")
                    {
                        i.IsProcessed = Convert.ToBoolean(linePart.Value);
                    }
                    else if (linePart.Key == "Rekening tegenpartij")
                    {
                        i.account = linePart.Value;
                    }
                    else if (linePart.Key == "Naam v/d tegenpartij :")
                    {
                        i.accountName = linePart.Value;
                    }
                    else if (linePart.Key == "Mededeling 1 :")
                    {
                        i.description = linePart.Value;
                    }
                    else if (linePart.Key == "Beschrijving")
                    {
                        i.type = linePart.Value;
                    }
                    else if (linePart.Key == "Ref. v/d verrichting")
                    {
                        i.refId = linePart.Value;
                    }
                    else if (linePart.Key == "Surplas Info")
                    {
                        i.surplasDescription = linePart.Value;
                    }
                    else if (linePart.Key == "linkedFile")
                    {
                        var r = (new List<string>(linePart.Value.Split('\\')));
                        i.linkedFile = r.Last();
                        // i.linkedFiles = new List<string>(linePart.Value.Split(' '));
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(linePart.Value))
                            continue;

                        //***** SWITCH OLD NEW ******
                        //i.tags[linePart.Key] = i.amount;
                        try
                        {
                            i.tags[linePart.Key] = double.Parse(linePart.Value, eng);
                        }
                        catch (Exception e)
                        {
                            var t = linePart;
                            Console.WriteLine(e.Message);
                        }
                    }
                }
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
