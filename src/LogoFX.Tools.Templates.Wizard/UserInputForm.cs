using System.Windows.Forms;

namespace LogoFX.Tools.Templates.Wizard
{
    public partial class UserInputForm : Form
    {
        public UserInputForm()
        {
            InitializeComponent();
        }

        public void DataToScreen(WizardData wizardData)
        {
            checkBoxFake.Checked = wizardData.FakeData;
            checkBoxTests.Checked = wizardData.Tests;
        }

        public void ScreenToData(WizardData wizardData)
        {
            wizardData.FakeData = checkBoxFake.Checked;
            wizardData.Tests = checkBoxTests.Checked;
        }
    }
}
