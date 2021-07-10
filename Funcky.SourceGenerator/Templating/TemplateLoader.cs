using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Funcky.SourceGenerator.Templating
{
    public class TemplateLoader : ITemplateLoader
    {
        private readonly Type _assemblyType;

        public TemplateLoader(Type type)
        {
            _assemblyType = type;
        }

        public string GetTemplate(string templateName)
        {
            if (GetAssembly().GetManifestResourceNames().Contains(templateName))
            {
                if (GetTemplateStream(templateName) is { } stream)
                {
                    using var reader = new StreamReader(stream);

                    return reader.ReadToEnd();
                }

                throw new NotImplementedException();
            }

            throw new Exception($"There is no template resource with the name {templateName}");
        }

        public Stream? GetTemplateStream(string templateName)
            => HasTemplate(templateName)
                ? GetAssembly().GetManifestResourceStream(templateName)
                : throw new Exception($"There is no template resource with the name {templateName}");

        private bool HasTemplate(string templateName)
            => AllTemplateNames()
                .Contains(templateName);

        private IEnumerable<string> AllTemplateNames()
            => GetAssembly()
                .GetManifestResourceNames();

        private Assembly GetAssembly()
            => _assemblyType.Assembly;
    }
}
