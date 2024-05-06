using Microsoft.CodeAnalysis;
using System.Linq;

namespace SourceGenerator.QSFramework.IOC
{
    internal sealed class IOCGenerator : IQSFrameworkGenerator
    {
        private static readonly string[] ExcludedModuleNames =
        {
            "UnityEngine.",
            "UnityEditor.",
            "Unity."
        };

        public void Initialize(in GeneratorInitializationContext initializationContext)
        {
            initializationContext.RegisterForSyntaxNotifications(() => new IOCSyntaxReceiver());
        }

        public void Generate(in GeneratorExecutionContext generatorContext)
        {
            var moduleName = generatorContext.Compilation.SourceModule.Name;
            foreach (var item in ExcludedModuleNames)
            {
                if (moduleName.StartsWith(item))
                {
                    return;
                }
            }
            var parserContext = IOCParserContext.Create(generatorContext);
            if (parserContext.injectAttribute == null)
            {
                return;
            }
            var syntaxParsers = (generatorContext.SyntaxReceiver as IOCSyntaxReceiver).syntaxParsers;
            var writer = new CodeWriter();
            foreach (var item in syntaxParsers)
            {
                var injectedType = item.Parse(parserContext);
                if (injectedType == null)
                {
                    continue;
                }
                var typeName = injectedType.typeName
                    .Replace("global::", "")
                    .Replace("<", "_")
                    .Replace(">", "_");
                var generatedTypeName = $"{typeName}GeneratedInjector";
                GenerateInjector(injectedType, writer, generatedTypeName);
                generatorContext.AddSource($"{generatedTypeName}.g.cs", writer.ToString());
                writer.Clear();
            }
        }

        private void GenerateInjector(IOCInjectedType injectedType, CodeWriter writer, string generatedTypeName)
        {
            writer.AppendLine("using System;");
            writer.AppendLine("using System.Collections.Generic;");
            writer.AppendLine("using QSFramework.Runtime.IOC;");
            writer.AppendLine();
            
            var namespaceSymbol = injectedType.symbol.ContainingNamespace;
            if (!namespaceSymbol.IsGlobalNamespace)
            {
                writer.AppendLine($"namespace {namespaceSymbol}");
                writer.BeginBlock();
            }

            using (writer.BeginBlockScope($"class {generatedTypeName} : IInjector"))
            {
                GenerateCreateInstanceMethod(injectedType, writer);
                writer.AppendLine();
                GenerateInjectMethod(injectedType, writer);
            }

            if (!namespaceSymbol.IsGlobalNamespace)
            {
                writer.EndBlock();
            }
        }

        private void GenerateCreateInstanceMethod(IOCInjectedType injectedType, CodeWriter writer)
        {
            using (writer.BeginBlockScope("public object CreateInstance(IObjectResolver resolver, IReadOnlyList<IInjectParameter> injectParameters)"))
            {
                var parameters = injectedType.constructor.Parameters
                        .Select(param =>
                        {
                            var paramType = param.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                            var paramName = param.Name;
                            return (paramType, paramName);
                        })
                        .ToArray();
                foreach (var (paramType, paramName) in parameters)
                {
                    writer.AppendLine($"var __{paramName} = resolver.ResolveParameter(typeof{paramType}, \"{paramName}\", injectParameters);");
                }
                var arguments = parameters.Select(x => $"({x.paramType})__{x.paramName}");
                writer.AppendLine($"var __instance = new {injectedType.typeName}({string.Join(", ", arguments)});");
                writer.AppendLine($"Inject(__instance, resolver, injectParameters);");
                writer.AppendLine($"return __instance;");
            }
        }

        private void GenerateInjectMethod(IOCInjectedType injectedType, CodeWriter writer)
        {
            using (writer.BeginBlockScope("public void Inject(object instance, IObjectResolver resolver, IReadOnlyList<IInjectParameter> injectParameters)"))
            {
                writer.AppendLine($"var __instance = ({injectedType.fullTypename})instance;");
                foreach (var item in injectedType.InjectedFields)
                {
                    if (!item.CanBeCallFromInternal())
                    {
                        injectedType.context.generator.ReportDiagnostic(Diagnostic.Create(
                            IOCDiagnosticDescriptors.PrivateFieldNotSupported,
                            item.Locations.FirstOrDefault() ?? injectedType.syntax.GetLocation(),
                            injectedType.symbol.Name));
                        return;
                    }
                    writer.AppendLine($"__instance.{item.Name} = resolver.Resolve<{item.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}>();");
                }
                foreach (var item in injectedType.InjectedProperties)
                {
                    writer.AppendLine($"__instance.{item.Name} = resolver.Resolve<{item.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}>();");
                }
                foreach (var method in injectedType.InjectedMethods)
                {
                    var parameters = method.Parameters
                        .Select(param =>
                        {
                            var paramType = param.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                            var paramName = param.Name;
                            return (paramType, paramName);
                        })
                        .ToArray();
                    foreach (var (paramType, paramName) in parameters)
                    {
                        writer.AppendLine($"var __{paramName} = resolver.ResolveParameter(typeof{paramType}, \"{paramName}\", injectParameters);");
                    }
                    var arguments = parameters.Select(x => $"({x.paramType})__{x.paramName}");
                    writer.AppendLine($"__instance.{method.Name}({string.Join(", ", arguments)});");
                }
            }
        }
    }
}
