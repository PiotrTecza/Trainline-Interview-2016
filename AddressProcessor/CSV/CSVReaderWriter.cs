using System;
using AddressProcessing.Wrappers;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */

    /* 
     * Please see SolutionDescription.txt attached
     */

    [Obsolete("Use CSVReader or CSVWriter instead")]
    public class CSVReaderWriter
    {
        private readonly ICSVReader csvReader;
        private readonly ICSVWriter csvWriter;

        public CSVReaderWriter()
        {
            IFileInfo fileInfo = new FileInfoWrapper();
            csvReader = new CSVReader(fileInfo);
            csvWriter = new CSVWriter(fileInfo);
        }

        public CSVReaderWriter(ICSVReader csvReader, ICSVWriter csvWriter)
        {
            this.csvReader = csvReader;
            this.csvWriter = csvWriter;
        }

        [Flags]
        public enum Mode { Read = 1, Write = 2 };

        public void Open(string fileName, Mode mode)
        {
            if (mode == Mode.Read)
            {
                csvReader.Open(fileName);
            }
            else if (mode == Mode.Write)
            {
                csvWriter.Open(fileName);
            }
            else
            {
                throw new Exception("Unknown file mode for " + fileName);
            }
        }

        public void Write(params string[] columns)
        {
            csvWriter.Write(columns);
        }

        public bool Read(string column1, string column2)
        {
            return csvReader.Read(out column1, out column2);
        }

        public bool Read(out string column1, out string column2)
        {
            return csvReader.Read(out column1, out column2);
        }

        public void Close()
        {
            csvWriter?.Close();
            csvReader?.Close();
        }
    }
}
