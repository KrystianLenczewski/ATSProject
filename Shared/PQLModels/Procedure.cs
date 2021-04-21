using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.PQLModels
{
    public class Procedure
    {
        public string Name { get; set; }
        public Procedure()
        {

        }
        public Procedure(string name)
        {
            Name = name;
        }
    }
}
