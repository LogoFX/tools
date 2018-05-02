namespace LogoFX.Tools.TemplateGenerator.Model.Contract
{
    public interface ISolutionConfiguration : IAppModel
    {
        string Path { get; }

        bool Vs2017 { get; set; }
    }
}