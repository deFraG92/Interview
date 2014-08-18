using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interview.InterviewWorker;

namespace Interview
{
    public partial class RespondentWindow : Form
    {
        private readonly QuestionMaker _questionMaker;
        private bool _isOk;
        public RespondentWindow(QuestionMaker questionMaker)
        {
            InitializeComponent();
           // _questionMaker = questionMaker;
            //InterviewThemesInit();
            OrderedCollectionInit();
        }

        private void OrderedCollectionInit()
        {
            var list = new OrderedDictionary {{new Question() {Name = "Сколько будет 2 + 2?"}, new Answer()}};
            var testQuestion = new Question() {Name = "Сколько будет 7 + 7?"};
            if (!list.Contains(testQuestion))
            {
                list.Add(testQuestion, new Answer());
            }

            var questionList = new Question[list.Keys.Count];
            list.Keys.CopyTo(questionList, 0);
            
            for (int i = 0; i < questionList.Length; i++)
            {
                var s = questionList[i];
            }
        }

        private void InterviewThemesInit()
        {
            var dataTable = _questionMaker.GetThemeNames();
            ChooseTheme_CMB.Text = dataTable.Rows[0][0].ToString();
            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                ChooseTheme_CMB.Items.Add(dataTable.Rows[i][0]);
            }
        }

        private void InterviewStart_BUT_Click(object sender, EventArgs e)
        {
            var flag1 = _questionMaker.SetInterviewThemeName(ChooseTheme_CMB.Text);
            var flag2 = _questionMaker.SetRespondentName(FIO_TEXTBOX.Text);
            if (flag1 && flag2)
            {
                _isOk = true;
                Close();
            }
        }

        private void RespondentWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isOk)
            {
                if (MessageBox.Show("Вы уверены, что хотите прервать тестирование?", "INFO",
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    Application.ExitThread();
                    Application.Exit();
                }
            
        }
            //
        }
    }
}
