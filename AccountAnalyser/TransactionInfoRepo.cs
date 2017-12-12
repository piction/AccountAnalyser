using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountAnalyser
{
    public class TransactionInfoRepo
    {
        public Dictionary<string, TransactionInfo> transactions = new Dictionary<string, TransactionInfo>();
        public HashSet<string> tagKeys = new HashSet<string>();

        public TransactionInfoRepo(IEnumerable<TransactionInfo> transactionList) 
        {
            foreach ( var t in transactionList)
            {
                try
                {
                    this.transactions.Add(t.refId, t);
                }
                catch( Exception e )
                {
                    string msg = e.Message;
                }

                foreach (var tagKey in t.tags.Keys)
                {
                    if (!tagKeys.Contains(tagKey))
                        tagKeys.Add(tagKey);
                }
                
            }

        }

        public void MergeTransactions (IEnumerable<TransactionInfo> other )
        {
            foreach ( var transInfo in other)
            {
                if ( transInfo.refId == null)
                {
                    throw new ArgumentException("TODO fix: A transactionInfo without refId can not be merged!");
                    // swallow later and see and log , a log analyser can dan show warnings like this 
                }

                if( !transactions.ContainsKey(transInfo.refId))
                {
                    transactions[transInfo.refId] = transInfo;
                    foreach (var tagKey in transInfo.tags.Keys)
                    {
                        if (!tagKeys.Contains(tagKey))
                            tagKeys.Add(tagKey);
                    }
                }
            }
            // reorder dictionary 
            transactions = transactions.OrderBy(x => x.Value).ToDictionary(x=> x.Key,y=> y.Value);
        }

        public void Update(TransactionInfo info)
        {
            var found = transactions.FirstOrDefault(x => x.Key.Equals(info.refId)).Key;
            if(found != null)
            {
                transactions[found] = info;
            }
        }
    }
}
