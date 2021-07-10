namespace Funcky.SourceGenerator.Templates
{
    public record TemplateSubstitution(string TemplateName, string Substitution)
    {
        public string SourceTemplate => $"{{{{{TemplateName}}}}}";
    }
}
