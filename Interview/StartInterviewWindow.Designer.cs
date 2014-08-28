namespace Interview
{
    partial class StartInterviewWindow
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
            this.ContinueInterview_But = new System.Windows.Forms.Button();
            this.StartNewInterview_But = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ContinueInterview_But
            // 
            this.ContinueInterview_But.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ContinueInterview_But.Location = new System.Drawing.Point(22, 37);
            this.ContinueInterview_But.Name = "ContinueInterview_But";
            this.ContinueInterview_But.Size = new System.Drawing.Size(159, 43);
            this.ContinueInterview_But.TabIndex = 0;
            this.ContinueInterview_But.Text = "Продолжить интервью";
            this.ContinueInterview_But.UseVisualStyleBackColor = true;
            this.ContinueInterview_But.Click += new System.EventHandler(this.ContinueInterview_But_Click);
            // 
            // StartNewInterview_But
            // 
            this.StartNewInterview_But.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StartNewInterview_But.Location = new System.Drawing.Point(234, 37);
            this.StartNewInterview_But.Name = "StartNewInterview_But";
            this.StartNewInterview_But.Size = new System.Drawing.Size(159, 43);
            this.StartNewInterview_But.TabIndex = 1;
            this.StartNewInterview_But.Text = "Начать новое интервью";
            this.StartNewInterview_But.UseVisualStyleBackColor = true;
            this.StartNewInterview_But.Click += new System.EventHandler(this.StartNewInterview_But_Click);
            // 
            // StartInterviewWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(404, 112);
            this.Controls.Add(this.StartNewInterview_But);
            this.Controls.Add(this.ContinueInterview_But);
            this.MaximumSize = new System.Drawing.Size(420, 150);
            this.MinimumSize = new System.Drawing.Size(420, 150);
            this.Name = "StartInterviewWindow";
            this.Text = "StartInterviewWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ContinueInterview_But;
        private System.Windows.Forms.Button StartNewInterview_But;
    }
}