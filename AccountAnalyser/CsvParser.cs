using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountAnalyser
{

    public interface ICsvLineObjectHandler
    {
        void HandleLine(Dictionary<string, string> line);
    }



    public class CsvParser
    {
        public CsvParser(char seperator , ICsvLineObjectHandler lineObjectHandler)
        {
            this.separator = seperator;
            this._lineObjectHandler = lineObjectHandler;
        }

        private ICsvLineObjectHandler _lineObjectHandler;
        public readonly char separator;
        public List<string> Headers { get; private set; }

        public void Parse(StreamReader reader, bool hasHeader = false)
        {
            // read first line as header
            Headers = hasHeader ? reader.ReadLine().Split(separator).ToList() : new List<string>();

            // read content of stream
            string line;
            while ( (line = reader.ReadLine()) != null)
            {
                var lineObjects = new Dictionary<string, string>();

                var lineParts = line.Split(separator);
                int totalFoundlineParts = lineParts.Count();
                for (int i = 0; i < totalFoundlineParts; i++)
                {
                    // Add a column-name if not known
                    if ( i == Headers.Count)
                    {
                        Headers.Add($"Column {i}");
                    }
                    lineObjects[Headers[i]] = lineParts[i];
                }
                // Handle full line
                _lineObjectHandler.HandleLine(lineObjects);
            }
        }
    }
}
