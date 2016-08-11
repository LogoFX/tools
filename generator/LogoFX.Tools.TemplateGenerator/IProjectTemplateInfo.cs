namespace LogoFX.Tools.TemplateGenerator
{
    public interface IProjectTemplateInfo : ISolutionItemTemplateInfo
    {
        string NameWithoutRoot { get; }

        string FileName { get; }
    }
}