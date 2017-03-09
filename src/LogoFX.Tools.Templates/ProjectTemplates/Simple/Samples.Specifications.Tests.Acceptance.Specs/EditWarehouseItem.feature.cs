// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.1.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace $safeprojectname$
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class EditWarehouseItemFeature : Xunit.IClassFixture<EditWarehouseItemFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "EditWarehouseItem.feature"
#line hidden
        
        public EditWarehouseItemFeature()
        {
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "EditWarehouseItem", "\tIn order to reflect the current warehouse items\' state\r\n\tAs a warehouse manager\r" +
                    "\n\tI want to be able to update warehouse items\' properties", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void SetFixture(EditWarehouseItemFeature.FixtureData fixtureData)
        {
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute(DisplayName="Edit item price")]
        [Xunit.TraitAttribute("FeatureTitle", "EditWarehouseItem")]
        [Xunit.TraitAttribute("Description", "Edit item price")]
        public virtual void EditItemPrice()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Edit item price", ((string[])(null)));
#line 6
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Kind",
                        "Price",
                        "Quantity"});
            table1.AddRow(new string[] {
                        "Oven",
                        "34.95",
                        "20"});
            table1.AddRow(new string[] {
                        "TV",
                        "346.95",
                        "50"});
#line 7
 testRunner.Given("warehouse contains the following items:", ((string)(null)), table1, "Given ");
#line 11
 testRunner.And("I am able to log in successfully with username \'Admin\' and password \'pass\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
 testRunner.When("I open the application", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 13
 testRunner.And("I set the username to \"Admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
 testRunner.And("I set the password to \"pass\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
 testRunner.And("I log in to the system", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
 testRunner.And("I set the Price for \"TV\" item to 350", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
 testRunner.Then("Total cost of \"TV\" item is 17500", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="Display error for incorrect Price value")]
        [Xunit.TraitAttribute("FeatureTitle", "EditWarehouseItem")]
        [Xunit.TraitAttribute("Description", "Display error for incorrect Price value")]
        public virtual void DisplayErrorForIncorrectPriceValue()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Display error for incorrect Price value", ((string[])(null)));
#line 19
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Kind",
                        "Price",
                        "Quantity"});
            table2.AddRow(new string[] {
                        "Oven",
                        "34.95",
                        "20"});
            table2.AddRow(new string[] {
                        "TV",
                        "346.95",
                        "50"});
            table2.AddRow(new string[] {
                        "PC",
                        "423.95",
                        "70"});
#line 20
 testRunner.Given("warehouse contains the following items:", ((string)(null)), table2, "Given ");
#line 25
 testRunner.And("I am able to log in successfully with username \'Admin\' and password \'pass\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 26
 testRunner.When("I open the application", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 27
 testRunner.And("I set the username to \"Admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 28
 testRunner.And("I set the password to \"pass\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 29
 testRunner.And("I log in to the system", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
 testRunner.And("I set the Price for \"TV\" item to -10", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 31
 testRunner.Then("Error message is displayed with the following text \"Price must be positive.\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="Display error for incorrect Quantity value")]
        [Xunit.TraitAttribute("FeatureTitle", "EditWarehouseItem")]
        [Xunit.TraitAttribute("Description", "Display error for incorrect Quantity value")]
        public virtual void DisplayErrorForIncorrectQuantityValue()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Display error for incorrect Quantity value", ((string[])(null)));
#line 33
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Kind",
                        "Price",
                        "Quantity"});
            table3.AddRow(new string[] {
                        "Oven",
                        "34.95",
                        "20"});
            table3.AddRow(new string[] {
                        "TV",
                        "346.95",
                        "50"});
            table3.AddRow(new string[] {
                        "PC",
                        "423.95",
                        "70"});
#line 34
 testRunner.Given("warehouse contains the following items:", ((string)(null)), table3, "Given ");
#line 39
 testRunner.And("I am able to log in successfully with username \'Admin\' and password \'pass\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 40
 testRunner.When("I open the application", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 41
 testRunner.And("I set the username to \"Admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 42
 testRunner.And("I set the password to \"pass\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
 testRunner.And("I log in to the system", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
 testRunner.And("I set the Quantity for \"TV\" item to -10", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
 testRunner.Then("Error message is displayed with the following text \"Quantity must be positive.\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="Display error for incorrect Kind value")]
        [Xunit.TraitAttribute("FeatureTitle", "EditWarehouseItem")]
        [Xunit.TraitAttribute("Description", "Display error for incorrect Kind value")]
        public virtual void DisplayErrorForIncorrectKindValue()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Display error for incorrect Kind value", ((string[])(null)));
#line 47
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Kind",
                        "Price",
                        "Quantity"});
            table4.AddRow(new string[] {
                        "Oven",
                        "34.95",
                        "20"});
            table4.AddRow(new string[] {
                        "TV",
                        "346.95",
                        "50"});
            table4.AddRow(new string[] {
                        "PC",
                        "423.95",
                        "70"});
#line 48
 testRunner.Given("warehouse contains the following items:", ((string)(null)), table4, "Given ");
#line 53
 testRunner.And("I am able to log in successfully with username \'Admin\' and password \'pass\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 54
 testRunner.When("I open the application", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 55
 testRunner.And("I set the username to \"Admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 56
 testRunner.And("I set the password to \"pass\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 57
 testRunner.And("I log in to the system", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
 testRunner.And("I set the Kind for \"TV\" item to \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
 testRunner.Then("Error message is displayed with the following text \"Kind should not be empty.\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                EditWarehouseItemFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                EditWarehouseItemFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
