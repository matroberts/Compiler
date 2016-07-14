using System.Collections.Generic;
using System.Linq;

namespace KleinCompiler.AbstractSyntaxTree
{
    public abstract class Type2
    {

    }

    public abstract class PrimitiveType : Type2
    {
        
    }

    public class BooleanType : PrimitiveType
    {
        public override bool Equals(object obj)
        {
            var type = obj as BooleanType;
            return type != null;
        }
    }

    public class IntegerType : PrimitiveType
    {
        public override bool Equals(object obj)
        {
            var type = obj as IntegerType;
            return type != null;
        }
    }

    public class FunctionType
    {
        private PrimitiveType ReturnType { get; }
        private List<PrimitiveType> Args { get; }

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
    }
}