using PKB;
using QueryProcessor.Enums;
using QueryProcessor.Extensions;
using QueryProcessor.QueryTreeNodes;
using Shared.PQLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryProcessor.QueryProcessing
{
    public class ResultProcessor
    {
        private List<Statement> _statements;
        private List<Procedure> _procedures;
        private List<Variable> _variables;
        private readonly List<int> _constanst;
        private bool _boolResult = true;

        public ResultProcessor(IPKBStore pkbStore)
        {
            _statements = pkbStore.GetAllStatements();
            _procedures = pkbStore.GetAllProcedures();
            _variables = pkbStore.GetAllVariables();
            _constanst = pkbStore.GetAllConstants();
        }

        public string GetResult(SynonimType[] synonimTypes)
        {
            throw new NotImplementedException();
        }

        public void AddRelationResult(bool boolResult) => _boolResult = _boolResult && boolResult;

        public void AddRelationResult<T>(SynonimType synonimType, List<T> designAbstractions)
        {
            if(synonimType == SynonimType.Procedure)
            {
                List<string> procedureNames = designAbstractions.Select(s => s as Procedure).Select(s=>s.Name).Where(w => !string.IsNullOrEmpty(w)).ToList();
                _procedures = _procedures.Where(w => procedureNames.Contains(w.Name)).ToList();
            }
            else if(synonimType == SynonimType.Variable)
            {
                List<string> variableNames = designAbstractions.Select(s => s as Variable).Select(s => s.Name).Where(w => !string.IsNullOrEmpty(w)).ToList();
                _variables = _variables.Where(w => variableNames.Contains(w.Name)).ToList();
            }
            else if(synonimType == SynonimType.Statement)
            {
                List<int> statementProgLines = designAbstractions.Select(s => s as Statement).Select(s => s.ProgramLine).Where(w => w != 0).ToList();
                _statements = _statements.Where(w => statementProgLines.Contains(w.ProgramLine)).ToList();
            }
        }

        public void ApplyAttributesRelatedWithCondition(List<AttributeNode> attributeNodes)
        {
            foreach(AttributeNode leftAttributeNode in attributeNodes ?? new List<AttributeNode>())
            {
                if (leftAttributeNode?.AttributeValue is AttributeNode rightAttributeNode)
                {
                    if((leftAttributeNode.AttributeType.IsEqualIgnoreCase(AttributeType.VARNAME) && rightAttributeNode.AttributeType.IsEqualIgnoreCase(AttributeType.PROCNAME)) || (rightAttributeNode.AttributeType.IsEqualIgnoreCase(AttributeType.VARNAME) && leftAttributeNode.AttributeType.IsEqualIgnoreCase(AttributeType.PROCNAME)))
                    {
                        List<string> variableNames = _variables.Select(s => s.Name).ToList();
                        List<string> procedureNames = _procedures.Select(s => s.Name).ToList();
                        List<string> commonNames = variableNames.Where(w => procedureNames.Contains(w)).ToList();
                        _variables = _variables.Where(w => commonNames.Contains(w.Name)).ToList();
                        _procedures = _procedures.Where(w => commonNames.Contains(w.Name)).ToList();
                    }
                    if((leftAttributeNode.AttributeType.IsEqualIgnoreCase(AttributeType.STMT) && rightAttributeNode.AttributeType.IsEqualIgnoreCase(AttributeType.VALUE)) || (rightAttributeNode.AttributeType.IsEqualIgnoreCase(AttributeType.STMT) && leftAttributeNode.AttributeType.IsEqualIgnoreCase(AttributeType.VALUE)))
                    {
                        _statements = _statements.Where(w => _constanst.Contains(w.ProgramLine)).ToList();
                    }
                }
            }
        }
    }
}
