﻿namespace Shared
{
    public enum ExpressionType
    {
        ASSIGN,
        WHILE,
        ADD,
        STMTLST,
        PROCEDURE,
        VAR,
        CONST,
        IF,
        CALL
    }

    public enum OperationsType
    {
        ADD = ExpressionType.ADD
    }

    public enum SpecialType
    {
        STMTLST = ExpressionType.STMTLST,
        PROCEDURE = ExpressionType.PROCEDURE,
    }

    public enum StatementType
    {
        ASSIGN = ExpressionType.ASSIGN,
        WHILE = ExpressionType.WHILE,
        IF = ExpressionType.IF,
        CALL = ExpressionType.CALL
    }

    public enum FactorType
    {
        VAR = ExpressionType.VAR,
        CONST = ExpressionType.CONST,
    }

    public class ExpressionModel
    {
        public ExpressionType Type { get; private set; }
        public string Name { get; private set; }
        public int Line { get; set; }

        public ExpressionModel(FactorType type, string name, int line)
        {
            Type = (ExpressionType)type;
            Name = name;
            Line = line;
        }

        public ExpressionModel(StatementType type, int line)
        {
            Type = (ExpressionType)type;
            Line = line;
        }

        public ExpressionModel(SpecialType type, int line)
        {
            Type = (ExpressionType)type;
            Line = line;
        }

        public ExpressionModel(OperationsType type, int line)
        {
            Type = (ExpressionType)type;
            Line = line;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                ExpressionModel p = (ExpressionModel)obj;
                return (Type == p.Type) && (Line == p.Line);
            }
        }
    }
}
