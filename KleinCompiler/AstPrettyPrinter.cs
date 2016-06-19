using System;
using System.Runtime.InteropServices;
using System.Text;

namespace KleinCompiler
{
    public class AstPrettyPrinter : IAstVisitor
    {
        public static void ToConsole(Ast ast)
        {
            var printer = new AstPrettyPrinter();
            ast.Accept(printer);
            Console.WriteLine(printer);    
        }

        public static string ToString(Ast ast)
        {
            var printer = new AstPrettyPrinter();
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
        private int tabs = 0;
        private int spacesPerTab = 4;

        public TabbedStringBuilder AppendLine(string text)
        {
            builder.Append(' ', tabs * spacesPerTab).AppendLine(text);
            return this;
        }

        public void Indent() { tabs += 1; }
        public void Outdent() { tabs -= 1; }
        public override string ToString() => builder.ToString();
    }
}