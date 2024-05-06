using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace SourceGenerator.QSFramework.IOC
{
    internal class IOCInjectedType
    {
        public readonly IOCParserContext context;
        public readonly TypeDeclarationSyntax syntax;
        public readonly INamedTypeSymbol symbol;
        public readonly string typeName;
        public readonly string fullTypename;
        public readonly IMethodSymbol constructor;
        public IMethodSymbol ExplictInjectdConstructor { get; private set; }
        public IReadOnlyList<IFieldSymbol> InjectedFields { get; }
        public IReadOnlyList<IPropertySymbol> InjectedProperties { get; }
        public IReadOnlyList<IMethodSymbol> InjectedMethods { get; }

        public bool IsInjectable => ExplictInjectdConstructor != null
            || InjectedFields.Count > 0
            || InjectedProperties.Count > 0
            || InjectedMethods.Count > 0;

        public IOCInjectedType(in IOCParserContext context, TypeDeclarationSyntax syntax, INamedTypeSymbol symbol)
        {
            this.context = context;
            this.symbol = symbol;
            this.syntax = syntax;
            typeName = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
            fullTypename = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            constructor = GetConstructor();
            InjectedFields = GetInjectedFields();
            InjectedProperties = GetInjectedProperties();
            InjectedMethods = GetInjectedMethods();
        }

        private IMethodSymbol GetConstructor()
        {
            var maxParameterCount = -1;
            IMethodSymbol constructor = null;
            foreach (var method in symbol.InstanceConstructors)
            {
                var methodParameterCount = method.Parameters.Length;
                if (method.ContainsAttribute(context.injectAttribute))
                {
                    constructor = method;
                    ExplictInjectdConstructor = method;
                    break;
                }
                else if (methodParameterCount > maxParameterCount)
                {
                    maxParameterCount = methodParameterCount;
                    constructor = method;
                }
            }
            return constructor;
        }

        private IReadOnlyList<IFieldSymbol> GetInjectedFields()
        {
            return symbol.GetAllMembers()
                .OfType<IFieldSymbol>()
                .Where(f => f.ContainsAttribute(context.injectAttribute))
                .DistinctBy(f => f.Name)
                .ToArray();
        }

        private IReadOnlyList<IPropertySymbol> GetInjectedProperties()
        {
            return symbol.GetAllMembers()
                .OfType<IPropertySymbol>()
                .Where(p => p.ContainsAttribute(context.injectAttribute))
                .DistinctBy(p => p.Name)
                .ToArray();
        }

        private IReadOnlyList<IMethodSymbol> GetInjectedMethods()
        {
            return symbol.GetAllMembers()
                .OfType<IMethodSymbol>()
                .Where(m => m.MethodKind == MethodKind.Ordinary && m.ContainsAttribute(context.injectAttribute))
                .ToArray();
        }
    }
}
