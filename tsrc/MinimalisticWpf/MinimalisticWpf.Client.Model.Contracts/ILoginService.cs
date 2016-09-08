using System.Threading.Tasks;

namespace MinimalisticWpf.Client.Model.Contracts
{
    /// <summary>
    /// Represents login service
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// Logins asynchronously.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>        
        /// <returns>Task representing login result.</returns>
        Task LoginAsync(string username, string password);
    }
}