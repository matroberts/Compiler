using System;
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

        public void Visit(Definition node)
        {
            builder.AppendLine($"Definition");
            builder.Indent();
            node.Identifier.Accept(this);
            node.Type.Accept(this);
            builder.Outdent();
        }

        public void Visit(Expr node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(BinaryOperator node)
        {
            builder.AppendLine($"BinaryOperator({node.Operator})");
            builder.Indent();
            node.Left.Accept(this);
            node.Right.Accept(this);
            builder.Outdent();
        }

        public void Visit(UnaryOperator node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(Identifier node)
        {
            builder.AppendLine($"Identifier({node.Value})");
        }

        public void Visit(KleinType node)
        {
            builder.AppendLine($"Type({node.Value})");
        }
    }

    public class TabbedStringBuilder
    {
        private StringBuilder builder = new StringBuilder();
        private int indent = 0;
        private int spacesPerIndent = 4;

        public TabbedStringBuilder AppendLine(string text)
        {
            builder.Append(' ', indent * spacesPerIndent).AppendLine(text);
            return this;
        }

        public void Indent() { indent += 1; }
        public void Outdent() { indent -= 1; }
        public override string ToString() => builder.ToString();
    }
}