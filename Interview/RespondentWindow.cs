using System;
using System.Windows.Forms;
using Interview.InterviewWorker;

namespace Interview
{
    public partial class RespondentWindow : Form
    {
        private readonly QuestionMaker _questionMaker;
        private bool _rightFio;
        private bool _rightBirthDate;
        public RespondentWindow()
        {
            InitializeComponent();
            _questionMaker = new QuestionMaker();
            InterviewThemesInit();
            Options.OptionsLoaderInit();
            //CalendarTest();
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
            if (flag1 && flag2 && _rightBirthDate)
            {
                _rightFio = true;
                _questionMaker.SetBirthDate(Birthday_Picker.Value);
                var window = new StartInterviewWindow(_questionMaker);
                window.ShowDialog();
                var window2 = new InterviewWindow(_questionMaker);
                window2.ShowDialog();
            }
        }

        private void RespondentWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_rightFio)
            {
                if (MessageBox.Show("Вы уверены, что хотите прервать тестирование?", "INFO",
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    Application.ExitThread();
                    Application.Exit();
                }
            }
        }

        private void Birthday_Picker_ValueChanged(object sender, EventArgs e)
        {
            _rightBirthDate = true;
        }
    }
}
