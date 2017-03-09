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
    public partial class ViewWarehouseContentsFeature : Xunit.IClassFixture<ViewWarehouseContentsFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ViewWarehouseContents.feature"
#line hidden
        
        public ViewWarehouseContentsFeature()
        {
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ViewWarehouseContents", "\tIn order to view and manage warehouse contents\r\n\tAs an entitled user\r\n\tI want to" +
                    " be able to edit the displayed items", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        
        public virtual void SetFixture(ViewWarehouseContentsFeature.FixtureData fixtureData)
        {
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute(DisplayName="Display warehouse items")]
        [Xunit.TraitAttribute("FeatureTitle", "ViewWarehouseContents")]
        [Xunit.TraitAttribute("Description", "Display warehouse items")]
        public virtual void DisplayWarehouseItems()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Display warehouse items", ((string[])(null)));
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
            table1.AddRow(new string[] {
                        "PC",
                        "423.95",
                        "70"});
#line 7
 testRunner.Given("warehouse contains the following items:", ((string)(null)), table1, "Given ");
#line 12
 testRunner.And("I am able to log in successfully with username \'Admin\' and password \'pass\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
 testRunner.When("I open the application", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 14
 testRunner.And("I set the username to \"Admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
 testRunner.And("I set the password to \"pass\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
 testRunner.And("I log in to the system", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Kind",
                        "Price",
                        "Quantity",
                        "Total Cost"});
            table2.AddRow(new string[] {
                        "Oven",
                        "34.95",
                        "20",
                        "699"});
            table2.AddRow(new string[] {
                        "TV",
                        "346.95",
                        "50",
                        "17347.5"});
            table2.AddRow(new string[] {
                        "PC",
                        "423.95",
                        "70",
                        "29676.5"});
#line 17
 testRunner.Then("I expect to see the following data on the screen:", ((string)(null)), table2, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="Delete warehouse item")]
        [Xunit.TraitAttribute("FeatureTitle", "ViewWarehouseContents")]
        [Xunit.TraitAttribute("Description", "Delete warehouse item")]
        public virtual void DeleteWarehouseItem()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Delete warehouse item", ((string[])(null)));
#line 23
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
#line 24
 testRunner.Given("warehouse contains the following items:", ((string)(null)), table3, "Given ");
#line 29
 testRunner.And("I am able to log in successfully with username \'Admin\' and password \'pass\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
 testRunner.When("I open the application", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 31
 testRunner.And("I set the username to \"Admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 32
 testRunner.And("I set the password to \"pass\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 33
 testRunner.And("I log in to the system", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 34
 testRunner.And("I delete \"TV\" item", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Kind",
                        "Price",
                        "Quantity",
                        "Total Cost"});
            table4.AddRow(new string[] {
                        "Oven",
                        "34.95",
                        "20",
                        "699"});
            table4.AddRow(new string[] {
                        "PC",
                        "423.95",
                        "70",
                        "29676.5"});
#line 35
 testRunner.Then("I expect to see the following data on the screen:", ((string)(null)), table4, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="Create new warehouse item")]
        [Xunit.TraitAttribute("FeatureTitle", "ViewWarehouseContents")]
        [Xunit.TraitAttribute("Description", "Create new warehouse item")]
        public virtual void CreateNewWarehouseItem()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create new warehouse item", ((string[])(null)));
#line 40
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Kind",
                        "Price",
                        "Quantity"});
            table5.AddRow(new string[] {
                        "Oven",
                        "34.95",
                        "20"});
            table5.AddRow(new string[] {
                        "TV",
                        "346.95",
                        "50"});
#line 41
 testRunner.Given("warehouse contains the following items:", ((string)(null)), table5, "Given ");
#line 45
 testRunner.And("I am able to log in successfully with username \'Admin\' and password \'pass\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
 testRunner.When("I open the application", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 47
 testRunner.And("I set the username to \"Admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
 testRunner.And("I set the password to \"pass\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 49
 testRunner.And("I log in to the system", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Kind",
                        "Price",
                        "Quantity"});
            table6.AddRow(new string[] {
                        "PC",
                        "423.95",
                        "70"});
#line 50
 testRunner.And("I create a new warehouse item with the following data:", ((string)(null)), table6, "And ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Kind",
                        "Price",
                        "Quantity",
                        "Total Cost"});
            table7.AddRow(new string[] {
                        "Oven",
                        "34.95",
                        "20",
                        "699"});
            table7.AddRow(new string[] {
                        "TV",
                        "346.95",
                        "50",
                        "17347.5"});
            table7.AddRow(new string[] {
                        "PC",
                        "423.95",
                        "70",
                        "29676.5"});
#line 53
 testRunner.Then("I expect to see the following data on the screen:", ((string)(null)), table7, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                ViewWarehouseContentsFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                ViewWarehouseContentsFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
