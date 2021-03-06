using Shared;
using System.Collections.Generic;

namespace PKB
{
    public interface IPKBStore
    {
        List<KeyValuePair<ExpressionModel, ExpressionModel>> ModifiesList { get; set; }
        List<KeyValuePair<ExpressionModel, ExpressionModel>> FollowsList { get; set; }
        List<KeyValuePair<ExpressionModel, ExpressionModel>> UsesList { get; set; }
        List<KeyValuePair<ExpressionModel, ExpressionModel>> NextList { get; set; }
        List<KeyValuePair<ExpressionModel, ExpressionModel>> AffectsList { get; set; }
        List<KeyValuePair<ExpressionModel, ExpressionModel>> CallsList { get; set; } 
        List<ParentModel> ParentList { get; set; }
        List<ExpressionModel> StatementList { get; set; }

        List<string> ProcList { get; set; }
        List<string> VarList { get; set; } 
        List<string> ConstList { get; set; }
    }
}
