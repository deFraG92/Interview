﻿namespace Interview
{
    partial class InterviewWindow
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
            this.Next_But = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Prev_BUT = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Next_But
            // 
            this.Next_But.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Next_But.BackColor = System.Drawing.Color.Gainsboro;
            this.Next_But.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Next_But.Location = new System.Drawing.Point(346, 203);
            this.Next_But.Name = "Next_But";
            this.Next_But.Size = new System.Drawing.Size(211, 30);
            this.Next_But.TabIndex = 2;
            this.Next_But.Text = ">> Следующий вопрос";
            this.Next_But.UseVisualStyleBackColor = false;
            this.Next_But.Click += new System.EventHandler(this.Next_But_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(25, 20);
            this.label1.MaximumSize = new System.Drawing.Size(500, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 18);
            this.label1.TabIndex = 3;
            // 
            // Prev_BUT
            // 
            this.Prev_BUT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Prev_BUT.BackColor = System.Drawing.Color.Gainsboro;
            this.Prev_BUT.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Prev_BUT.Location = new System.Drawing.Point(28, 203);
            this.Prev_BUT.Name = "Prev_BUT";
            this.Prev_BUT.Size = new System.Drawing.Size(211, 30);
            this.Prev_BUT.TabIndex = 4;
            this.Prev_BUT.Text = "<< Предыдущий вопрос";
            this.Prev_BUT.UseVisualStyleBackColor = false;
            this.Prev_BUT.Click += new System.EventHandler(this.Prev_BUT_Click);
            // 
            // InterviewWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(584, 262);
            this.Controls.Add(this.Prev_BUT);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Next_But);
            this.MaximumSize = new System.Drawing.Size(600, 800);
            this.MinimumSize = new System.Drawing.Size(600, 300);
            this.Name = "InterviewWindow";
            this.Text = "InterView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Next_But;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Prev_BUT;
    }
}

