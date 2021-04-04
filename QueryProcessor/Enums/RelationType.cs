using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor.Enums
{
    public enum RelationType
    {
        MODIFIES,
        USES,
        PARENT,
        PARENT_STAR,
        FOLLOWS,
        FOLLOWS_STAR,
        CALLS,
        CALLS_STAR,
    }
}
