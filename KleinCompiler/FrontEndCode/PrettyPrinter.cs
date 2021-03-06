﻿using System;
using System.Text;
using KleinCompiler.AbstractSyntaxTree;

namespace KleinCompiler.FrontEndCode
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
            builder.AppendLine($"Definition({node.Name})");
            builder.Indent();
            node.TypeDeclaration.Accept(this);
            builder.AppendLine($"Formals");
            builder.Indent();
            foreach (var formal in node.Formals)
            {
                formal.Accept(this);
            }
            builder.Outdent();
            node.Body.Accept(this);
            builder.Outdent();
        }

        public void Visit(Formal node)
        {
            builder.AppendLine($"Formal({node.Name})");
            builder.Indent();
            node.TypeDeclaration.Accept(this);
            builder.Outdent();
        }

        public void Visit(BooleanTypeDeclaration node)
        {
            builder.AppendLine($"Type(Boolean)");
        }

        public void Visit(IntegerTypeDeclaration node)
        {
            builder.AppendLine($"Type(Integer)");
        }

        public void Visit(Body node)
        {
            builder.AppendLine("Body");
            builder.Indent();
            foreach (var print in node.Prints)
            {
                print.Accept(this);
            }
            builder.AppendLine("Expr");
            builder.Indent();
            node.Expr.Accept(this);
            builder.Outdent();
            builder.Outdent();
        }

        public void Visit(Print node)
        {
            builder.AppendLine("Print");
            builder.Indent();
            node.Expr.Accept(this);
            builder.Outdent();
        }

        public void Visit(IfThenElse node)
        {
            builder.AppendLine("If");
            builder.Indent();
            node.IfExpr.Accept(this);
            builder.Outdent();
            builder.AppendLine("Then");
            builder.Indent();
            node.ThenExpr.Accept(this);
            builder.Outdent();
            builder.AppendLine("Else");
            builder.Indent();
            node.ElseExpr.Accept(this);
            builder.Outdent();
        }

        public void Visit(LessThanOperator node)
        {
            builder.AppendLine($"LessThan");
            builder.Indent();
            node.Left.Accept(this);
            node.Right.Accept(this);
            builder.Outdent();
        }

        public void Visit(EqualsOperator node)
        {
            builder.AppendLine($"Equals");
            builder.Indent();
            node.Left.Accept(this);
            node.Right.Accept(this);
            builder.Outdent();
        }

        public void Visit(OrOperator node)
        {
            builder.AppendLine($"Or");
            builder.Indent();
            node.Left.Accept(this);
            node.Right.Accept(this);
            builder.Outdent();
        }

        public void Visit(PlusOperator node)
        {
            builder.AppendLine($"Plus");
            builder.Indent();
            node.Left.Accept(this);
            node.Right.Accept(this);
            builder.Outdent();
        }

        public void Visit(MinusOperator node)
        {
            builder.AppendLine($"Minus");
            builder.Indent();
            node.Left.Accept(this);
            node.Right.Accept(this);
            builder.Outdent();
        }

        public void Visit(AndOperator node)
        {
            builder.AppendLine($"And");
            builder.Indent();
            node.Left.Accept(this);
            node.Right.Accept(this);
            builder.Outdent();
        }

        public void Visit(TimesOperator node)
        {
            builder.AppendLine($"Times");
            builder.Indent();
            node.Left.Accept(this);
            node.Right.Accept(this);
            builder.Outdent();
        }

        public void Visit(DivideOperator node)
        {
            builder.AppendLine($"Divide");
            builder.Indent();
            node.Left.Accept(this);
            node.Right.Accept(this);
            builder.Outdent();
        }

        public void Visit(NotOperator node)
        {
            builder.AppendLine($"Not");
            builder.Indent();
            node.Right.Accept(this);
            builder.Outdent();
        }

        public void Visit(NegateOperator node)
        {
            builder.AppendLine($"Negate");
            builder.Indent();
            node.Right.Accept(this);
            builder.Outdent();
        }

        public void Visit(Identifier node)
        {
            builder.AppendLine($"Identifier({node.Value})");
        }

        public void Visit(BooleanLiteral node)
        {
            builder.AppendLine($"Boolean({node.Value})");
        }

        public void Visit(IntegerLiteral node)
        {
            builder.AppendLine($"Integer({node.Value})");
        }

        public void Visit(FunctionCall node)
        {
            builder.AppendLine($"FunctionCall({node.Name})");
            builder.Indent();
            foreach (var actual in node.Actuals)
            {
                actual.Accept(this);
            }
            builder.Outdent();
        }

        public void Visit(Actual node)
        {
            builder.AppendLine("Actual");
            builder.Indent();
            node.Expr.Accept(this);
            builder.Outdent();
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