using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace SourceGenerator.QSFramework.IOC
{
    internal readonly struct IOCParserContext
    {
        public readonly INamedTypeSymbol? injectAttribute;
        public readonly GeneratorExecutionContext generator;
        public IOCParserContext(GeneratorExecutionContext generator, INamedTypeSymbol? injectAttribute)
        {
            this.injectAttribute = injectAttribute;
            this.generator = generator;
        }

        public static IOCParserContext Create(in GeneratorExecutionContext generator)
        {
            var injectAttribute = generator.Compilation.GetTypeByMetadataName("QSFramework.Runtime.IOC.InjectAttribute");
            return new IOCParserContext(generator, injectAttribute);
        }
    }
}
