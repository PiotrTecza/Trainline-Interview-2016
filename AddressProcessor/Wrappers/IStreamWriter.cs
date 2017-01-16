namespace AddressProcessing.Wrappers
{
    public interface IStreamWriter
    {
        void WriteLine(string value);
        void Close();

    }
}
