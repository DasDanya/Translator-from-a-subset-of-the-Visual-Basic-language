using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Machines
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e) 
        {
            ResultButton.Enabled = false;
            string path = Path.Combine(Directory.GetCurrentDirectory() + "\\code.txt");
            ReadFromFile(path);

        }
        //private void Form1_Load(object sender, EventArgs e)
        //{
        //    ResultButton.Enabled = false;
        //    string path = Path.Combine(Directory.GetCurrentDirectory() + "\\code.txt");
        //    ReadFromFile(path);
        //}

       
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
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Txt files(.txt)|*.txt";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                        ReadFromFile(openFileDialog.FileName);
                }
            }
            catch 
            {
                MessageBox.Show("Ошибка чтения файла", "Чтение файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


                        str = str +  ' ' + "/-"; // знак /- перенос на новую строку 
                        
                    }
                    
                }
                //str = str + ' ' + "/-"; // знак /- перенос на новую строку
                str += ' '; // Разделяем строки richtextbox пробелом

            }
            // Удаляем последний /- (его не должно быть)
            str = str.Substring(0, str.Length - 2);

            string result = "";
            
            // цикл для того,чтобы между одинаковыми скобками был пробел
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] == '(' && str[i + 1] == '(') || (str[i] == ')' && str[i + 1] == ')'))
                {
                    result = result + str[i] +  " ";
                }
                else
                    result = result + str[i];
                
            }

            //check(str);

            return result;
        }

       
        
        private void AnalysisButton_Click(object sender, EventArgs e)
        {
            Classification.Clear();
            logOperationRichTextBox.Clear();

            if (CodeTextBox.Text.Trim() == "") // Здесь был CodeTextBox.Text.Trim()     
            MessageBox.Show("Пустой текстовый блок!", "Лексический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {

                LexicalAnalysis lexicalAnalysis = new LexicalAnalysis();
                string text = "";

                // Если лексический анализ успешно произошёл
                if (lexicalAnalysis.Analysis(GetDataFromRichTextBox(text), ResultButton)) // Производим лексический анализ
                {
                    Classification.tokens = Token.GeneratingListTokens(Classification.lexemes, Classification.KeyWords, Classification.separators, Classification.variables, Classification.literals); // Получаем список токенов

                    // Начинаем лексический анализ
                    SyntacticAnalysis syntacticAnalysis = new SyntacticAnalysis(Classification.tokens, logOperationRichTextBox);
                    syntacticAnalysis.StartAnalysis();

                    //CodeTextBox.Text.Trim();

                    //MessageBox.Show(text); // убрать (для проверки)
                }
                
            }
        }
       
        private void ResultButton_Click(object sender, EventArgs e)
        {
            ResultButton.Enabled = false;

            TablesForm form2 = new TablesForm();
            form2.Show(); 

        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            CodeTextBox.Clear();
            logOperationRichTextBox.Clear();
        }
    }
}

