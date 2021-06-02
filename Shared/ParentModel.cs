using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class ParentModel
    {
        public ExpressionModel Parent { get; private set; }
        public ExpressionModel Child { get; private set; }
        public int Index { get; set; }

        public ParentModel(ExpressionModel parent, ExpressionModel child, int index)
        {
            Parent = parent;
            Child = child;
            Index = index;
        }
    }
}
