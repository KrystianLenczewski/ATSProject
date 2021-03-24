using System.Collections.Generic;

namespace Shared
{
    public enum StatementType
    {
        ASSIGN,
        WHILE,
        ADD,
        CONST,
        VAR,
        STMTLIST
    }

    public class Statement
    {
        public StatementType Type { get; set; }
        public List<Statement> StmtLst { get; set; }
    }
}
