using Shared;
using System.Collections.Generic;

namespace PKB
{
    public class PKBStore : IPKBStore
    {
        public List<KeyValuePair<ExpressionModel, ExpressionModel>> ModifiesList { get; set; } = new List<KeyValuePair<ExpressionModel, ExpressionModel>>();
        public List<KeyValuePair<ExpressionModel, ExpressionModel>> FollowsList { get; set; } = new List<KeyValuePair<ExpressionModel, ExpressionModel>>();
        public List<KeyValuePair<ExpressionModel, ExpressionModel>> UsesList { get; set; } = new List<KeyValuePair<ExpressionModel, ExpressionModel>>();
        public List<ParentModel> ParentList { get; set; } = new List<ParentModel>();

        public override string ToString()
        {
            string x = "";
            foreach (var y in ModifiesList)
            {
                x += $"mod:\t{y.Key.Line} {y.Key.Name} {y.Key.Type} - {y.Value.Line} {y.Value.Name} {y.Value.Type}\n";
            }

            foreach (var y in ParentList)
            {
                x += $"parent:\t{y.Parent.Line} {y.Parent.Name} {y.Parent.Type} - {y.Child.Line} {y.Child.Name} {y.Child.Type} - {y.Index}\n";
            }

            foreach (var y in UsesList)
            {
                x += $"uses:\t{y.Key.Line} {y.Key.Name} {y.Key.Type} - {y.Value.Line} {y.Value.Name} {y.Value.Type}\n";
            }

            foreach (var y in FollowsList)
            {
                x += $"follows:\t{y.Key.Line} {y.Key.Name} {y.Key.Type} - {y.Value.Line} {y.Value.Name} {y.Value.Type}\n";
            }

            return x;
        }

        // TODO: PROC_TABLE & VAR_TABLE
    }
}
