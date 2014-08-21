using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

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
        }

        public static Point QuestionLocation
        {
            get
            {
                
            }
            private set
            {
                
            }
        }

    }
}
