using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor
{
    public enum RelationArgumentType
    {
        Procedure,
        String,
        Prog_line,//linia programu
        Statement,
        Assign,
        Discard,//_
        Variable,
        Integer
    }
}
