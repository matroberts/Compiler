﻿using System;
using System.Text;

namespace KleinCompiler
{
    public class PrettyPrinter : IAstVisitor
    {
        public static void ToConsole(Ast ast)
        {
            Console.WriteLine(ToString(ast));    
        }

        public static string ToString(Ast ast)
        {
            var printer = new PrettyPrinter();
            ast.Accept(printer);
            return printer.ToString();
        }

        private TabbedStringBuilder builder = new TabbedStringBuilder();

        public override string ToString() => builder.ToString();

        public void Visit(Program node)
        {
            builder.AppendLine("Program");
            builder.Indent();
            foreach (var definition in node.Definitions)
            {
                definition.Accept(this);
            }
            builder.Outdent();
        }

        public void Visit(Definition node)
        {
            builder.AppendLine($"Definition");
            builder.Indent();
            node.Identifier.Accept(this);
            node.Type.Accept(this);
            builder.AppendLine($"Formals");
            builder.Indent();
            foreach (var formal in node.Formals)
            {
                formal.Accept(this);
            }
            builder.Outdent();
            builder.Outdent();
        }

        public void Visit(BinaryOperator node)
        {
            string opString;
            switch (node.Operator)
            {
                case BOp.Times:
                    opString = "*";
                    break;
                case BOp.Plus:
                    opString = "+";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            builder.AppendLine($"BinaryOperator({opString})");
            builder.Indent();
            node.Left.Accept(this);
            node.Right.Accept(this);
            builder.Outdent();
        }

        public void Visit(UnaryOperator node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(Formal node)
        {
            builder.AppendLine($"Formal");
            builder.Indent();
            node.Identifier.Accept(this);
            node.Type.Accept(this);
            builder.Outdent();
        }

        public void Visit(Identifier node)
        {
            builder.AppendLine($"Identifier({node.Value})");
        }

        public void Visit(KleinType node)
        {
            builder.AppendLine($"Type({node.Value})");
        }

        public void Visit(BooleanLiteral node)
        {
            builder.AppendLine($"Boolean({node.Value})");
        }
    }

    public class TabbedStringBuilder
    {
        private StringBuilder builder = new StringBuilder();
        private int indent = 0;
        private int spacesPerIndent = 4;

        public TabbedStringBuilder AppendLine(string text)
        {
            builder.Append(' ', indent*spacesPerIndent).AppendLine(text);
            return this;
        }

        public void Indent()
        {
            indent += 1;
        }

        public void Outdent()
        {
            indent -= 1;
        }

        public override string ToString() => builder.ToString();
    }
}