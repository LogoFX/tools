using System.Windows;
using System.Windows.Controls;

namespace LogoFX.Tools.Templates.Wizard.Views
{
    /// <summary>
    /// Interaction logic for SolutionDataView.xaml
    /// </summary>
    public partial class SolutionDataView : UserControl
    {
        public SolutionDataView()
        {
            InitializeComponent();
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void SolutionDataView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (ComboBox.SelectedItem == null &&
                ComboBox.Items.Count > 0)
            {
                ComboBox.SelectedIndex = 0;
            }
        }
    }
}
