using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SourceGenerator.QSFramework.IOC
{
    internal sealed class IOCSyntaxParser
    {
        private readonly TypeDeclarationSyntax typeDeclarationSyntax;
        private const string ExcludedNamespace = "global::QSFramework.Runtime.IOC";

        public IOCSyntaxParser(TypeDeclarationSyntax typeDeclarationSyntax)
        {
            this.typeDeclarationSyntax = typeDeclarationSyntax;
        }

        public IOCInjectedType? Parse(in IOCParserContext context)
        {
            var semanticModel = context.generator.Compilation.GetSemanticModel(typeDeclarationSyntax.SyntaxTree);
            var declaredSymbol = semanticModel.GetDeclaredSymbol(typeDeclarationSyntax, context.generator.CancellationToken);
            if (declaredSymbol.Arity > 0)
            {
                return null;
            }
            var namespaceName = declaredSymbol.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            if (namespaceName.Equals(ExcludedNamespace))
            {
                return null;
            }
            var injectedType = new IOCInjectedType(context, typeDeclarationSyntax, declaredSymbol);
            if (!injectedType.IsInjectable)
            {
                return null;
            }
            return injectedType;
        }
    }
}
