using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public enum EntityType
    {
        Program,
        Procedure,
        Assign,
        Call,
        StatementIf,
        StatementWhile,
        OperatorPlus,
        OperatorMinus,
        Variable,
        Constant
    }
}
