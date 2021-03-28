using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    class StatementIf
    {
        public Variable ControlVariable { get; set; }
        public List<Statement> ThenStatementList { get; set; }
        public List<Statement> ElseStatementList { get; set; }
    }
}
