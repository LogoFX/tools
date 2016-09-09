using System.Threading.Tasks;

namespace $safeprojectname$
{
    public interface ILoginProvider
    {
        Task Login(string username, string password);
    }
}
