using System;
using System.IO;

namespace AddressProcessing.Wrappers
{
    public class FileInfoWrapper : IFileInfo
    {
        public FileInfo FileInfoInstance { get; private set; }

        public void Initialize(string fileName)
        {
            FileInfoInstance = new FileInfo(fileName);
        }

        public IStreamReader OpenText()
        {
            return new StreamReaderWrapper(FileInfoInstance.OpenText());
        }

        public IStreamWriter CreateText()
        {
            return new StreamWriterWrapper(FileInfoInstance.CreateText());
        }
    }
}