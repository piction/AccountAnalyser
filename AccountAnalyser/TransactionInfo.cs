using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountAnalyser
{
    public class TransactionInfo : IComparable 
    {
        public string refId;
        public DateTime date;
        public DateTime? invoiceDate;
        public bool IsProcessed;
        public string type;
        public double amount;
        public string account;
        public string accountName;
        public string description;
        public string surplasDescription;
        public Dictionary<string,double> tags = new Dictionary<string, double>();
        public string linkedFile = "";

        public int CompareTo(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("Can not compare TransactionInfo with null");

            var other = (TransactionInfo)obj;
            if (this.date > other.date)
                return 1;
            if (this.date < other.date)
                return -1;
            else
                return 0;
        }
    }


}
