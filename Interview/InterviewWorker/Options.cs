using System;
using System.Collections.Generic;
using System.Drawing;


namespace Interview.InterviewWorker
{
    public enum OptionName 
    {
        QuestionLocation,
        SpaceBetweenQuestionAndAnswers,
        SpaceBetweenAnswers,
        SpaceBetweenQuestionAndPicture,
        HaveBackWard,
        HavePicture,
        HaveHistory
    }

    public static class Options
    {
        private static DataLoader _dataLoader;
        public static void OptionsLoaderInit()
        {
            _dataLoader = new OptionsDataLoader();
            InitOptions();
        }
        private static void InitOptions()
        {
            try
            {
                var data = _dataLoader.GetDataTable(OptionName.QuestionLocation).Rows;
                QuestionLocation = new Point(Convert.ToInt32(data[0][0]), Convert.ToInt32(data[1][0]));
                data = _dataLoader.GetDataTable(OptionName.SpaceBetweenAnswers).Rows;
                SpaceBetweenAnswers = new Point(Convert.ToInt32(data[0][0]), Convert.ToInt32(data[1][0]));
                data = _dataLoader.GetDataTable(OptionName.SpaceBetweenQuestionAndAnswers).Rows;
                SpaceBetweenQuestionAndAnswers = new Point(Convert.ToInt32(data[0][0]), Convert.ToInt32(data[1][0]));
                data = _dataLoader.GetDataTable(OptionName.HaveBackWard).Rows;
                HaveBackward = Convert.ToBoolean(data[0][0]);
                data = _dataLoader.GetDataTable(OptionName.HaveHistory).Rows;
                HaveHistory = Convert.ToBoolean(data[0][0]);
            }
            catch (Exception exp)
            {
                throw new Exception("Options.InitOptions " + exp);
            }
            
        }
        public static Point QuestionLocation { get; private set; }
        public static Point SpaceBetweenAnswers { get; private set; }
        public static Point SpaceBetweenQuestionAndAnswers { get; private set; }
        public static bool HaveBackward { get; private set; }
        public static bool HaveHistory { get; private set; }
    }
}
