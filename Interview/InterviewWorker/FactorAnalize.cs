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
            _factorScoreList = new Dictionary<Factor, int>();
            SetFactorResultList();
        }

        public static int GetFactorScoreByFactorName(string factorName)
        {
            var factor = new Factor {Name = factorName};
            if (_factorScoreList.ContainsKey(factor))
            {
                return _factorScoreList[factor];
            }
            return -1;
        }

        public static void SetFactorAndFactorScore(Factor factor, int score)
        {
            _factorScoreList.Add(factor, score);
        }

        private static void SetFactorResultList()
        {
            _dataLoader.SetDataTable(SetFactorData.Factors);
        }


    }
}
