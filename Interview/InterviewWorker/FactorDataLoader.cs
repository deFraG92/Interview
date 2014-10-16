using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        private bool _factorDependence;
        private int _maxInterviewNum;

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
                _maxInterviewNum = GetMaxInterviewNum();
                _factorDependence = CheckForFactorDependence();
                var factorIdCollection = !_factorDependence
                    ? GetFactorsIdWithoutFactorDependence()
                    : GetFactorsIdWithFactorDependence();
                SetFactorScore(factorIdCollection);
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
                return Convert.ToInt32(dependRow.Rows[0][0]) > 0;
            }
            catch (Exception exp)
            {
                throw new Exception(exp.ToString());
            }

        }

        private bool CheckForFactorDependence(int factorId)
        {
            var query = " select count(distinct FactorsValue.factor_dependence_id) " +
                        " from main.FactorsValue," +
                             " main.Factors" +
                        " where FactorsValue.factor_id = Factors.id " +
                              " and FactorsValue.factor_id = " + factorId + 
                              " and Factors.theme_id = " + _themeId;
            try
            {
                var dependRow = DbConnection.SelectFromDb(query);
                return Convert.ToInt32(dependRow.Rows[0][0]) > 0;
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
                        " where Factors.theme_id = " + _themeId + 
                                " and Factors.id = FactorsValue.factor_id" +
                                " and FactorsValue.factor_id not in " +
                                " ( select FactorsValue.factor_id " +
                                " from main.FactorsValue " +
                                " where FactorsValue.factor_dependence_id is not null ) " ;
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

        private IEnumerable<int> GetFactorsAndDependencesId()
        {
            var query = " select distinct FactorsValue.factor_id, FactorsValue.factor_dependence_id " +
                        " from main.Factors, " +
                             " main.FactorsValue " +
                        " where Factors.theme_id = " + _themeId +
                                " and Factors.id = FactorsValue.factor_id" +
                                " and FactorsValue.factor_dependence_id is not null"; ;

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

        private IEnumerable<int> GetOrderFactorList(int[] factorAndFactorDependencesCollection)
        {
            var flag = true;
            var counter = 0;
            const int colSize = 2;
            var rowSize = factorAndFactorDependencesCollection.Count() / colSize;
            var orderList = (List<int>)GetFactorsIdWithoutFactorDependence();
            while (flag)
            {
                for (int i = 0; i < rowSize; i++)
                {
                    var dependElem = factorAndFactorDependencesCollection[i * colSize + 1];
                    var baseElem = factorAndFactorDependencesCollection[i * colSize];
                    if (dependElem != -1)
                    {
                        if (orderList.Contains(dependElem))
                        {
                            if (!orderList.Contains(baseElem))
                            {
                                counter++;
                                orderList.Add(baseElem);
                            }
                            factorAndFactorDependencesCollection[i * colSize + 1] = -1;
                        }
                    }
                }
                if (counter == 0)
                {
                    flag = false;
                }
                else
                {
                    counter = 0;
                }
            }
            return orderList;
        }

        private IEnumerable<int> GetFactorsIdWithFactorDependence()
        {
            var factorCollectionId = GetFactorsAndDependencesId();
            var orderFactorIdList = GetOrderFactorList(factorCollectionId.ToArray());
            return orderFactorIdList;
        }
        
        private void SetFactorScore(IEnumerable<int> factorIdCollection)
        {
            foreach (var factorId in factorIdCollection)
            {
                // Вытягиваем имя вопроса, ид фактора, число, имя операции
                var factorDataRow = GetFactorDataByFactorId(factorId);
                var modifiedQuestionRow = TryGetScoreOfQuestion(factorDataRow, _factorValueFields[0]);
                // устанавливаем набор требуемых полей
                var needFactorsFields = !_factorDependence ? new[] {_factorValueFields[0], _factorValueFields[2]} :
                    new[] {_factorValueFields[0], _factorValueFields[1], _factorValueFields[2]};
                // Проверяем зависимость от факторов
                var haveFactorDependence = CheckForFactorDependence(factorId);
                var modifiedFactorsRow = haveFactorDependence ? TryGetScoreOfFactors(modifiedQuestionRow, _factorValueFields[1])
                    : modifiedQuestionRow;
                // Используя парсер, вычисляем значение фактора
                var factorScoreInString = GetParseStringFromDataTableAndGetResult(modifiedFactorsRow, needFactorsFields, _factorValueFields[3]);
                InsertFactorsResultIntoDbAndFactorScoreList(factorId, factorScoreInString);
            }
        }
        
        private DataTable GetFactorDataByFactorId(int factorId)
        {
            var query = "select Questions.name "+ _factorValueFields[0] +", " +
                                "FactorsValue.factor_dependence_id "+ _factorValueFields[1] +", " +
                                "FactorsValue.digit "+ _factorValueFields[2] +", " +
                                "Operations.name "+ _factorValueFields[3] +
                        " from main.FactorsValue " +
                        "left join main.Operations on Operations.id = FactorsValue.operation_id " +
                        "left join main.Questions on Questions.id = FactorsValue.question_dependence_id " +
                        "where FactorsValue.factor_id = " + factorId;
            try
            {
                var factorDataRow = DbConnection.SelectFromDb(query);
                return factorDataRow.Rows.Count > 0 ? factorDataRow : null;
            }
 
            catch (Exception exp)
            {
                throw new Exception("GetFactorDataByFactorId: " + exp);
            }
        }

        private DataTable TryGetScoreOfQuestion(DataTable table, string questionRowName)
        {
            if (table.Columns.Contains(questionRowName))
            {
                var modifiedQuestionDataTable = table;
                for (int i = 0; i < table.Rows.Count; i ++)
                {
                    var questionName = table.Rows[i][questionRowName];
                    if (questionName.ToString() != "")
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
                    if (factorId.ToString() != "")
                    {
                        var factorName = GetFactorNameById(Convert.ToInt32(factorId));
                        modifiedFactorDataTable.Rows[i][factorRowName] =
                            FactorAnalize.GetFactorScoreByFactorName(factorName);
                        
                    }
                }
                return modifiedFactorDataTable;
            }
            throw new Exception("TryGetScoreOfFactors: Can't find need row!");
        }

        private string GetParseStringFromDataTableAndGetResult(DataTable table, string[] needFactorFields, string operationField)
        {
            var factorStr = string.Empty;
            for (int i = 0; i < table.Rows.Count; i ++)
            {
                for (int j = 0; j < needFactorFields.Count(); j++)
                {
                    if (table.Rows[i][needFactorFields[j]].ToString() != "")
                    {
                        factorStr += table.Rows[i][needFactorFields[j]] + " ";
                        break;
                    }
                }
            }
            for (int i = 0; i < table.Rows.Count; i ++)
            {
                if (table.Rows[i][operationField].ToString() != "")
                {
                    factorStr += table.Rows[i][operationField] + " ";
                }
            }
            var result = StringParser.GetResultFromInfixString(factorStr);
            return result;
        }

        private void InsertFactorsResultIntoDbAndFactorScoreList(int factorId, string factoreScoreInString)
        {
            int factorScore;
            var parseToInt = Int32.TryParse(factoreScoreInString, out factorScore);
            if (parseToInt)
            {
                try
                {
                    var factorName = GetFactorNameById(factorId);
                    FactorAnalize.SetFactorAndFactorScore(new Factor {Name = factorName}, factorScore);
                    if (Options.HaveFactorsHistory)
                    {
                        InsertFactorInDb(factorId, factorScore);
                    }
                    else
                    {
                        if (_maxInterviewNum > 0)
                        {
                            var query = "select FactorResults.id " +
                                        "from main.FactorResults " +
                                        "where FactorResults.interview_number = " + _maxInterviewNum +
                                        " and FactorResults.respondent_id = " + _respondentId +
                                        " and FactorResults.theme_id = " + _themeId +
                                        " and FactorResults.factor_id = " + factorId;
                            var factorResultId = DbConnection.SelectScalarFromDb(query);
                            if (factorResultId.ToString() == "")
                            {
                                throw new Exception("Factor was not found!");
                            }
                            query = "update main.FactorResults " +
                                    "set FactorResults.answer_date = current_date " +
                                    ", FactorResults.score = " + factorScore +
                                    ", FactorResults.interview_number = " + (_maxInterviewNum + 1) +
                                    " where FactorResults.id = " + Convert.ToInt32(factorResultId);
                            DbConnection.DmlOperation(query);
                        }
                        else
                        {
                            InsertFactorInDb(factorId, factorScore);
                            return;
                        }
                        
                    }
                    
                }
                catch (Exception exp)
                {
                    throw new Exception("InsertFactorsResultIntoDbAndFactorScoreList: " + exp);
                }
            }
        }

        private void InsertFactorInDb(int factorId, int factorScore)
        {
            var newFactorResultsId = GetNewTableIndexId("FactorResults");
            try
            {
                var query =
                    "insert into main.FactorResults(id, respondent_id, factor_id, score, interview_number, answer_date) " +
                    "values(" + newFactorResultsId + ", " + _respondentId + ", " + factorId + ", " +
                    factorScore + ", " + (_maxInterviewNum + 1) + ", " + "current_date)";
                DbConnection.DmlOperation(query);
            }
            catch (Exception exp)
            {
                throw new Exception("InsertFactorInDb: " + exp);
            }
        }


        private void RespondentAndThemeIdInit()
        {
            _respondentId = GetCurrRespondentId();
            _themeId = GetCurrThemeId();
        }
        
        private string GetFactorNameById(int factorId)
        {
            var query = "select Factors.name" +
                        " from main.Factors " +
                        " where Factors.id = " + factorId;
            try
            {
                var factorName = DbConnection.SelectScalarFromDb(query);
                return factorName.ToString();
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
                        " where Respondents.FIO = '" + InterView.GetRespondentName() + "'" +
                               "and Respondents.birthday = '" + InterView.GetBirthDate() + "'";
            try
            {
                var respondentId = DbConnection.SelectScalarFromDb(query);
                return Convert.ToInt32(respondentId);
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
                        " where Themes.name = '" + InterView.GetInterviewTheme() + "'";
            try
            {
                var themeId = DbConnection.SelectScalarFromDb(query);
                return Convert.ToInt32(themeId);
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
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int j = 0; j < table.Columns.Count; j++)
                        resultCollection.Add(Convert.ToInt32(table.Rows[i][j]));
                }
            }
            return resultCollection;
        }

        private int GetNewTableIndexId(string tableName)
        {
            var query = "select max(id) from " + tableName;
            try
            {
                var result = DbConnection.SelectScalarFromDb(query);
                if (result.ToString() != "")
                    return Convert.ToInt32(result) + 1;
                return 1;
            }
            catch (Exception exp)
            {
                throw new Exception("GetNewTableIndexId " + exp);
            }

        }

        private int GetMaxInterviewNum()
        {
            var query = "select max(FactorResults.interview_number)" +
                        "from main.FactorResults " +
                        "where FactorResults.respondent_id = " + _respondentId +
                        " and FactorResults.theme_id = " + _themeId;
            try
            {
                var maxInterviewNum = DbConnection.SelectScalarFromDb(query);
                if (maxInterviewNum.ToString() != "")
                {
                    return Convert.ToInt32(maxInterviewNum);
                }

            }
            catch (Exception exp)
            {
                throw new Exception("GetMaxInterviewNum " + exp);
            }
            return 0;
        }
        

    }
}
