
namespace Machines
{
    partial class TablesForm
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
            this.LexemesTreeView = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.KeyWordsTreeView = new System.Windows.Forms.TreeView();
            this.SeparatorsTreeView = new System.Windows.Forms.TreeView();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.VariablesTreeView = new System.Windows.Forms.TreeView();
            this.LiteralsTreeView = new System.Windows.Forms.TreeView();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TokensTreeView = new System.Windows.Forms.TreeView();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LexemesTreeView
            // 
            this.LexemesTreeView.Location = new System.Drawing.Point(26, 50);
            this.LexemesTreeView.Name = "LexemesTreeView";
            this.LexemesTreeView.Size = new System.Drawing.Size(422, 368);
            this.LexemesTreeView.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(170, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Список лексем";
            // 
            // KeyWordsTreeView
            // 
            this.KeyWordsTreeView.Location = new System.Drawing.Point(465, 50);
            this.KeyWordsTreeView.Name = "KeyWordsTreeView";
            this.KeyWordsTreeView.Size = new System.Drawing.Size(235, 168);
            this.KeyWordsTreeView.TabIndex = 2;
            // 
            // SeparatorsTreeView
            // 
            this.SeparatorsTreeView.Location = new System.Drawing.Point(465, 250);
            this.SeparatorsTreeView.Name = "SeparatorsTreeView";
            this.SeparatorsTreeView.Size = new System.Drawing.Size(235, 168);
            this.SeparatorsTreeView.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(492, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "1. Список ключевых слов";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(492, 227);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(174, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "2. Список разделителей";
            // 
            // VariablesTreeView
            // 
            this.VariablesTreeView.Location = new System.Drawing.Point(719, 50);
            this.VariablesTreeView.Name = "VariablesTreeView";
            this.VariablesTreeView.Size = new System.Drawing.Size(301, 168);
            this.VariablesTreeView.TabIndex = 6;
            // 
            // LiteralsTreeView
            // 
            this.LiteralsTreeView.Location = new System.Drawing.Point(719, 250);
            this.LiteralsTreeView.Name = "LiteralsTreeView";
            this.LiteralsTreeView.Size = new System.Drawing.Size(301, 168);
            this.LiteralsTreeView.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(774, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(167, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "3. Список переменных";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(790, 227);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(151, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "4. Список литералов";
            // 
            // TokensTreeView
            // 
            this.TokensTreeView.Location = new System.Drawing.Point(1052, 50);
            this.TokensTreeView.Name = "TokensTreeView";
            this.TokensTreeView.Size = new System.Drawing.Size(235, 368);
            this.TokensTreeView.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1054, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(233, 20);
            this.label6.TabIndex = 11;
            this.label6.Text = "Таблица стандартных символов";
            // 
            // TablesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 450);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TokensTreeView);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.LiteralsTreeView);
            this.Controls.Add(this.VariablesTreeView);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SeparatorsTreeView);
            this.Controls.Add(this.KeyWordsTreeView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LexemesTreeView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "TablesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Результат лексического анализа";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TablesForm_FormClosing);
            this.Load += new System.EventHandler(this.TablesForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView LexemesTreeView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView KeyWordsTreeView;
        private System.Windows.Forms.TreeView SeparatorsTreeView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TreeView VariablesTreeView;
        private System.Windows.Forms.TreeView LiteralsTreeView;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TreeView TokensTreeView;
        private System.Windows.Forms.Label label6;
    }
}