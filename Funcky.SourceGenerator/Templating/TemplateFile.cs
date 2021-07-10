using System;
using System.Collections.Generic;
using System.Linq;
using Funcky.SourceGenerator.Templates;

namespace Funcky.SourceGenerator.Templating
{
    internal class TemplateFile
    {
        private readonly string _template;

        public TemplateFile(string template)
        {
            _template = template;
        }

        public string Apply(IEnumerable<TemplateSubstitution> substitutions)
            => substitutions
                .Aggregate(_template, AggregateSubstitution);

        private static string AggregateSubstitution(string appliedTemplate, TemplateSubstitution substitution)
            => appliedTemplate.Replace(substitution.SourceTemplate, substitution.Substitution);
    }
}
