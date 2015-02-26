﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using DocAsCode.EntityModel;
using System.Text.RegularExpressions;

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
            if ((nameTypedSymbol.TypeKind != TypeKind.Class) && (nameTypedSymbol.TypeKind != TypeKind.Enum)
                && (nameTypedSymbol.TypeKind != TypeKind.Interface) && (nameTypedSymbol.TypeKind != TypeKind.Struct)
                && (nameTypedSymbol.TypeKind != TypeKind.Delegate))
            {
                return false;
            }

            return base.CanGenerate(symbol);
        }

        public override DocMetadata GenerateFrom(ISymbol symbol)
        {
            var nameTypedSymbol = symbol as INamedTypeSymbol;
            switch (nameTypedSymbol.TypeKind)
            {
                case TypeKind.Class:
                    {
                        var syntax = GetSyntaxNode(symbol) as ClassDeclarationSyntax;

                        if (syntax == null)
                        {
                            return null;
                        }

                        DocMetadata mta = base.GenerateFrom(symbol);
                        var classMta = new ClassDocMetadata(mta);

                        classMta.InheritanceHierarchy = new Stack<Identity>();
                        var type = nameTypedSymbol.BaseType;
                        while(type != null)
                        {
                            classMta.InheritanceHierarchy.Push(new Identity(type.GetDocumentationCommentId()));
                            type = type.BaseType;
                        }

                        classMta.FilePath = syntax.SyntaxTree.FilePath;

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
                    };
                case TypeKind.Enum:
                    {
                        var syntax = GetSyntaxNode(symbol) as EnumDeclarationSyntax;

                        if (syntax == null)
                        {
                            return null;
                        }

                        DocMetadata mta = base.GenerateFrom(symbol);
                        var EnumMta = new EnumDocMetadata(mta);
                        EnumMta.Syntax = new SyntaxDocFragment
                        {
                            Content = syntax
                                .WithAttributeLists(new SyntaxList<AttributeListSyntax>())
                                .WithBaseList(null)
                                .WithMembers(new SeparatedSyntaxList<EnumMemberDeclarationSyntax>())
                                .NormalizeWhitespace()
                                .ToString()
                                .Replace(syntax.OpenBraceToken.ValueText, string.Empty)
                                .Replace(syntax.CloseBraceToken.ValueText, string.Empty)
                                .Trim(),
                            XmlDocumentation = EnumMta.XmlDocumentation,
                        };

                        return EnumMta;
                    };
                case TypeKind.Interface:
                    {
                        var syntax = GetSyntaxNode(symbol) as InterfaceDeclarationSyntax;

                        if (syntax == null)
                        {
                            return null;
                        }

                        DocMetadata mta = base.GenerateFrom(symbol);
                        var interfaceMta = new InterfaceDocMetadata(mta);

                        interfaceMta.InheritanceHierarchy = new Stack<Identity>();
                        var type = nameTypedSymbol.BaseType;
                        while (type != null)
                        {
                            interfaceMta.InheritanceHierarchy.Push(new Identity(type.GetDocumentationCommentId()));
                            type = type.BaseType;
                        }

                        interfaceMta.FilePath = syntax.SyntaxTree.FilePath;

                        interfaceMta.Syntax = new SyntaxDocFragment
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
                            XmlDocumentation = interfaceMta.XmlDocumentation,
                        };
                        return interfaceMta;
                    };
                case TypeKind.Struct:
                    {
                        var syntax = GetSyntaxNode(symbol) as StructDeclarationSyntax;

                        if (syntax == null)
                        {
                            return null;
                        }

                        DocMetadata mta = base.GenerateFrom(symbol);
                        var StructMta = new StructDocMetadata(mta);

                        StructMta.InheritanceHierarchy = new Stack<Identity>();
                        var type = nameTypedSymbol.BaseType;
                        while (type != null)
                        {
                            StructMta.InheritanceHierarchy.Push(new Identity(type.GetDocumentationCommentId()));
                            type = type.BaseType;
                        }

                        StructMta.FilePath = syntax.SyntaxTree.FilePath;

                        StructMta.Syntax = new SyntaxDocFragment
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
                            XmlDocumentation = StructMta.XmlDocumentation,
                        };
                        return StructMta;
                    };
                case TypeKind.Delegate:
                    {
                        var syntax = GetSyntaxNode(symbol) as DelegateDeclarationSyntax;

                        if (syntax == null)
                        {
                            return null;
                        }

                        DocMetadata mta = base.GenerateFrom(symbol);
                        var delegateMta = new DelegateDocMetadata(mta);

                        delegateMta.Syntax = new SyntaxDocFragment
                        {
                            Content = syntax
                                .WithAttributeLists(new SyntaxList<AttributeListSyntax>())
                                .NormalizeWhitespace()
                                .ToString()
                                .Trim(),
                            XmlDocumentation = delegateMta.XmlDocumentation,
                        };
                        return delegateMta;
                    };
                default: return new DocMetadata();
            }
        }
    }

    public class ClassSymbolGenerator : DefaultGenerator
    {
        public override bool CanGenerate(ISymbol symbol)
        {
            // Currently support Methods, property, field and event.
            if ((symbol.Kind != SymbolKind.Method) && (symbol.Kind != SymbolKind.Property) 
                && (symbol.Kind != SymbolKind.Field) && (symbol.Kind != SymbolKind.Event))
            {
                return false;
            }

            return base.CanGenerate(symbol);
        }

        public override DocMetadata GenerateFrom(ISymbol symbol)
        {
            switch (symbol.Kind)
            {
                case (SymbolKind.Method):
                    {
                        var syntax = GetSyntaxNode(symbol) as MethodDeclarationSyntax;

                        if (syntax != null)
                        {
                            DocMetadata mta = base.GenerateFrom(symbol);
                            var methodMta = new MethodDocMetadata(mta);

                            var methodSymbol = symbol as IMethodSymbol;

                            if(methodSymbol != null)
                            {
                                var parametersDic = new SortedDictionary<string, string>() { };

                                foreach (var p in methodSymbol.Parameters)
                                {
                                    var param = String.Format("{0} : {1}", p.Type.GetDocumentationCommentId() == null ? p.Type.BaseType.GetDocumentationCommentId() : p.Type.GetDocumentationCommentId(), p.Name);
                                    parametersDic.Add(param, "");
                                };

                                var returnType = methodSymbol.ReturnType.GetDocumentationCommentId() == null ? methodSymbol.ReturnType.BaseType.GetDocumentationCommentId() : methodSymbol.ReturnType.GetDocumentationCommentId();

                                methodMta.Syntax = new MethodSyntax
                                {
                                    Parameters = parametersDic,
                                    Returns = new Dictionary<string, string> { { returnType, "" }, },
                                    Content = syntax.WithBody(null)
                                        .NormalizeWhitespace()
                                        .ToString()
                                        .Trim(),
                                    XmlDocumentation = methodMta.XmlDocumentation,
                                    StartLine = syntax.SyntaxTree.GetLineSpan(syntax.Span).StartLinePosition.Line,
                                };
                            }

                            return methodMta;
                        }
                        else
                        {
                            var constructorSyntax = GetSyntaxNode(symbol) as ConstructorDeclarationSyntax;
                            if (constructorSyntax != null)
                            {
                                DocMetadata mta = base.GenerateFrom(symbol);
                                var constuctorMta = new ConstructorDocMetadata(mta);

                                var methodSymbol = symbol as IMethodSymbol;

                                if (methodSymbol != null)
                                {
                                    var parametersDic = new SortedDictionary<string, string>() { };

                                    foreach (var p in methodSymbol.Parameters)
                                    {
                                        var param = String.Format("{0} : {1}", p.Type.GetDocumentationCommentId() == null ? p.Type.BaseType.GetDocumentationCommentId() : p.Type.GetDocumentationCommentId(), p.Name);
                                        parametersDic.Add(param, "");
                                    };
                                    constuctorMta.Syntax = new ConstructorSyntax
                                    {
                                        Parameters = parametersDic,
                                        Content = constructorSyntax.WithBody(null)
                                            .NormalizeWhitespace()
                                            .ToString()
                                            .Trim(),
                                        XmlDocumentation = constuctorMta.XmlDocumentation,
                                        StartLine = constructorSyntax.SyntaxTree.GetLineSpan(constructorSyntax.Span).StartLinePosition.Line,
                                    };
                                }
                                return constuctorMta;
                            }
                            return null;
                        }
                    };
                case (SymbolKind.Property):
                    {
                        var syntax = GetSyntaxNode(symbol) as PropertyDeclarationSyntax;

                        if (syntax == null)
                        {
                            return null;
                        }

                        DocMetadata mta = base.GenerateFrom(symbol);
                        var propertyMta = new PropertyDocMetadata(mta);

                        var propertySymbol = symbol as IPropertySymbol;
                        string type = propertySymbol.Type.GetDocumentationCommentId() == null ? propertySymbol.Type.BaseType.GetDocumentationCommentId() : propertySymbol.Type.GetDocumentationCommentId();

                        propertyMta.Syntax = new PropertySyntax
                        {
                            Content = syntax
                                .WithAttributeLists(new SyntaxList<AttributeListSyntax>())
                                .WithAccessorList(null)
                                .NormalizeWhitespace()
                                .ToString()
                                .Trim(),
                            XmlDocumentation = propertyMta.XmlDocumentation,
                            StartLine = syntax.SyntaxTree.GetLineSpan(syntax.Span).StartLinePosition.Line,
                            PropertyType = type,
                        };
                        return propertyMta;
                    };
                case (SymbolKind.Field):
                    {
                        var syntax = GetSyntaxNode(symbol) as VariableDeclaratorSyntax;

                        if (syntax == null)
                        {
                            return null;
                        }

                        DocMetadata mta = base.GenerateFrom(symbol);
                        var FieldMta = new FieldDocMetadata(mta);

                        FieldMta.Syntax = new SyntaxDocFragment
                        {
                            Content = syntax
                                .WithInitializer(null)
                                .NormalizeWhitespace()
                                .ToString()
                                .Trim(),
                            XmlDocumentation = FieldMta.XmlDocumentation,
                            StartLine = syntax.SyntaxTree.GetLineSpan(syntax.Span).StartLinePosition.Line,
                        };
                        return FieldMta;
                    };
                case (SymbolKind.Event):
                    {
                        var syntax = GetSyntaxNode(symbol) as VariableDeclaratorSyntax;

                        if (syntax == null)
                        {
                            return null;
                        }

                        DocMetadata mta = base.GenerateFrom(symbol);
                        var eventMta = new EventDocMetadataDefinition(mta);

                        eventMta.Syntax = new SyntaxDocFragment
                        {
                            Content = syntax
                                .NormalizeWhitespace()
                                .ToString()
                                .Trim(),
                            XmlDocumentation = eventMta.XmlDocumentation,
                            StartLine = syntax.SyntaxTree.GetLineSpan(syntax.Span).StartLinePosition.Line,
                        };
                        return eventMta;
                    };
                default: return new DocMetadata();
            }
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
            new ClassSymbolGenerator(),
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

        public static CompositeDocMetadata ExpandSymbolMembers(INamedTypeSymbol type, CompositeDocMetadata mta)
        {
            foreach (var member in type.GetMembers())
            {
                var metadata = DocMetadataConverterFactory.Convert(member);

                var constructorMta = metadata as ConstructorDocMetadata;
                if (constructorMta != null)
                {
                    mta.TryAdd(constructorMta, MemberType.Constructor);
                    continue;
                }

                var methodMta = metadata as MethodDocMetadata;
                if (methodMta != null)
                {
                    mta.TryAdd(methodMta, MemberType.Method);
                    continue;
                }

                var propertyMta = metadata as PropertyDocMetadata;
                if (propertyMta != null)
                {
                    mta.TryAdd(propertyMta, MemberType.Property);
                    continue;
                }
                var fieldMta = metadata as FieldDocMetadata;
                if (fieldMta != null)
                {
                    mta.TryAdd(fieldMta, MemberType.Field);
                    continue;
                }
                var eventMta = metadata as EventDocMetadataDefinition;
                if (eventMta != null)
                {
                    mta.TryAdd(eventMta, MemberType.Event);
                    continue;
                }
            }

            return mta;
        }
    }
}