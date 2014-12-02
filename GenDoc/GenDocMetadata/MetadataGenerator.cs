using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocAsCode.EntityModel;
using DocAsCode.Utility;

namespace DocAsCode.GenDocMetadata
{
    interface IMetadataGenerator
    {
        bool CanGenerate(ISymbol symbol);

        DocMetadata GenerateFrom(ISymbol symbol);
    }

    public class DefaultGenerator : IMetadataGenerator
    {
        public virtual bool CanGenerate(ISymbol symbol)
        {
            if (symbol.DeclaredAccessibility.HasFlag(Accessibility.Public))
            {
                return true;
            }

            if (symbol.DeclaredAccessibility.HasFlag(Accessibility.Protected))
            {
                return true;
            }

            return false;
        }

        public virtual DocMetadata GenerateFrom(ISymbol symbol)
        {
            DocMetadata mta = new DocMetadata(symbol.GetDocumentationCommentId());
            mta.XmlDocumentation = symbol.GetDocumentationCommentXml();
            return mta;
        }

        protected SyntaxNode GetSyntaxNode(ISymbol symbol)
        {
            return symbol.DeclaringSyntaxReferences.FirstOrDefault() == null ? null : symbol.DeclaringSyntaxReferences.FirstOrDefault().GetSyntaxAsync().Result;
        }
    }

    public class NamespaceSymbolGenerator : DefaultGenerator
    {
        public override bool CanGenerate(ISymbol symbol)
        {
            var namespaceSymbol = symbol as INamespaceSymbol;
            if (namespaceSymbol == null)
            {
                return false;
            }

            return base.CanGenerate(symbol);
        }

        public override DocMetadata GenerateFrom(ISymbol symbol)
        {
            var syntax = GetSyntaxNode(symbol) as NamespaceDeclarationSyntax;

            if (syntax == null)
            {
                return null;
            }

            var mta = new NamespaceDocMetadata(base.GenerateFrom(symbol));

            return mta;
        }
    }

    public class NameTypeSymbolGenerator : DefaultGenerator
    {
        public override bool CanGenerate(ISymbol symbol)
        {
            var nameTypedSymbol = symbol as INamedTypeSymbol;
            if (nameTypedSymbol == null)
            {
                return false;
            }

            // Currently only support Class
            if (nameTypedSymbol.TypeKind != TypeKind.Class)
            {
                return false;
            }

            return base.CanGenerate(symbol);
        }

        public override DocMetadata GenerateFrom(ISymbol symbol)
        {
            var syntax = GetSyntaxNode(symbol) as ClassDeclarationSyntax;

            if (syntax == null)
            {
                return null;
            }

            DocMetadata mta = base.GenerateFrom(symbol);
            var classMta = new ClassDocMetadata(mta);

            classMta.Syntax = new SyntaxDocFragment
            {
                Content = syntax
                .WithAttributeLists(new SyntaxList<AttributeListSyntax>())
                .WithBaseList(null)
                .WithMembers(new SyntaxList<MemberDeclarationSyntax>())
                .NormalizeWhitespace()
                .ToString()
                .Replace(syntax.OpenBraceToken.ValueText, string.Empty)
                .Replace(syntax.CloseBraceToken.ValueText, string.Empty)
                .Trim(),
                XmlDocumentation = classMta.XmlDocumentation,
            };

            return classMta;
        }
    }

    public class MethodSymbolGenerator : DefaultGenerator
    {
        public override bool CanGenerate(ISymbol symbol)
        {
            // Currently only support Methods
            if (symbol.Kind != SymbolKind.Method)
            {
                return false;
            }

            return base.CanGenerate(symbol);
        }

        public override DocMetadata GenerateFrom(ISymbol symbol)
        {
            // Property is AccessorDeclarationSyntax
            var syntax = GetSyntaxNode(symbol) as MethodDeclarationSyntax;

            if (syntax == null)
            {
                return null;
            }

            DocMetadata mta = base.GenerateFrom(symbol);
            var methodMta = new MethodDocMetadata(mta);

            methodMta.Syntax = new MethodSyntax
            {
                Content = syntax.WithBody(null)
                .NormalizeWhitespace()
                .ToString(),
                XmlDocumentation = methodMta.XmlDocumentation,
            };

            return methodMta;
        }
    }

    public static class DocMetadataConverterFactory
    {
        /// <summary>
        /// Order matters
        /// </summary>
        private static IList<IMetadataGenerator> _supportedConverters = new List<IMetadataGenerator>
        {
            new NamespaceSymbolGenerator(),
            new NameTypeSymbolGenerator(),
            new MethodSymbolGenerator(),
            new DefaultGenerator()
        };

        public static DocMetadata Convert(ISymbol symbol)
        {
            foreach(var converter in _supportedConverters)
            {
                if (converter.CanGenerate(symbol))
                {
                    return converter.GenerateFrom(symbol);
                }
            }

            return null;
        }
    }
}
