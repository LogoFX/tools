using JetBrains.Annotations;
using $safeprojectname$.Contracts;

namespace $safeprojectname$
{
    internal sealed class User : AppModel, IUser
    {
        public User(string username)
        {
            Username = username;
        }

        public string Username { get; private set; }
    }
}