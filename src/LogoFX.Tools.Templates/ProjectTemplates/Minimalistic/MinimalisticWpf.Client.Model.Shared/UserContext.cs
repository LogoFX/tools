using $saferootprojectname$.Client.Model.Contracts;

namespace $safeprojectname$
{
    public static class UserContext
    {
        private static IUser _currentUser;

        public static IUser Current
        {
            get { return _currentUser; }
            set
            {
                if (_currentUser == value)
                {
                    return;
                }

                _currentUser = value;
            }
        }
    }
}
