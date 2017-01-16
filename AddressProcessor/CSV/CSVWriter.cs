using AddressProcessing.Wrappers;

namespace AddressProcessing.CSV
{
    public class CSVWriter : ICSVWriter
    {
        private IStreamWriter streamWriter;
        private IFileInfo fileInfo;

        public CSVWriter(IFileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }

        public void Open(string fileName)
        {
            fileInfo.Initialize(fileName);
            streamWriter = fileInfo.CreateText();
        }

        public void Write(params string[] columns)
        {
            var output = string.Join("\t", columns);
            streamWriter.WriteLine(output);
        }

        public void Close()
        {
            streamWriter?.Close();
        }
    }
}
