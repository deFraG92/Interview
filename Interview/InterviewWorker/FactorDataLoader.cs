using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using Interview.DBWorker;

namespace Interview.InterviewWorker
{
    public enum SetFactorData
    {
        Factors
    }

    internal class FactorDataLoader : DataLoader
    {
        private readonly bool _isConnected;
        private readonly string[] _factorValueFields = { "QuestionName", "Factor_id", "Digit", "OperationName" };
        private int _themeId;
        private int _respondentId;

        public FactorDataLoader(string tns)
        {
            try
            {
                if (DbConnection == null)
                {
                    DbConnection = SqliteDbConnection.GetSqliteDbWorker();
                }
                _isConnected = DbConnection.IsConnected() || DbConnection.ConnectToDb(tns);
                RespondentAndThemeIdInit();
            }
            catch (Exception exp)
            {
                throw new Exception(exp.ToString());
            }
        }

        public override DataTable GetDataTable(Enum getDataEnum)
        {
            throw new NotImplementedException();
        }
        
        public override void SetDataTable(Enum setDataEnum)
        {
            if (_isConnected)
            {
                var setData = (SetFactorData)setDataEnum;
                if ((_respondentId > 0) & (_themeId > 0))
                {
                    if (setData == SetFactorData.Factors)
                    {
                        SetFactorsNameAndScore();
                    }
                }
            }
        }

        private void SetFactorsNameAndScore()
        {
            if (InterView.GetInterviewCompleteness())
            {
                var factorDependence = CheckForFactorDependence();
                var factorIdCollection = !factorDependence
                    ? GetFactorsIdWithoutFactorDependence()
                    : GetFactorsIdWithFactorDependence();
                
            }
        }

        private bool CheckForFactorDependence()
        {
            var query = " select count(distinct FactorsValue.factor_dependence_id) " +
                        " from main.FactorsValue," +
                             " main.Factors" +
                        " where FactorsValue.factor_id = Factors.id" +
                              " and Factors.theme_id = " + _themeId;
            try
            {
                var dependRow = DbConnection.SelectFromDb(query);
                return (Int32)dependRow.Rows[0][0] > 0;
            }
            catch (Exception exp)
            {
                throw new Exception(exp.ToString());
            }

        }
        
        private IEnumerable<int> GetFactorsIdWithoutFactorDependence()
        {
            var query = " select distinct FactorsValue.factor_id " +
                        " from main.Factors, " +
                             " main.FactorsValue " +
                        " where Factors.theme_id = " + _themeId;
            try
            {
                var factorIdRow = DbConnection.SelectFromDb(query);
                return GetEnumerableCollection(factorIdRow);
            }
            catch (Exception exp)
            {
                throw new Exception(exp.ToString());
            }
        }

        private IEnumerable<int> FindFactorChildsAndGetCollection(int factorId)
        {
            var query = "with recursive Childs(id) as " +
                        "( " +
                        "values(" + factorId + ") " +
                        "union all " +
                        "select FactorsValue.factor_id " +
                        "from main.FactorsValue join Childs on Childs.id = FactorsValue.factor_dependence_id " +
                        ") " +
                        "select distinct Childs.id from Childs";
            try
            {
                var factorChildRow = DbConnection.SelectFromDb(query);
                return GetEnumerableCollection(factorChildRow);
            }
            catch (Exception exp)
            {
                throw new Exception("FindFactorChildAndSetItIntoFactorCollection: " + exp);
            }
        }
        
        private IEnumerable<int> GetFactorsIdWithFactorDependence()
        {
            var factorsIdWithoutDependence = GetFactorsIdWithoutFactorDependence();
            var factorCollectionId = new List<int>(factorsIdWithoutDependence);
            foreach (var factorId in factorsIdWithoutDependence)
            {
                var factorChildsCollection = FindFactorChildsAndGetCollection(factorId);
                foreach (var elem in factorChildsCollection.Where(x => !factorCollectionId.Contains(x)))
                {
                    factorCollectionId.Add(elem);
                }
            }
            return factorCollectionId;
        }

        private void SetFactorScore(IEnumerable<int> factorIdCollection)
        {
            foreach (var factorId in factorIdCollection)
            {
                var factorDataRow = GetFactorScoreByFactorId(factorId);
            }
        }

        private DataTable GetFactorScoreByFactorId(int factorId)
        {
            var query = "select Factors.name FactorName, " +
                                "Questions.name "+ _factorValueFields[0] +", " +
                                "FactorsValue.factor_dependence_id "+ _factorValueFields[1] +", " +
                                "FactorsValue.digit "+ _factorValueFields[2] +", " +
                                "Operations.name "+ _factorValueFields[3] +
                        "from main.FactorsValue, " +
                             "main.Factors " +
                        "left join main.Operations on Operations.id = FactorsValue.operation_id " +
                        "left join main.Questions on Questions.id = FactorsValue.question_dependence_id " +
                        "where Factors.id = FactorsValue.factor_id " +
                              "and FactorsValue.factor_id = " + factorId;
            try
            {
                var factorDataRow = DbConnection.SelectFromDb(query);
                var modifiedQuestionRow = TryGetScoreOfQuestion(factorDataRow, _factorValueFields[0]);
                var modifiedFactorsRow = TryGetScoreOfFactors(modifiedQuestionRow, _factorValueFields[1]);
                


           }
            catch (Exception exp)
            {
                throw new Exception("GetFactorScoreByFactorId: " + exp);
            }
            return null;
        }

        private DataTable TryGetScoreOfQuestion(DataTable table, string questionRowName)
        {
            if (table.Columns.Contains(questionRowName))
            {
                var modifiedQuestionDataTable = table;
                for (int i = 0; i < table.Rows.Count; i ++)
                {
                    var questionName = table.Rows[i][questionRowName];
                    if (questionName != null)
                    {
                        modifiedQuestionDataTable.Rows[i][questionRowName] =
                           InterView.GetScoreByQuestionName(questionName.ToString());
                    }
                }
                return modifiedQuestionDataTable;
            }
            throw new Exception("TryGetScoreOfQuestion: Can't find need row!");
        }

        private DataTable TryGetScoreOfFactors(DataTable table, string factorRowName)
        {
            if (table.Columns.Contains(factorRowName))
            {
                var modifiedFactorDataTable = table;
                for (int i = 0; i < table.Rows.Count; i ++)
                {
                    var factorId = table.Rows[i][factorRowName];
                    if (factorId != null)
                    {
                        var factorName = GetFactorNameById((int)factorId);
                        modifiedFactorDataTable.Rows[i][factorRowName] =
                            FactorAnalize.GetFactorScoreByFactorName(factorName);
                    }
                }
            }
            throw new Exception("TryGetScoreOfFactors: Can't find need row!");
        }

        private void GetParseStringFromDataTable(DataTable table, string[] needFactorFields)
        {
                   
        }


        private void RespondentAndThemeIdInit()
        {
            _respondentId = GetCurrRespondentId();
            _themeId = GetCurrThemeId();
        }
        
        private string GetFactorNameById(int factorId)
        {
            var query = "select " +
                        "from main.Factors " +
                        "where Factors.id = " + factorId;
            try
            {
                var factorIdRow = DbConnection.SelectFromDb(query);
                return factorIdRow.Rows.Count > 0 ? factorIdRow.Rows[0][0].ToString() : null;
            }
            catch (Exception exp)
            {
                throw new Exception("GetFactorNameById: " + exp);
            }
        }
        
        private int GetCurrRespondentId()
        {
            var query = " select Respondents.id " +
                        " from main.Respondents " +
                        " where Respondents.name = " + InterView.GetRespondentName();
            try
            {
                var respondentRow = DbConnection.SelectFromDb(query);
                if (respondentRow.Rows.Count > 0)
                {
                    return (Int32) respondentRow.Rows[0][0];
                }
                return -1;
            }
            catch (Exception exp)
            {
                throw new Exception(exp.ToString());
            }
        }

        private int GetCurrThemeId()
        {
            var query = " select Themes.id " +
                        " from main.Themes " +
                        " where Themes.name = " + InterView.GetInterviewTheme();
            try
            {
                var themeRow = DbConnection.SelectFromDb(query);
                if (themeRow.Rows.Count > 0)
                {
                    return (Int32)themeRow.Rows[0][0];
                }
                return -1;
            }
            catch (Exception exp)
            {
                throw new Exception(exp.ToString());
            }
        }

        private IEnumerable<int> GetEnumerableCollection(DataTable table)
        {
            var resultCollection = new List<int>();
            if (table.Rows.Count > 0)
            {
                resultCollection.AddRange(table.Rows.Cast<int>());
            }
            return resultCollection;
        }

        
    }
}
