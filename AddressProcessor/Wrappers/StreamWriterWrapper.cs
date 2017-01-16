using System.IO;

namespace AddressProcessing.Wrappers
{
    public class StreamWriterWrapper : IStreamWriter
    {
        public StreamWriter StreamWriterInstance { get; }

        public StreamWriterWrapper(StreamWriter writer)
        {
            StreamWriterInstance = writer;
        }

        public void WriteLine(string value)
        {
            StreamWriterInstance.WriteLine(value);
        }

        public void Close()
        {
            StreamWriterInstance.Close();
        }
    }
}
