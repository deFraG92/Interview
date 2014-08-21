namespace Interview
{
    partial class RespondentWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Continue_But = new System.Windows.Forms.Button();
            this.FIO_TEXTBOX = new System.Windows.Forms.TextBox();
            this.ChooseTheme_CMB = new System.Windows.Forms.ComboBox();
            this.FIO_LBL = new System.Windows.Forms.Label();
            this.Theme_LBL = new System.Windows.Forms.Label();
            this.Birthday_Picker = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Continue_But
            // 
            this.Continue_But.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Continue_But.Location = new System.Drawing.Point(338, 114);
            this.Continue_But.Name = "Continue_But";
            this.Continue_But.Size = new System.Drawing.Size(137, 44);
            this.Continue_But.TabIndex = 0;
            this.Continue_But.Text = "Продолжить";
            this.Continue_But.UseVisualStyleBackColor = true;
            this.Continue_But.Click += new System.EventHandler(this.InterviewStart_BUT_Click);
            // 
            // FIO_TEXTBOX
            // 
            this.FIO_TEXTBOX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FIO_TEXTBOX.Location = new System.Drawing.Point(88, 12);
            this.FIO_TEXTBOX.Multiline = true;
            this.FIO_TEXTBOX.Name = "FIO_TEXTBOX";
            this.FIO_TEXTBOX.Size = new System.Drawing.Size(387, 19);
            this.FIO_TEXTBOX.TabIndex = 1;
            this.FIO_TEXTBOX.Text = "Селимов И.А.";
            this.FIO_TEXTBOX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ChooseTheme_CMB
            // 
            this.ChooseTheme_CMB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ChooseTheme_CMB.FormattingEnabled = true;
            this.ChooseTheme_CMB.Location = new System.Drawing.Point(192, 84);
            this.ChooseTheme_CMB.Name = "ChooseTheme_CMB";
            this.ChooseTheme_CMB.Size = new System.Drawing.Size(164, 24);
            this.ChooseTheme_CMB.TabIndex = 2;
            // 
            // FIO_LBL
            // 
            this.FIO_LBL.AutoSize = true;
            this.FIO_LBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FIO_LBL.Location = new System.Drawing.Point(28, 12);
            this.FIO_LBL.Name = "FIO_LBL";
            this.FIO_LBL.Size = new System.Drawing.Size(54, 16);
            this.FIO_LBL.TabIndex = 3;
            this.FIO_LBL.Text = "Ф.И.О.";
            // 
            // Theme_LBL
            // 
            this.Theme_LBL.AutoSize = true;
            this.Theme_LBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Theme_LBL.Location = new System.Drawing.Point(28, 87);
            this.Theme_LBL.Name = "Theme_LBL";
            this.Theme_LBL.Size = new System.Drawing.Size(158, 16);
            this.Theme_LBL.TabIndex = 4;
            this.Theme_LBL.Text = "Тема тестирования:";
            // 
            // Birthday_Picker
            // 
            this.Birthday_Picker.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Birthday_Picker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.Birthday_Picker.Location = new System.Drawing.Point(359, 40);
            this.Birthday_Picker.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
            this.Birthday_Picker.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.Birthday_Picker.Name = "Birthday_Picker";
            this.Birthday_Picker.Size = new System.Drawing.Size(116, 22);
            this.Birthday_Picker.TabIndex = 6;
            this.Birthday_Picker.Value = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
            this.Birthday_Picker.ValueChanged += new System.EventHandler(this.Birthday_Picker_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(229, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "Дата рождения:";
            // 
            // RespondentWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(484, 162);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Birthday_Picker);
            this.Controls.Add(this.Theme_LBL);
            this.Controls.Add(this.FIO_LBL);
            this.Controls.Add(this.ChooseTheme_CMB);
            this.Controls.Add(this.FIO_TEXTBOX);
            this.Controls.Add(this.Continue_But);
            this.MaximumSize = new System.Drawing.Size(500, 200);
            this.MinimumSize = new System.Drawing.Size(500, 200);
            this.Name = "RespondentWindow";
            this.Text = "RespondentWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RespondentWindow_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Continue_But;
        private System.Windows.Forms.TextBox FIO_TEXTBOX;
        private System.Windows.Forms.ComboBox ChooseTheme_CMB;
        private System.Windows.Forms.Label FIO_LBL;
        private System.Windows.Forms.Label Theme_LBL;
        private System.Windows.Forms.DateTimePicker Birthday_Picker;
        private System.Windows.Forms.Label label1;
    }
}