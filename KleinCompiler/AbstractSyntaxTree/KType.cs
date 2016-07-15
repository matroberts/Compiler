using System.Collections.Generic;
using System.Linq;

namespace KleinCompiler.AbstractSyntaxTree
{
    public abstract class KType
    {

    }

    public abstract class PrimitiveType : KType
    {
        
    }

    public class BooleanType : PrimitiveType
    {
        public override bool Equals(object obj)
        {
            var type = obj as BooleanType;
            return type != null;
        }

        public override string ToString()
        {
            return "boolean";
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }

    public class IntegerType : PrimitiveType
    {
        public override bool Equals(object obj)
        {
            var type = obj as IntegerType;
            return type != null;
        }

        public override string ToString()
        {
            return "integer";
        }
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }

    public class FunctionType : KType
    {
        public PrimitiveType ReturnType { get; }
        public List<PrimitiveType> Args { get; }

        public FunctionType(PrimitiveType returnType, params PrimitiveType[] args)
        {
            ReturnType = returnType;
            Args = args.ToList();
        }

        public bool CheckArgs(List<PrimitiveType> args)
        {
            if (Args.Count != args.Count)
                return false;

            for (int i = 0; i < Args.Count; i++)
            {
                if (Args[i].Equals(args[i]) == false)
                    return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            var type = obj as FunctionType;
            if(type == null)
                return false;

            if (ReturnType.Equals(type.ReturnType) == false)
                return false;

            return CheckArgs(type.Args);
        }

        public override string ToString()
        {
            return $"({string.Join(", ", Args)}) : {ReturnType}";
        }
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}