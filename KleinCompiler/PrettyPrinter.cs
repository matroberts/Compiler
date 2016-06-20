using System;
using System.Runtime.InteropServices;
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

        public void Visit(Definition definition)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(Program definition)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(Expr expr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(BinaryOperator expr)
        {
            builder.AppendLine($"BinaryOperator({expr.Operator})");
            builder.Indent();
            expr.Left.Accept(this);
            expr.Right.Accept(this);
            builder.Outdent();
        }

        public void Visit(UnaryOperator expr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(Identifier expr)
        {
            builder.AppendLine($"Identifier({expr.Value})");
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