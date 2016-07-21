using $safeprojectname$.Contracts;

namespace $safeprojectname$
{
    class User : AppModel, IUser
    {
        public User(string username)
        {
            Username = username;
        }

        public string Username { get; }
    }
}