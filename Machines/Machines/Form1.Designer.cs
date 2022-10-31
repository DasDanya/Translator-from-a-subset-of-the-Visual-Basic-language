
namespace Machines
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SelectFileButton = new System.Windows.Forms.Button();
            this.AnalysisButton = new System.Windows.Forms.Button();
            this.ResultButton = new System.Windows.Forms.Button();
            this.CodeTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // SelectFileButton
            // 
            this.SelectFileButton.Location = new System.Drawing.Point(451, 42);
            this.SelectFileButton.Name = "SelectFileButton";
            this.SelectFileButton.Size = new System.Drawing.Size(144, 66);
            this.SelectFileButton.TabIndex = 1;
            this.SelectFileButton.Text = "Выбрать файл";
            this.SelectFileButton.UseVisualStyleBackColor = true;
            this.SelectFileButton.Click += new System.EventHandler(this.SelectFileButton_Click);
            // 
            // AnalysisButton
            // 
            this.AnalysisButton.Location = new System.Drawing.Point(451, 182);
            this.AnalysisButton.Name = "AnalysisButton";
            this.AnalysisButton.Size = new System.Drawing.Size(144, 66);
            this.AnalysisButton.TabIndex = 2;
            this.AnalysisButton.Text = "Лексический анализ";
            this.AnalysisButton.UseVisualStyleBackColor = true;
            this.AnalysisButton.Click += new System.EventHandler(this.AnalysisButton_Click);
            // 
            // ResultButton
            // 
            this.ResultButton.Location = new System.Drawing.Point(451, 355);
            this.ResultButton.Name = "ResultButton";
            this.ResultButton.Size = new System.Drawing.Size(144, 66);
            this.ResultButton.TabIndex = 3;
            this.ResultButton.Text = "Результат анализа";
            this.ResultButton.UseVisualStyleBackColor = true;
            this.ResultButton.Click += new System.EventHandler(this.ResultButton_Click);
            // 
            // CodeTextBox
            // 
            this.CodeTextBox.Location = new System.Drawing.Point(23, 42);
            this.CodeTextBox.Multiline = true;
            this.CodeTextBox.Name = "CodeTextBox";
            this.CodeTextBox.Size = new System.Drawing.Size(392, 379);
            this.CodeTextBox.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 450);
            this.Controls.Add(this.CodeTextBox);
            this.Controls.Add(this.ResultButton);
            this.Controls.Add(this.AnalysisButton);
            this.Controls.Add(this.SelectFileButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Главное меню";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button SelectFileButton;
        private System.Windows.Forms.Button AnalysisButton;
        private System.Windows.Forms.Button ResultButton;
        private System.Windows.Forms.TextBox CodeTextBox;
    }
}

