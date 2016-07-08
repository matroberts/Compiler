using KleinCompiler;
using KleinCompiler.AbstractSyntaxTree;
using NUnit.Framework.Constraints;

namespace KleinCompilerTests
{
    public class EqualAstConstraint : Constraint
    {
        private readonly NUnitEqualityComparer _comparer = new NUnitEqualityComparer();
        private readonly object _expected;
        private Tolerance _tolerance = Tolerance.Default;

        public EqualAstConstraint(object expected) : base(expected)
        {
            _expected = expected;
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            return new EqualAstConstraintResult(this, actual, _comparer.AreEqual(_expected, actual, ref _tolerance));
        }
    }

    public class EqualAstConstraintResult : ConstraintResult
    {
        private readonly object _expectedValue;

        public EqualAstConstraintResult(EqualAstConstraint constraint, object actual, bool hasSucceeded)
            : base(constraint, actual, hasSucceeded)
        {
            this._expectedValue = constraint.Arguments[0];
        }

        public override void WriteMessageTo(MessageWriter writer)
        {
            DisplayDifferences(writer, _expectedValue, ActualValue, 0);
        }


        private void DisplayDifferences(MessageWriter writer, object expected, object actual, int depth)
        {
            if (expected is Ast && actual is Ast)
                DisplayAstDifferences(writer, (Ast)expected, (Ast)actual);
            else
                writer.DisplayDifferences(expected, actual);
        }

        private void DisplayAstDifferences(MessageWriter writer, Ast expected, Ast actual)
        {
            writer.WriteLine("Expected:");
            writer.Write(PrettyPrinter.ToString(expected));
            writer.WriteMessageLine("");

            writer.WriteLine("But was:");
            writer.Write(PrettyPrinter.ToString(actual));
            writer.WriteMessageLine("");
        }
    }

    public class Is : NUnit.Framework.Is
    {
        public static EqualAstConstraint AstEqual(object expected)
        {
            return new EqualAstConstraint(expected);
        }
    }
}