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
        RespondentId
    }

    public enum SetDataType
    {
        AnswerResult,
        Respondent,
        Factor,
        FactorResult
    }

    public abstract class QuestionLoader
    {
        public abstract DataTable GetDataTable(GetDataType getDataType);
        public abstract void SetDataTable(SetDataType setDataType);
    }
}