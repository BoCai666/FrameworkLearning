using System;
using System.Collections.Generic;
using System.Text;

namespace SourceGenerator.QSFramework
{
    internal class CodeWriter
    {
        private readonly StringBuilder buffer = new StringBuilder();
        private int indentLevel;

        public IDisposable BeginIndentScope() => new IndentScope(this);
        public IDisposable BeginBlockScope(string? startLine = null) => new BlockScope(this, startLine);

        public void AppendLine(string content = "")
        {
            if (string.IsNullOrEmpty(content))
            {
                buffer.AppendLine();
            }
            else
            {
                buffer.AppendLine($"{new string(' ', indentLevel * 4)}{content}");
            }
        }

        public void IncreaseIndent()
        {
            indentLevel++;
        }

        public void DecreaseIndent()
        {
            if (indentLevel > 0)
            {
                indentLevel--;
            }
        }

        public void BeginBlock()
        {
            AppendLine("{");
            IncreaseIndent();
        }

        public void EndBlock()
        {
            DecreaseIndent();
            AppendLine("}");
        }

        public void Clear()
        {
            buffer.Clear();
            indentLevel = 0;
        }

        public override string ToString()
        {
            return buffer.ToString();
        }

        readonly struct IndentScope : IDisposable
        {
            readonly CodeWriter writer;

            public IndentScope(CodeWriter writer)
            {
                this.writer = writer;
                writer.IncreaseIndent();
            }

            public void Dispose()
            {
                writer.DecreaseIndent();
            }
        }

        readonly struct BlockScope : IDisposable
        {
            readonly CodeWriter writer;

            public BlockScope(CodeWriter writer, string? startLine = null)
            {
                this.writer = writer;
                writer.AppendLine(startLine);
                writer.BeginBlock();
            }

            public void Dispose()
            {
                writer.EndBlock();
            }
        }
    }
}
