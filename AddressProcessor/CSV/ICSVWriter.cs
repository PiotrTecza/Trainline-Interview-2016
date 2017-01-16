namespace AddressProcessing.CSV
{
    public interface ICSVWriter
    {
        void Open(string fileName);
        void Write(params string[] columns);
        void Close();
    }
}