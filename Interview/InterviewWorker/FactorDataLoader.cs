using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Interview.DBWorker;

namespace Interview.InterviewWorker
{
    public enum GetFactorData
    {
        Factors
    }
    
    class FactorDataLoader : DataLoader
    {
        private readonly bool _isConnected;
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
            }
            catch (Exception exp)
            {
                throw new Exception(exp.ToString());
            }
        }
        
        public override DataTable GetDataTable(Enum getDataEnum)
        {
            var getData = (GetFactorData) getDataEnum;
            if (_isConnected)
            {
                 _respondentId = GetCurrRespondentId();
                 _themeId = GetCurrThemeId();
                 if ((_respondentId > 0) & (_themeId > 0))
                 {
                     if (getData == GetFactorData.Factors)
                     {
                         return GetFactorsNameAndScore();
                     }
                 }
            }
            return null;
        }
        
        public override void SetDataTable(Enum setDataEnum)
        {
            throw new NotImplementedException();
            
        }

        private DataTable GetFactorsNameAndScore()
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


        private DataTable GetFactorScore(IEnumerable<int> factorIdCollection)
        {
            
        }

        private int GetFactorScoreByFactorId(int factorId)
        {
            var query = "select Factors.name FactorName, " +
                                "Questions.name QuestionName, " +
                                "FactorsValue.factor_dependence_id factor_id, " +
                                "FactorsValue.digit, " +
                                "Operations.name OperationName " +
                        "from main.FactorsValue, " +
                             "main.Factors " +
                        "left join main.Operations on Operations.id = FactorsValue.operation_id " +
                        "left join main.Questions on Questions.id = FactorsValue.question_dependence_id " +
                        "where Factors.id = FactorsValue.factor_id " +
                              "and FactorsValue.factor_id = " + factorId;
            try
            {
                var factorDataRow = DbConnection.SelectFromDb(query);
                var modifiedDataRow = TryGetScoreOfQuestionAndFactors(factorDataRow);
            }
            catch (Exception exp)
            {
                throw new Exception("GetFactorScoreByFactorId: " + exp);
            }

        }

        private DataTable TryGetScoreOfQuestionAndFactors(DataTable table)
        {
            var modifiedQuestionRow = TryGetScoreOfQuestion(table, "QuestionName");
            var modifiedFactorsRow = TryGetScoreOfFactors(modifiedQuestionRow, "factor_id");

        }

        private DataTable TryGetScoreOfQuestion(DataTable table, string questionRowName)
        {
            if (table.Columns.Contains(questionRowName))
            {
                var modifiedQuestionDataTable = table;
                for (int i = 0; i < table.Rows.Count; i ++)
                {
                    var questionName = table.Rows[i][questionRowName];
                    modifiedQuestionDataTable.Rows[i][questionRowName] =
                        InterView.GetScoreByQuestionName(questionName.ToString());
                }
                return modifiedQuestionDataTable;
            }
            throw new Exception("TryGetScoreOfQuestion: Can't find need row!");
        }

        private DataRow TryGetScoreOfFactors(DataTable table, string factorRowName)
        {
            if (table.Columns.Contains(factorRowName))
            {
                var modifiedFactorData = table;
                
            }
            throw new Exception("TryGetScoreOfFactors: Can't find need row!");
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
