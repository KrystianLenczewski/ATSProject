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
        CONST,
        IF,
        CALL,
        NULL
    }

    public enum OperationsType
    {
        ADD = ExpressionType.ADD
    }

    public enum SpecialType
    {
        STMTLST = ExpressionType.STMTLST
    }

    public enum WithNameType
    {
        PROCEDURE = ExpressionType.PROCEDURE
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

        public ExpressionModel(FactorType type, string name)
        {
            Type = (ExpressionType)type;
            Name = name;
        }

        public ExpressionModel(StatementType type, int line)
        {
            Type = (ExpressionType)type;
            Line = line;
        }

        public ExpressionModel(WithNameType type, string name)
        {
            Type = (ExpressionType)type;
            Name = name;
        }

        public ExpressionModel(SpecialType type, int line = 0)
        {
            Type = (ExpressionType)type;
            Line = line;
        }

        public ExpressionModel(StatementType type, string name, int line)
        {
            Type = (ExpressionType)type;
            Name = name;
            Line = line;
        }

        public ExpressionModel(OperationsType type, int line)
        {
            Type = (ExpressionType)type;
            Line = line;
        }
    }
}
