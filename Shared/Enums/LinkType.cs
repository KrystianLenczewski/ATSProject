using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    enum LinkType
    {
        FirstChild,
        RightSibling,
        Parent,
        Parent_Star,
        Follow,
        Follow_Star,
        Uses,
        Modifies
    }
}
