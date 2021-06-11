using Shared;
using System.Collections.Generic;

namespace PKB
{
    public class PKBStore : IPKBStore
    {
        public List<KeyValuePair<ExpressionModel, ExpressionModel>> ModifiesList { get; set; } = new List<KeyValuePair<ExpressionModel, ExpressionModel>>(); // niedomaga
        public List<KeyValuePair<ExpressionModel, ExpressionModel>> FollowsList { get; set; } = new List<KeyValuePair<ExpressionModel, ExpressionModel>>(); // chyba done
        public List<KeyValuePair<ExpressionModel, ExpressionModel>> UsesList { get; set; } = new List<KeyValuePair<ExpressionModel, ExpressionModel>>(); // chyba done
        public List<KeyValuePair<ExpressionModel, ExpressionModel>> NextList { get; set; } = new List<KeyValuePair<ExpressionModel, ExpressionModel>>(); // niedomaga
        public List<KeyValuePair<ExpressionModel, ExpressionModel>> AffectsList { get; set; } = new List<KeyValuePair<ExpressionModel, ExpressionModel>>(); // removed
        public List<KeyValuePair<ExpressionModel, ExpressionModel>> CallsList { get; set; } = new List<KeyValuePair<ExpressionModel, ExpressionModel>>(); // chyba done
        public List<ParentModel> ParentList { get; set; } = new List<ParentModel>(); // chyba done

        public List<ExpressionModel> StatementList { get; set; } = new List<ExpressionModel>(); // done
        public List<string> ProcList { get; set; } = new List<string>(); // done
        public List<string> VarList { get; set; } = new List<string>(); // done
        public List<string> ConstList { get; set; } = new List<string>(); // done

        public override string ToString()
        {
            string x = "";
            /*foreach (var y in ModifiesList)
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

            /*foreach (var y in FollowsList)
            {
                x += $"follows:\t{y.Key.Line} {y.Key.Name} {y.Key.Type} - {y.Value.Line} {y.Value.Name} {y.Value.Type}\n";
            }

            foreach (var y in StatementList)
            {
                x += $"alls:\t{y.Line} {y.Type}\n";
            }

            foreach (var y in ProcList)
            {
                x += $"procs:\t{y}\n";
            }

            foreach (var y in VarList)
            {
                x += $"vars:\t{y}\n";
            }

            foreach (var y in CallsList)
            {
                x += $"calls:\t{y.Key.Name}\t{y.Value.Name}\n";
            }

            foreach (var y in ConstList)
            {
                x += $"consts:\t{y}\n";
            }

            foreach (var y in NextList)
            {
                x += $"nexts:\t{y.Key.Line} - {y.Key.Type}\t{y.Value.Line} - {y.Value.Type}\n";
            }*/

            return x;
        }
    }
}
