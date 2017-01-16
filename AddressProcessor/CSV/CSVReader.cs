using System.Linq;
using AddressProcessing.Wrappers;

namespace AddressProcessing.CSV
{
    public class CSVReader : ICSVReader
    {
        private IStreamReader streamReader;
        private IFileInfo fileInfo;

        public CSVReader(IFileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }

        public void Open(string fileName)
        {
            fileInfo.Initialize(fileName);
            streamReader = fileInfo.OpenText();
        }

        public bool Read(out string column1, out string column2)
        {
            var line = streamReader.ReadLine();
            var columns = line?.Split('\t');
            column1 = columns?.ElementAtOrDefault(0);
            column2 = columns?.ElementAtOrDefault(1);

            return line != null;
        }

        public void Close()
        {
            streamReader?.Close();
        }
    }
}

