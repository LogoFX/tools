using System.Windows;

namespace LogoFX.Tools.TemplateGenerator.Shell.Controls
{
    /// <summary>
    /// Interaction logic for BusyIndicator.xaml
    /// </summary>
    public partial class BusyIndicator
    {
        public BusyIndicator()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;
        }

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register(
                "IsBusy",
                typeof(bool),
                typeof(BusyIndicator),
                new PropertyMetadata(
                    false,
                    OnIsBusyChanged));

        private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BusyIndicator) d).OnIsBusyChanged((bool) e.NewValue, (bool) e.OldValue);
        }

        private void OnIsBusyChanged(bool newValue, bool oldValue)
        {
            Visibility = newValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
    }
}
