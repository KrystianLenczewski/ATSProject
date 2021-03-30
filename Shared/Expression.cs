namespace Shared
{
    public enum ExpressionType
    {
        ASSIGN,
        WHILE,
        ADD,
        STMTLST,
        PROCEDURE,
        VAR,
        CONST
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
    }

    public enum FactorType
    {
        VAR = ExpressionType.VAR,
        CONST = ExpressionType.CONST,
    }

    public class Expression
    {
        public ExpressionType Type { get; private set; }
        public string Name { get; private set; }
        public int Index { get; set; }

        public Expression(FactorType type, string name)
        {
            Type = (ExpressionType)type;
            Name = name;
        }

        public Expression(StatementType type)
        {
            Type = (ExpressionType)type;
        }

        public Expression(StatementType type, int index)
        {
            Type = (ExpressionType)type;
            Index = index;
        }

        public Expression(SpecialType type)
        {
            Type = (ExpressionType)type;
        }

        public Expression(OperationsType type, int index)
        {
            Type = (ExpressionType)type;
            Index = index;
        }
    }
}
