using System;
using System.Collections.Generic;
using System.Text;

namespace QueryProcessor
{
    public class RelTable
    {
        private List<RelTableItem> relTableItems = new List<RelTableItem>();
        public RelTable()
        {
            initialize();
        }
        void initialize()
        {
            relTableItems.Add(new RelTableItem("Calls", 2, new List<RelationArgumentType> { RelationArgumentType.Procedure,
                RelationArgumentType.Discard,RelationArgumentType.String }, new List<RelationArgumentType> { RelationArgumentType.Procedure,
                RelationArgumentType.Discard,RelationArgumentType.String }));

        }
       public  bool ValidateRelation(string relationName, int argumentsCount, params RelationArgumentType[] argumentTypes)
        {
            throw new NotImplementedException();
        }
    }
}
