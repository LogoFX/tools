namespace LogoFX.Tools.TemplateGenerator.Contracts
{
    public interface IProjectTemplateInfo : ISolutionItemTemplateInfo
    {
        string NameWithoutRoot { get; }

        string FileName { get; }
    }
}