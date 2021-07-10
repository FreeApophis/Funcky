using System;

namespace Funcky.SourceGenerator.Templating
{
    internal class Template
    {
        private readonly ITemplateLoader _templateLoader;

        private Template(ITemplateLoader templateLoader)
        {
            _templateLoader = templateLoader;
        }

        public static Template Create(Type type)
            => new(new TemplateLoader(type));

        public TemplateFile Get(string templateName)
            => new(_templateLoader.GetTemplate(FullTemplateName(templateName)));

        private static string FullTemplateName(string templateName)
            => $"Funcky.SourceGenerator.Templates.{templateName}";
    }
}
