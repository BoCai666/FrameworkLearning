using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using SourceGenerator.QSFramework.IOC;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SourceGenerator.QSFramework
{
    [Generator]
    internal class SourceGenerateEntry : ISourceGenerator
    {
        private readonly List<IQSFrameworkGenerator> generators = new List<IQSFrameworkGenerator>()
        {
            new IOCGenerator(),
        };

        public void Initialize(GeneratorInitializationContext context)
        {
            foreach (var item in generators)
            {
                item.Initialize(context);
            }
        }

        public void Execute(GeneratorExecutionContext context)
        {
            foreach (var item in generators)
            {
                item.Generate(context);
            }
        }

        
    }
}
