using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor.Infrastructure
{
    internal struct QueryElement
    {
        internal const string Select = "select";
        internal const string SuchThat = "such that";
        internal const string With = "with";
        internal const string Pattern = "pattern";
        internal const string And = "and";

        public static string GetKeyword(string word)
        {
            string keyword = "";
            switch (word)
            {
                case Select:
                    keyword = Select;
                    break;
                case SuchThat:
                    keyword = SuchThat;
                    break;
                case With:
                    keyword = With;
                    break;
                case Pattern:
                    keyword = Pattern;
                    break;
                default:
                    break;
            }
            return keyword;
        }
    }
}
