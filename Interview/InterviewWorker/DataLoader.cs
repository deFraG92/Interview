using System;
using System.Data;


namespace Interview.InterviewWorker
{
    public enum GetDataType 
    {
        Theme,
        Questions,
        Answers,
        QuestionsAnswersScores,
        InterviewId,
        RespondentId,
        FirstQuestionId,
        InterviewCompleteness,
        AnswerScores,
        QuestionPicture
    }

    public enum SetDataType
    {
        BaseOptionsInit,
        AnswerResultInsert,
        AnswerResultUpdate,
        Respondent,
        Factor,
        FactorResult
    }

    public abstract class DataLoader
    {
        protected DBWorker.IDbConnection DbConnection;
        public abstract DataTable GetDataTable(Enum getDataEnum);
        public abstract void SetDataTable(Enum setDataEnum);
    }
}