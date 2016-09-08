namespace MinimalisticWpf.Client.Data.Contracts.Providers
{
    public interface ILoginProvider
    {
        void Login(string username, string password);
    }
}