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
        Options
    }

    public enum SetDataType
    {
        AnswerResult,
        Respondent,
        Factor,
        FactorResult
    }

    public abstract class DataLoader
    {
        public abstract DataTable GetDataTable(Enum getDataEnum);
        public abstract void SetDataTable(Enum setDataEnum);
    }
}