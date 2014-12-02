using System;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.Shell;
using System.Diagnostics;
using EnvDTE;

namespace DocAsCode
{
    [DiagnosticAnalyzer]
    [ExportDiagnosticAnalyzer(DiagnosticId, LanguageNames.CSharp)]
    public class DiagnosticAnalyzer : ISyntaxNodeAnalyzer<SyntaxKind>
    {
        [Import]
        private SVsServiceProvider serviceProvider = null;
        private DTE dte = null;

        internal const string DiagnosticId = "MissingDocumentation";
        internal const string Description = "Type doesn't have documentation.";
        internal const string MessageFormat = "{0} '{1}' doesn't have a documentation.";
        internal const string Category = "Documentation";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Description, MessageFormat, Category, DiagnosticSeverity.Warning);

        public ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public ImmutableArray<SyntaxKind> SyntaxKindsOfInterest
        {
            get
            {
                return ImmutableArray.Create(SyntaxKind.ClassDeclaration, SyntaxKind.InterfaceDeclaration, SyntaxKind.StructDeclaration, SyntaxKind.EnumDeclaration,
                                             SyntaxKind.VariableDeclarator, SyntaxKind.MethodDeclaration, SyntaxKind.ConstructorDeclaration, SyntaxKind.DestructorDeclaration,
                                             SyntaxKind.DelegateDeclaration, SyntaxKind.NamespaceDeclaration, SyntaxKind.CompilationUnit);
            }
        }

        public void AnalyzeNode(SyntaxNode node, SemanticModel semanticModel, Action<Diagnostic> addDiagnostic, CancellationToken cancellationToken)
        {
            ReferenceDocumentHeler helper = new ReferenceDocumentHeler(serviceProvider);
            ISymbol symbol = null;

            try
            {
                symbol = semanticModel.GetDeclaredSymbol(node);
            }
            catch (OperationCanceledException e)
            {
                Debug.WriteLine("Exception: " + e.Message);
                return;
            }

            Boolean isCompilationUnitOrNamesapceDeclaration = node.IsKind(SyntaxKind.CompilationUnit) || node.IsKind(SyntaxKind.NamespaceDeclaration);
            Boolean isRightAAccessibility = symbol == null || (symbol.DeclaredAccessibility != Accessibility.Public && symbol.DeclaredAccessibility != Accessibility.Protected
                                    && symbol.DeclaredAccessibility != Accessibility.ProtectedAndInternal);
            if (!isCompilationUnitOrNamesapceDeclaration && isRightAAccessibility)
            {
                return;
            }

            if (node is EnumDeclarationSyntax)
            {
                var enumDecl = node as EnumDeclarationSyntax;
                var id = enumDecl.Identifier;
                if (!helper.XMLFileExist(ReferenceDocumentHeler.GetQualifiedFileName(enumDecl, semanticModel)))
                    addDiagnostic(Diagnostic.Create(Rule, id.GetLocation(), enumDecl.EnumKeyword, id));
            }
            else if (node is MethodDeclarationSyntax)
            {
                var methodDecl = node as MethodDeclarationSyntax;
                var id = methodDecl.Identifier;
                if (!helper.XMLFileExist(ReferenceDocumentHeler.GetQualifiedFileName(methodDecl, semanticModel)))
                    addDiagnostic(Diagnostic.Create(Rule, id.GetLocation(), "Method", id));
            }
            else if (node is ConstructorDeclarationSyntax)
            {
                var constuctorDecl = node as ConstructorDeclarationSyntax;
                var id = constuctorDecl.Identifier;
                if (!helper.XMLFileExist(ReferenceDocumentHeler.GetQualifiedFileName(constuctorDecl, semanticModel)))
                    addDiagnostic(Diagnostic.Create(Rule, id.GetLocation(), "Constuctor", id));
            }
            else if (node is DelegateDeclarationSyntax)
            {
                var delegateDecl = node as DelegateDeclarationSyntax;
                var id = delegateDecl.Identifier;
                if (!helper.XMLFileExist(ReferenceDocumentHeler.GetQualifiedFileName(delegateDecl, semanticModel)))
                    addDiagnostic(Diagnostic.Create(Rule, id.GetLocation(), "Delegate", id));
            }
            else if (node is PropertyDeclarationSyntax)
            {
                var propertyDecl = node as PropertyDeclarationSyntax;
                var id = propertyDecl.Identifier;
                if (!helper.XMLFileExist(ReferenceDocumentHeler.GetQualifiedFileName(propertyDecl, semanticModel)))
                    addDiagnostic(Diagnostic.Create(Rule, id.GetLocation(), "Property", id));
            }
            else if (node is TypeDeclarationSyntax)
            {
                var typeDecl = node as TypeDeclarationSyntax;
                var id = typeDecl.Identifier;
                if (!helper.XMLFileExist(ReferenceDocumentHeler.GetQualifiedFileName(typeDecl, semanticModel)))
                    addDiagnostic(Diagnostic.Create(Rule, id.GetLocation(), typeDecl.Keyword, id));
            }
            else if (node is VariableDeclaratorSyntax)
            {
                var fieldDecl = node as VariableDeclaratorSyntax;
                var id = fieldDecl.Identifier;
                if (!helper.XMLFileExist(ReferenceDocumentHeler.GetQualifiedFileName(fieldDecl, semanticModel)))
                    addDiagnostic(Diagnostic.Create(Rule, id.GetLocation(), "Variable", id));
            }
            else if(node is NamespaceDeclarationSyntax)
            {
                var namespaceDecl = node as NamespaceDeclarationSyntax;
                var id = namespaceDecl.GetFirstToken();
                if (!helper.XMLFileExist(ReferenceDocumentHeler.GetQualifiedFileName(namespaceDecl, semanticModel)))
                    addDiagnostic(Diagnostic.Create(Rule, id.GetLocation(), "Namespace", namespaceDecl.Name.ToString()));
            }
        }
    }
}
