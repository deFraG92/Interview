using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Interview.InterviewWorker;


namespace Interview
{
    public partial class InterviewWindow : Form
    {
        private readonly QuestionMaker _questionMaker;
        public InterviewWindow(QuestionMaker questionMaker)
        {
            InitializeComponent();
            _questionMaker = questionMaker;
            _questionMaker.QuestionsAndAnswersInit(this);
            //Admin Tools
            Question_Lbl.Location = Options.QuestionLocation;
            Prev_But.Visible = Options.HaveBackward;
            //
            GetQuestionAndAnswers(QuestionMove.Forward);
           //ParseInit();
        }
      
        private void Next_But_Click(object sender, EventArgs e)
        {
            var isTrue = false;
            _questionMaker.SetAnswer(out isTrue);
            if (isTrue)
            {
                GetQuestionAndAnswers(QuestionMove.Forward);
            }
        }

        private void Prev_BUT_Click(object sender, EventArgs e)
        {
            GetQuestionAndAnswers(QuestionMove.BackWard);
        }

        private void GetQuestionAndAnswers(QuestionMove questionMove)
        {
            var question = _questionMaker.GetQuestion(questionMove);
            if (question == null)
                Close();
            else
            {
                Question_Lbl.Text = question.ToString();
                var questionLocation = new Point(Question_Lbl.Location.X, Question_Lbl.Location.Y + Question_Lbl.Height);
                _questionMaker.ChangeQuestionCoords(questionLocation);
                _questionMaker.MakeAnswers(question, questionMove);
            }
        }

        // Replace to Another Class 
        #region
        private void ParseInit()
        {
            _signList = new List<List<char>>()
            {
                new List<char>(){'(', ')'},
                new List<char>(){'^'},
                new List<char>(){'*', '/'},
                new List<char>(){'+', '-'}
            };
           var result = GetFinalResultOfPolishRecord("('10'+'A')*'3'");
       }

        private string GetFinalResultOfPolishRecord(string str)
        {
            var result = GetInfixString(str);
            if (result != string.Empty)
            {
                double doubleRes;
                while (!Double.TryParse(result, out doubleRes))
                {
                    result = GetIterationResultOfPolishRecord(result);
                }
            }
            return result.Trim();
        }


        private bool IsSign(char symbol)
        {
            foreach (var elem in _signList)
            {
                if (elem.Contains(symbol))
                    return true;
            }
            return false;
        }

        private int GetPriority(char sign)
        {
            for (int i = 0; i < _signList.Count; i++)
            {
                if (_signList[i].Contains(sign))
                    return i;
            }
            return -1;
        }

        private string GetIterationResultOfPolishRecord(string infixString)
        {
            var array = (infixString + " ").ToCharArray();
            var signIndex = 0;
            char sign = ' ';
            for (var i = 0; i < array.Length - 1; i++)
            {
                if ( (array[i] != ' ') & (array[i + 1] == ' ') & (signIndex == 0) )
                {
                    foreach (var signList in _signList)
                    {
                        if (signList.Contains(array[i]))
                        {
                            signIndex = i;
                            sign = array[i];
                            break;
                        }
                    }
                }
            }
            var counter = 0;
            var str2Counter = 0;
            var str1 = string.Empty;
            var str2 = string.Empty;
            for (var j = signIndex - 1; j >= 0; j--)
            {
                if (array[j] == ' ')
                {
                    counter++;
                    if (counter > 2)
                    {
                        break;
                    }
                }
                else
                {
                    if (counter == 1)
                    {
                        str1 += array[j];
                    }
                    if (counter == 2)
                    {
                        str2 += array[j];
                        str2Counter = j;
                    }
                }
            }
            var reverseStr1 = String.Join(string.Empty, str1.Reverse().Select(p => p.ToString()).ToArray());
            var reverseStr2 = String.Join(string.Empty, str2.Reverse().Select(p => p.ToString()).ToArray());
            var count = GetOperationResult(reverseStr1, reverseStr2, sign);
            infixString = infixString.Remove(str2Counter, signIndex - str2Counter + 1);
            var resultstr = infixString.Insert(str2Counter, count.ToString());
            return resultstr;
        }

        private double GetOperationResult(string a, string b, char sign)
        {
            double result;
            if ((Double.TryParse(a, out result)) & (Double.TryParse(b, out result)))
            {
                var doubleA = Convert.ToDouble(a);
                var doubleB = Convert.ToDouble(b);
                switch (sign)
                {
                   case '+':
                        return doubleA + doubleB;
                        
                    case '-':
                        return doubleB - doubleA;

                    case '*':
                        return doubleA * doubleB;

                    case '/':
                        return doubleB != 0 ? doubleB / doubleA : 0 ;

                    case '^':
                        return Math.Pow(doubleB, doubleA);
                }
            }
            throw new Exception("GetOperationResult ERROR!");
        }

        private List<List<char>> _signList; 
        private string GetInfixString(string str)
        {
            var stack = new Stack<char>();
            var resultStr = string.Empty;
            var array = str.ToCharArray();
            var strSymbols = string.Empty;
            var strDigit = string.Empty;
            var isDigit = false;
            foreach (var elem in array)
            {
                if (!IsSign(elem))
                {
                    if ((elem.ToString() == "'") | (strSymbols.LastIndexOf("'")== 0) ) 
                    {
                        strSymbols += elem;
                        if (strSymbols.LastIndexOf("'") > 0)
                        {
                            resultStr += strSymbols + " ";
                            strSymbols = string.Empty;
                        }
                    }
                    else
                    {
                        strDigit += elem;
                        isDigit = true;
                    }
                }
                else
                {
                    if (isDigit)
                    {
                        resultStr += strDigit + " ";
                        strDigit = string.Empty;
                        isDigit = false;
                    }
                    if (stack.Count > 0)
                    {
                        if (elem.ToString() == ")")
                        {
                            var c = stack.Pop();
                            while (c != '(')
                            {
                                resultStr += c + " ";
                                c = stack.Pop();
                            }
                            continue;
                        }

                        if (GetPriority(elem) < GetPriority(stack.Peek()) | (stack.Contains('(')))
                        {
                            stack.Push(elem);
                        }
                        else
                        {
                            resultStr += stack.Pop() + " ";
                            stack.Push(elem);
                        }
                    }
                    else
                    {
                        stack.Push(elem);
                    }
                }
            }
            if (strDigit != string.Empty)
                resultStr += strDigit + " ";
            var stackStr = string.Empty;
            while (stack.Count > 0)
            {
                stackStr += stack.Pop() + " ";
            }
            resultStr += stackStr;
            return resultStr;
        }
        #endregion


    }
}
