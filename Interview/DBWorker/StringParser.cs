using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interview.DBWorker
{
    public static class StringParser
    {
        private static readonly List<List<char>> SignList = new List<List<char>>()
                                                            {
                                                                new List<char>(){'(', ')'},
                                                                new List<char>(){'^'},
                                                                new List<char>(){'*', '/'},
                                                                new List<char>(){'+', '-'}
                                                            };

        public static string ParseStringAndGetResult(string str)
        {
            var infixString = ParseStringInInfixForm(str);
            var result = GetResultFromInfixString(infixString);
            return result;
        }

        public static string ParseStringInInfixForm(string str)
        {
            return GetInfixString(str);
        }

        public static string GetResultFromInfixString(string infixString)
        {
            return GetFinalResultOfPolishRecord(infixString);
        }

        private static string GetFinalResultOfPolishRecord(string infixString)
        {
            if (infixString != string.Empty)
            {
                try
                {
                    double doubleRes;
                    while (!Double.TryParse(infixString, out doubleRes))
                    {
                        infixString = GetIterationResultOfPolishRecord(infixString);
                    }
                }
                catch (Exception exp)
                {
                    throw new Exception("GetFinalResultOfPolishRecord: " + exp);
                }
            }
            return infixString.Trim();
        }

        private static string GetIterationResultOfPolishRecord(string infixString)
        {
            var array = (infixString + " ").ToCharArray();
            var signIndex = 0;
            char sign = ' ';
            for (var i = 0; i < array.Length - 1; i++)
            {
                if ((array[i] != ' ') & (array[i + 1] == ' ') & (signIndex == 0))
                {
                    foreach (var signList in SignList)
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
        

        private static bool IsSign(char symbol)
        {
            foreach (var elem in SignList)
            {
                if (elem.Contains(symbol))
                    return true;
            }
            return false;
        }

        private static int GetPriority(char sign)
        {
            for (int i = 0; i < SignList.Count; i++)
            {
                if (SignList[i].Contains(sign))
                    return i;
            }
            return -1;
        }
        
        private static double GetOperationResult(string a, string b, char sign)
        {
            double result;
            try
            {
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
                            return doubleA*doubleB;
                        case '/':
                            return doubleA != 0 ? doubleB/doubleA : 0;

                        case '^':
                            return Math.Pow(doubleB, doubleA);
                    }
                }
            }
            catch (Exception exp)
            {
                throw new Exception("GetOperationResult " + exp);
            }
            return result;
        }   
        
        private static string GetInfixString(string str)
        {
            var stack = new Stack<char>();
            var resultStr = string.Empty;
            var array = str.ToCharArray();
            var strSymbols = string.Empty;
            var strDigit = string.Empty;
            var isDigit = false;
            try
            {
                foreach (var elem in array)
                {
                    if (!IsSign(elem))
                    {
                        if ((elem.ToString() == "'") | (strSymbols.LastIndexOf("'") == 0))
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
            }
            catch (Exception exp)
            {
                throw new Exception("GetInfixString: " + exp);
            }
            return resultStr;
        }


    }
}
