using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SourceGenerator.QSFramework.IOC
{
    internal sealed class IOCSyntaxReceiver : ISyntaxReceiver
    {
        public readonly List<IOCSyntaxParser> syntaxParsers = new List<IOCSyntaxParser>(16);
        public bool AutoGenerateCode { get; private set; } = true;

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (!AutoGenerateCode)
            {
                return;
            }
            if (syntaxNode is not ClassDeclarationSyntax classDeclarationSyntax)
            {
                return;
            }
            AutoGenerateCode = CheckAutoGenerateCode(classDeclarationSyntax);
            foreach (var modifier in classDeclarationSyntax.Modifiers)
            {
                if (modifier.IsKind(SyntaxKind.AbstractKeyword) || modifier.IsKind(SyntaxKind.StaticKeyword))
                {
                    return;
                }
            }
            syntaxParsers.Add(new IOCSyntaxParser((TypeDeclarationSyntax)syntaxNode));
        }

        private bool CheckAutoGenerateCode(ClassDeclarationSyntax classDeclarationSyntax)
        {
            if (classDeclarationSyntax.Identifier.ValueText.Equals("IOCConfig"))
            {
                var childNodes = classDeclarationSyntax.ChildNodes();
                foreach (var node in childNodes)
                {
                    if (node is not FieldDeclarationSyntax fieldNode)
                    {
                        continue;
                    }
                    var variableName = fieldNode.Declaration.Variables[0].Identifier.Value;
                    if (!variableName.Equals("AutoGenerateCode"))
                    {
                        continue;
                    }
                    if (fieldNode.Declaration.Variables[0].Initializer == null)
                    {
                        return false;
                    }
                    else
                    {
                        var expression = fieldNode.Declaration.Variables[0].Initializer.Value;
                        if (expression.IsKind(SyntaxKind.TrueLiteralExpression) || expression.IsKind(SyntaxKind.FalseLiteralExpression))
                        {
                           return expression.IsKind(SyntaxKind.TrueLiteralExpression);
                        }
                    }
                }
            }
            return true;
        }
    }
}
