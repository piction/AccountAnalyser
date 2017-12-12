using AccountAnalyser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountAnalyserTest
{
    [TestClass]
    public class TransactinInfoRepoTest
    {
        [TestMethod]
        public void ShouldMergeTransactionLists()
        {
            SortedList<string, TransactionInfo> mySortedList = new SortedList<string, TransactionInfo>();


            string testFile = @"C:\Users\Trikkie\Documents\Visual Studio 2017\Projects\AccountAnalyser\AccountAnalyserTest\TestData\ExampleAccount.csv";

            var handler = new ArgentaTransactionParser();
            var parser = new CsvParser(';', handler);
            using (StreamReader reader = new StreamReader(testFile))
            {
                reader.ReadLine();
                parser.Parse(reader, true);
            }
            var first6 = handler.transactions.Take(6);
            var semiOverlap = handler.transactions.Skip(3).Take(5);

            var sut = new TransactionInfoRepo(first6);
            sut.MergeTransactions(semiOverlap);

            Assert.IsTrue(sut.transactions.Count == 8);

                           

        }



        [TestMethod]
        public void ShouldWriteTransactionRepo()
        {
            //string testFile = @"C:\Users\Trikkie\Documents\Visual Studio 2017\Projects\AccountAnalyser\AccountAnalyserTest\TestData\ExampleAccount.csv";
            string testFile = @"C:\Users\Trikkie\Documents\Visual Studio 2017\Projects\AccountAnalyser\AccountAnalyserTest\TestData\ExampleOverviewData.csv";
            
            var handler = new BackupTransactionParser();
            var parser = new CsvParser(',', handler);
            using (StreamReader reader = new StreamReader(testFile))
            {
                // reader.ReadLine();
                parser.Parse(reader, true);
            }
            var sut = new TransactionInfoRepo(handler.transactions);

            BackupTransactionsRepoWriter.WriteToCsv(sut,"outputTest.csv");
           
        }

        [TestMethod]
        public void bogus ()
        {
            DateTime nu = DateTime.Now;
            string test = nu.ToString();
            string haha = test;

        }
    }
}
