using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;

namespace DocAsCode
{
    [ExportCodeFixProvider(DiagnosticAnalyzer.DiagnosticId, LanguageNames.CSharp)]
    internal class CodeFixProvider : ICodeFixProvider
    {
        [Import]
        internal SVsServiceProvider serviceProvider = null;

        public IEnumerable<string> GetFixableDiagnosticIds()
        {
            return new[] { DiagnosticAnalyzer.DiagnosticId };
        }

        public async Task<IEnumerable<CodeAction>> GetFixesAsync(Microsoft.CodeAnalysis.Document document, TextSpan span, IEnumerable<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            var diagnosticSpan = diagnostics.First().Location.SourceSpan;

            var model = (SemanticModel)document.GetSemanticModelAsync().Result;

            var qualifiedFileName = ReferenceDocumentHeler.GetQualifiedFileName(root.FindToken(diagnosticSpan.Start).Parent, model);
            var qualifiedAPIName = ReferenceDocumentHeler.GetQualifiedAPIName(root.FindToken(diagnosticSpan.Start).Parent, model);

            return new[] { CodeAction.Create("Create a documentation.", new CodeActionOperation[] { new AddDocumentOperation(serviceProvider, document.Project.AssemblyName, qualifiedFileName, qualifiedAPIName) }) };
        }
    }
}