using System;
using System.Windows;
using System.Windows.Interop;

namespace LogoFX.Tools.Templates.Wizard
{
    public static class WpfServices
    {
        public static T CreateWindow<T>(object data = null)
            where T : Window, new()
        {
            T wnd = new T
            {
                DataContext = data
            };
            return wnd;
        }

        public static void SetWindowOwner(Window window, EnvDTE.Window ownerWindow)
        {
            WindowInteropHelper wih = new WindowInteropHelper(window);
            wih.Owner = new IntPtr(ownerWindow.HWnd);
        }
    }
}