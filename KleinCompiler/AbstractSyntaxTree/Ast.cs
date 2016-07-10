﻿using System;

namespace KleinCompiler.AbstractSyntaxTree
{
    public abstract class Ast
    {
        public override bool Equals(object obj)
        {
            var token = obj as Ast;
            if (token == null)
                return false;

            if (this.GetType() != obj.GetType())
                return false;

            return true;
        }

        public abstract void Accept(IAstVisitor visior);

        public override string ToString()
        {
            return $"{GetType().Name}";
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public abstract TypeValidationResult CheckType();

        private KType? type;
        public KType Type
        {
            get
            {
                if (type == null)
                {
                    var result = CheckType();
                    if(result.HasError)
                        throw new Exception(result.Message);
                    type = result.Type;
                }
                return type.Value;
            }
            protected set { type = value; }
        }
    }
}