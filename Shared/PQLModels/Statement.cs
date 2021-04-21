using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.PQLModels
{
    public class Statement
    {
        public int ProgramLine { get; set; }
        public Statement()
        {

        }
        public Statement(int programLine)
        {
            ProgramLine = programLine;
        }
    }
}
