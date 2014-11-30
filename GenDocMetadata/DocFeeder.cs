using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenDocMetadata
{
    interface IDocFeeder
    {
        bool CanFeed(ISymbol symbol);

        Metadata FeedFrom(ISymbol symbol);
    }

    public class DefaultFeeder : IDocFeeder
    {
        public virtual bool CanFeed(ISymbol symbol)
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

        public virtual Metadata FeedFrom(ISymbol symbol)
        {
            Metadata mta = new Metadata(symbol.GetDocumentationCommentId());
            mta.XmlDocumentation = symbol.GetDocumentationCommentXml();
            return mta;
        }

        protected SyntaxNode GetSyntaxNode(ISymbol symbol)
        {
            return symbol.DeclaringSyntaxReferences.FirstOrDefault() == null ? null : symbol.DeclaringSyntaxReferences.FirstOrDefault().GetSyntaxAsync().Result;
        }
    }

    public class NamespaceSymbolFeeder : DefaultFeeder
    {
        public override bool CanFeed(ISymbol symbol)
        {
            var namespaceSymbol = symbol as INamespaceSymbol;
            if (namespaceSymbol == null)
            {
                return false;
            }

            return base.CanFeed(symbol);
        }

        public override Metadata FeedFrom(ISymbol symbol)
        {
            var syntax = GetSyntaxNode(symbol) as NamespaceDeclarationSyntax;

            if (syntax == null)
            {
                return null;
            }

            var mta = new NamespaceDocMetadata(base.FeedFrom(symbol));

            return mta;
        }
    }

    public class NameTypeSymbolFeeder : DefaultFeeder
    {
        public override bool CanFeed(ISymbol symbol)
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

            return base.CanFeed(symbol);
        }

        public override Metadata FeedFrom(ISymbol symbol)
        {
            var syntax = GetSyntaxNode(symbol) as ClassDeclarationSyntax;

            if (syntax == null)
            {
                return null;
            }

            Metadata mta = base.FeedFrom(symbol);
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

    public class MethodSymbolFeeder : DefaultFeeder
    {
        public override bool CanFeed(ISymbol symbol)
        {
            // Currently only support Methods
            if (symbol.Kind != SymbolKind.Method)
            {
                return false;
            }

            return base.CanFeed(symbol);
        }

        public override Metadata FeedFrom(ISymbol symbol)
        {
            // Property is AccessorDeclarationSyntax
            var syntax = GetSyntaxNode(symbol) as MethodDeclarationSyntax;

            if (syntax == null)
            {
                return null;
            }

            Metadata mta = base.FeedFrom(symbol);
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
        private static IList<IDocFeeder> _supportedConverters = new List<IDocFeeder>
        {
            new NamespaceSymbolFeeder(),
            new NameTypeSymbolFeeder(),
            new MethodSymbolFeeder(),
            new DefaultFeeder()
        };

        public static Metadata Convert(ISymbol symbol)
        {
            foreach(var converter in _supportedConverters)
            {
                if (converter.CanFeed(symbol))
                {
                    return converter.FeedFrom(symbol);
                }
            }

            return null;
        }
    }
}
