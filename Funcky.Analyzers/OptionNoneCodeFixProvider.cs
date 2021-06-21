using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using static Funcky.Analyzers.Rules;

namespace Funcky.Analyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp)]
    public sealed class OptionNoneCodeFixProvider : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(ReplaceNoneMethodCallWithPropertyAccess.Id);

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = (await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false))!;

            foreach (var diagnostic in context.Diagnostics)
            {
                var node = root.FindNode(diagnostic.Location.SourceSpan);
                if (node is InvocationExpressionSyntax invocationExpressionSyntax)
                {
                    context.RegisterCodeFix(
                        CodeAction.Create(
                            title: "Replace method invocation with property access",
                            equivalenceKey: "Replace method invocation with property access",
                            createChangedDocument: cancellationToken => FixCode(context.Document, invocationExpressionSyntax, cancellationToken)),
                        diagnostic);
                }
            }
        }

        private static async Task<Document> FixCode(Document document, InvocationExpressionSyntax invocation, CancellationToken cancellationToken)
        {
            var editor = await DocumentEditor.CreateAsync(document, cancellationToken);
            editor.ReplaceNode(invocation, invocation.Expression.WithTriviaFrom(invocation));
            return editor.GetChangedDocument();
        }
    }
}
