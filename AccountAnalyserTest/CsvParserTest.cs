using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AccountAnalyser;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

namespace AccountAnalyserTest
{

    public class TestCsvHandler : ICsvLineObjectHandler
    {
        public TestCsvHandler(string fileName)
        {
            writer = new StreamWriter(fileName);
        }
        private StreamWriter writer;

        public void HandleLine(Dictionary<string, string> line)
        {
            foreach ( var l in line)
            {
                writer.Write($"[{l.Key}]: {l.Value}");
            }
            writer.Write(Environment.NewLine);
        }

        public void Finish()
        {
            writer.Dispose();
        }


        
    }

    [TestClass]
    public class CsvParserTest
    {


        [TestMethod]
        public void TestMethod1()
        {
            string testFile = @"C:\Users\Trikkie\Documents\Visual Studio 2017\Projects\AccountAnalyser\AccountAnalyserTest\TestData\ExampleAccount.csv";

            //var handler = new TestCsvHandler("testFile.txt");
            var handler = new ArgentaTransactionParser();
            var sut = new CsvParser(';', handler);
            using (StreamReader reader = new StreamReader(testFile))
            {
                reader.ReadLine();
                sut.Parse(reader, true);
            }
            //handler.Finish();
        }

        [TestMethod]
        public void ShouldPareOverviewCsv()
        {
            string testFile = @"C:\Users\Trikkie\Documents\Visual Studio 2017\Projects\AccountAnalyser\AccountAnalyserTest\TestData\ExampleOverviewData.csv";

            var handler = new BackupTransactionParser();
            var sut = new CsvParser(',', handler);
            using (StreamReader reader = new StreamReader(testFile))
            {
                sut.Parse(reader, true);
            }
        }


        
        [TestMethod]
        public void Trying()
        {
            string amountToParse = "-10.267,55";
            CultureInfo french = new CultureInfo("nl-NL"); // use this to have the comma as decimal seperator
            double amount = double.Parse(amountToParse, french); // todo fix comma stuff
            Console.WriteLine(amount);
            //using (StreamWriter writer = new StreamWriter("testfile.txt"))
            //{
            //    writer.WriteLine("CULTURE ISO ISO WIN DISPLAYNAME                              ENGLISHNAME");


            //    foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.NeutralCultures))
            //    {

            //        writer.Write("{0,-7}", ci.Name);
            //        writer.Write(" {0,-3}", ci.TwoLetterISOLanguageName);
            //        writer.Write(" {0,-3}", ci.ThreeLetterISOLanguageName);
            //        writer.Write(" {0,-3}", ci.ThreeLetterWindowsLanguageName);
            //        writer.Write(" {0,-40}", ci.DisplayName);
            //        writer.WriteLine(" {0,-40}", ci.EnglishName);

            //    }
            //}
        }



    
    }



}
