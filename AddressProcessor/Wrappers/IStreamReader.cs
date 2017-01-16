namespace AddressProcessing.Wrappers
{
    public interface IStreamReader 
    {
        void Close();
        string ReadLine();
    }
}