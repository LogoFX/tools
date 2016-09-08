using JetBrains.Annotations;
using MinimalisticWpf.Client.Model.Contracts;

namespace MinimalisticWpf.Client.Model
{
    internal sealed class User : AppModel, IUser
    {
        public User(string username)
        {
            Username = username;
        }

        public string Username { get; }
    }
}