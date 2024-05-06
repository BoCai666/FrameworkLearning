using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace SourceGenerator.QSFramework
{
    internal interface IQSFrameworkGenerator
    {
        void Initialize(in GeneratorInitializationContext initlizationContext);
        void Generate(in GeneratorExecutionContext generatorContext);
    }
}
