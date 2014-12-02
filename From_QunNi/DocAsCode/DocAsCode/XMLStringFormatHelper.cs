using Microsoft.CodeAnalysis;


namespace DocAsCode
{
    internal class XMLStringFormatHelper : SymbolDisplayFormat
    {
        public static readonly SymbolDisplayFormat qualifiedFileFormat =
            new SymbolDisplayFormat(
                memberOptions: SymbolDisplayMemberOptions.IncludeContainingType,
                globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
                kindOptions: SymbolDisplayKindOptions.IncludeMemberKeyword | SymbolDisplayKindOptions.IncludeNamespaceKeyword |
                SymbolDisplayKindOptions.IncludeTypeKeyword
                );
    }
}
