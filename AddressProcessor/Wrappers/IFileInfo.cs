namespace AddressProcessing.Wrappers
{
    public interface IFileInfo
    {
        void Initialize(string fileName);
        IStreamReader OpenText();
        IStreamWriter CreateText();
    }
}