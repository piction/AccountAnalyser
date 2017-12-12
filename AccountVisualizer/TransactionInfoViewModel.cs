using AccountAnalyser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountVisualizer
{
    public class TransactionInfoViewModel
    {
        public TransactionInfoViewModel() { }
        public TransactionInfoViewModel(TransactionInfo info)
        {
            this.info = info;
        }

        public DateTime Date => info.date;
        public DateTime? InvoiceDate => info.invoiceDate;
        public bool IsProcessed => info.IsProcessed;
        public string Account => info.account;
        public string AccountName => info.accountName;
        public double Amount => info.amount;
        public string Id => info.refId;
        public string Category
        {
            get
            {
                var c = info.tags
                .Where(x => x.Value != 0)
                .Select(x => $"{x.Key}({Math.Round((100 * x.Value) / info.amount)}%)");
                return String.Join(" / ", c);
            }
        }
        public string Message => info.description;
        public string SurplasInfo => info.surplasDescription;
        public string LinkedFile => info.linkedFile;
        public bool NotSaved { get; set; } = false;
        public TransactionInfo info;

        public bool IsIncomplete(bool shouldHaveLinkedFile = false)
        {
            var t = this.info.tags.Sum(x => x.Value);
            bool sumOk =Math.Abs(this.info.amount - this.info.tags.Sum(x => x.Value)) > 1e-3;
            bool Ok = sumOk && string.IsNullOrWhiteSpace(info.surplasDescription);
            if ( !shouldHaveLinkedFile)
                return sumOk;
            return sumOk && string.IsNullOrWhiteSpace(info.linkedFile);
        }
    }
}
