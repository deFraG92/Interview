using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Interview.InterviewWorker
{
    public static class FactorAnalize
    {
        private static DataLoader _dataLoader;
        private static Dictionary<Factor, int> _factorScoreList; 
        
        public static void FactorAnalizeInit(string tns)
        {
            _dataLoader = new FactorDataLoader(tns);
        }

        public static GetFactorScoreByFactorName(string factorName)
        {
            var factor = new Factor() {Name = factorName};
        }

        private static void SetFactorResultList()
        {
            _dataLoader.GetDataTable(GetFactorData.Factors);
        }


    }
}
