using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.PQLModels
{
    public class Variable
    {
        public string Name { get; set; }
        public Variable()
        {

        }
        public Variable(string name)
        {
            Name = name; 
        }
    }
}
