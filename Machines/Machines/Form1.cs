using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Machines
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ResultButton.Enabled = false;
            string path = Path.Combine(Directory.GetCurrentDirectory() + "\\code.txt");
            ReadFromFile(path);
        }

       
        /// <summary>
        /// Считывает данные из файла и записывает в richtextbox
        /// </summary>
        /// <param name="path">Путь до файла</param>
        private void ReadFromFile(string path)
        {
            StreamReader reader = new StreamReader(path);
            CodeTextBox.Text = reader.ReadToEnd();
            reader.Close();
        }
        private void SelectFileButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    ReadFromFile(openFileDialog.FileName);
            }
        }

        /// <summary>
        /// Записывает данные из Richtextbox в строку, ставя пробелы между строками Richtextbox
        /// </summary>
        /// <param name="str">Строка для заполнения</param>
        /// <returns>Заполненная строка</returns>
        private string GetDataFromRichTextBox(string str)
        {

            for (int i = 0; i < CodeTextBox.Lines.Length; i++)
            {

                for (int j = 0; j < CodeTextBox.Lines[i].Length; j++)
                {
                    // Если в строке один знак - символ
                    if (CodeTextBox.Lines[i].Length == 1 && !char.IsLetterOrDigit(CodeTextBox.Lines[i][j]))
                    {
                        // Добавляем символ в строку и делаем перенос строки
                        str = str + CodeTextBox.Lines[i][j] + ' ' + "/-";
                        continue;

                    }


                    if (CodeTextBox.Lines[i].Length - j != 1) // Если элемент непоследний в строке
                    {
                        // Если текущий элемент не символ,а следующий - символ или текущий элемент - символ, а следующий - нет (Делаем пробелы между разделителем и неразделителем)

                        if (((char.IsLetterOrDigit(CodeTextBox.Lines[i][j]) || CodeTextBox.Lines[i][j] == ' ') && ((!char.IsLetterOrDigit(CodeTextBox.Lines[i][j + 1])) && CodeTextBox.Lines[i][j + 1] != ' ')) || ((!char.IsLetterOrDigit(CodeTextBox.Lines[i][j]) && CodeTextBox.Lines[i][j] != ' ') && (char.IsLetterOrDigit(CodeTextBox.Lines[i][j + 1]) || CodeTextBox.Lines[i][j + 1] == ' ')))
                        {
                            str = str + CodeTextBox.Lines[i][j] + ' ';
                        }
                        else
                            str += CodeTextBox.Lines[i][j];
                    }
                    else if (CodeTextBox.Lines[i].Length - j == 1) // Если элемент последний в строке
                    {
                        // Если последний элемент строки - символ и предпоследний элемент строки - символ
                        if ((!char.IsLetterOrDigit(CodeTextBox.Lines[i][j]) && CodeTextBox.Lines[i][j] != ' ') && (!char.IsLetterOrDigit(CodeTextBox.Lines[i][j - 1]) && CodeTextBox.Lines[i][j - 1] != ' '))
                            str += CodeTextBox.Lines[i][j];

                        // Если последний элемент строки - символ и предпоследний элемент строки - не символ
                        else if ((!char.IsLetterOrDigit(CodeTextBox.Lines[i][j]) && CodeTextBox.Lines[i][j] != ' ') && (char.IsLetterOrDigit(CodeTextBox.Lines[i][j - 1]) || CodeTextBox.Lines[i][j - 1] == ' '))

                            str = str + ' ' + CodeTextBox.Lines[i][j];

                        else
                            str = str + CodeTextBox.Lines[i][j];

                        
                        str = str + ' ' + "/-"; // знак /- перенос на новую строку 
                    }
                }

                str += ' '; // Разделяем строки richtextbox пробелом

            }
            // Удаляем последний /- (его не должно быть)
            str = str.Substring(0, str.Length - 2);

            return str;
        }

        
        private void AnalysisButton_Click(object sender, EventArgs e)
        {
            if (CodeTextBox.Text == "")
                MessageBox.Show("Пустой текстовый блок!", "Лексический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                LexicalAnalysis lexicalAnalysis = new LexicalAnalysis();
                string text = "";
                lexicalAnalysis.Analysis(GetDataFromRichTextBox(text), ResultButton); // Производим лексический анализ

                Classification.tokens = Token.GeneratingListTokens(Classification.lexemes, Classification.KeyWords, Classification.separators, Classification.variables, Classification.literals); // Получаем список токенов
            }
        }
       
        private void ResultButton_Click(object sender, EventArgs e)
        {
            ResultButton.Enabled = false;

            Form2 form2 = new Form2();
            form2.Show(); 

        }
    }
}

