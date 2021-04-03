using QueryProcessor.Enums;
using QueryProcessor.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryProcessor
{
    internal class SymbolTable
    {
        Dictionary<string, RelationArgumentType> querySymbols = new Dictionary<string, RelationArgumentType>();

        public SymbolTable(string query)
        {
            querySymbols.Add("BOOLEAN", RelationArgumentType.BOOLEAN);
            InitializeSymbolTable(query);
        }

        private void InitializeSymbolTable(string query)
        {
            List<string> splitedQuery = query.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            splitedQuery.ForEach(x => x.Trim().ToLower());

            int selectIndex = splitedQuery.IndexOf(QueryElement.Select.ToLower());
            if (selectIndex == -1) return;
            string symbolDeclarationString = String.Join(' ', splitedQuery.Take(selectIndex));
            List<string> splitedSymbolDeclarations = symbolDeclarationString.Split(';',StringSplitOptions.RemoveEmptyEntries).ToList();


            for(int i=0; i<splitedSymbolDeclarations.Count; i++)
            {
                //assign a1,a2
                List<string> splitedSymbolDeclaration = splitedSymbolDeclarations[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
                splitedSymbolDeclaration.ForEach(x => x.Trim().ToLower());
                if (splitedSymbolDeclaration.Count != 2) throw new Exception("Nieprawidłowy format declaracji.");
                RelationArgumentType declarationType = GetTypeForDeclaration(splitedSymbolDeclaration[0]);
                List<string> declarationNames= splitedSymbolDeclaration[1].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (string declarationName in declarationNames)
                {
                    querySymbols.Add(declarationName, declarationType);
                }

                //assign
                //a1,a2

                //assing
                //a1,
                //a2
            }


        }

        internal bool IsSymbolExist(string declarationName)
        {
            return querySymbols.ContainsKey(declarationName);
        }

        private RelationArgumentType GetTypeForDeclaration(string declarationName)
        {
            if (declarationName.Equals("procedure", StringComparison.OrdinalIgnoreCase)) return RelationArgumentType.Procedure;
            else if (declarationName.Equals("string", StringComparison.OrdinalIgnoreCase)) return RelationArgumentType.String;
            else if (declarationName.Equals("prog_line", StringComparison.OrdinalIgnoreCase)) return RelationArgumentType.Prog_line;
            else if (declarationName.Equals("stmt", StringComparison.OrdinalIgnoreCase)) return RelationArgumentType.Statement;
            else if (declarationName.Equals("assign", StringComparison.OrdinalIgnoreCase)) return RelationArgumentType.Assign;
            else if (declarationName.Equals("variable", StringComparison.OrdinalIgnoreCase)) return RelationArgumentType.Variable;
            else if (declarationName.Equals("while", StringComparison.OrdinalIgnoreCase)) return RelationArgumentType.While;
            else if (declarationName.Equals("constant", StringComparison.OrdinalIgnoreCase)) return RelationArgumentType.Constant;
            else if (declarationName.Equals("if", StringComparison.OrdinalIgnoreCase)) return RelationArgumentType.If;
            else throw new Exception("Nie rozpoznano typu.");

        }

        

    }
}
