using System.IO;

namespace AddressProcessing.Wrappers
{
    public class StreamReaderWrapper : IStreamReader
    {
        public StreamReader StreamReaderInstance { get; }

        public StreamReaderWrapper(StreamReader reader)
        {
            StreamReaderInstance = reader;
        }

        public void Close()
        {
            StreamReaderInstance.Close();
        }

        public string ReadLine()
        {
            return StreamReaderInstance.ReadLine();
        }
    }
}
