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
        HaveHistory,
        HavePicture,
        PictureLocation,
        PictureSize,
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
                data = _dataLoader.GetDataTable(OptionName.HavePicture).Rows;
                HavePictures = Convert.ToBoolean(data[0][0]);
                data = _dataLoader.GetDataTable(OptionName.PictureLocation).Rows;
                PictureLocation = new Point(Convert.ToInt32(data[0][0]), Convert.ToInt32(data[1][0]));
                data = _dataLoader.GetDataTable(OptionName.PictureSize).Rows;
                PictureSize = new Size(Convert.ToInt32(data[0][0]), Convert.ToInt32(data[1][0]));
            }
            catch (Exception exp)
            {
                throw new Exception("Options.InitOptions " + exp);
            }
        }

        public static Point QuestionLocation { get; private set; }
        public static Point SpaceBetweenAnswers { get; private set; }
        public static Point SpaceBetweenQuestionAndAnswers { get; private set; }
        public static Point PictureLocation { get; private set; }
        public static Size PictureSize { get; private set; }
        public static bool HaveBackward { get; private set; }
        public static bool HaveHistory { get; private set; }
        public static bool HavePictures { get; private set; }


    }
}
