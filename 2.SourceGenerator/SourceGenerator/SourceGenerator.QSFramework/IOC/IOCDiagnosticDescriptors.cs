using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace SourceGenerator.QSFramework.IOC
{
    internal class IOCDiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor PrivateFieldNotSupported = new(
            id: "QS0001",
            title: "私有字段不支持代码生成",
            messageFormat: "标记为[Inject]的字段 '{0}' 为私有的不支持代码生成",
            category: "SourceGenerator.QSFramework",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);
    }
}
