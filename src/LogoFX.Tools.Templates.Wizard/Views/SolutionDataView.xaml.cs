using System.Windows;
using System.Windows.Controls;
using LogoFX.Tools.Templates.Wizard.ViewModel;

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

        private void SolutionDataView_OnLoaded(object sender, RoutedEventArgs e)
        {
            ((SolutionDataViewModel) DataContext).OnViewLoaded();
        }
    }
}
