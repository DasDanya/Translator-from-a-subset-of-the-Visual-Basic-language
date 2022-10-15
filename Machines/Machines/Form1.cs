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

        string buffer = ""; // Строка для лексического анализа
        Status status = Status.None; // Статус символа

        List<Lexeme> lexemes = new List<Lexeme>(); // Список лексем
        List<string> variables = new List<string>(); // Список переменных
        List<string> literals = new List<string>(); // Список литералов
        List<Token> tokens = new List<Token>(); // Список токенов

        List <string> separators = new List<string> {"+", "-", "=", "/", "<", ">","*", "(", ")", "<>", "/-" }; // Массив одинарных разделителей
        List<string> keyWords = new List<string> { "Dim", "Integer", "Boolean", "Double", "As", "If", "Then", "Else", "End", "Not", "And", "Or", "Xor", "AndAlso", "OrElse"}; // список ключевый слов
         

        /// <summary>
        /// Статусы для лексического анализа
        /// </summary>
        enum Status
        {
            None,
            R,
            I,
            D
        }

        /// <summary>
        /// Считывает данные из файла и записывает в richtextbox
        /// </summary>
        /// <param name="path">Путь до файла</param>
        private void ReadFromFile(string path)
        {
            StreamReader reader = new StreamReader(path);

            CodeRichTextBox.Text = reader.ReadToEnd();
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
            
            for (int i = 0; i < CodeRichTextBox.Lines.Length; i++)
            {
                for (int j = 0; j < CodeRichTextBox.Lines[i].Length; j++) 
                {
                   
                    if (CodeRichTextBox.Lines[i].Length - j != 1) // Если элемент непоследний в строке
                    {
                        // Если текущий элемент не символ,а следующий - символ или текущий элемент - символ, а следующий - нет (Делаем пробелы между разделителем и неразделиетлем)
                        if (((char.IsLetterOrDigit(CodeRichTextBox.Lines[i][j]) || CodeRichTextBox.Lines[i][j] == ' ') && ((!char.IsLetterOrDigit(CodeRichTextBox.Lines[i][j + 1])) && CodeRichTextBox.Lines[i][j + 1] != ' ')) || ((!char.IsLetterOrDigit(CodeRichTextBox.Lines[i][j]) && CodeRichTextBox.Lines[i][j] != ' ') && (char.IsLetterOrDigit(CodeRichTextBox.Lines[i][j + 1]) || CodeRichTextBox.Lines[i][j + 1] == ' ')))
                        {
                            str = str + CodeRichTextBox.Lines[i][j] + ' ';
                        }
                        else
                            str += CodeRichTextBox.Lines[i][j];
                    }
                    else if (CodeRichTextBox.Lines[i].Length - j == 1) // Если элемент последний в строке
                    {
                        // Если последний элемент строки - символ и предпоследний элемент строки - символ
                        if ((!char.IsLetterOrDigit(CodeRichTextBox.Lines[i][j]) && CodeRichTextBox.Lines[i][j] != ' ') && (!char.IsLetterOrDigit(CodeRichTextBox.Lines[i][j - 1]) && CodeRichTextBox.Lines[i][j - 1] != ' '))
                            str += CodeRichTextBox.Lines[i][j];


                        // Если последний элемент строки - символ и предпоследний элемент строки - не символ
                        else if ((!char.IsLetterOrDigit(CodeRichTextBox.Lines[i][j]) && CodeRichTextBox.Lines[i][j] != ' ') && (char.IsLetterOrDigit(CodeRichTextBox.Lines[i][j - 1]) || CodeRichTextBox.Lines[i][j - 1] == ' '))
                            str = str + ' ' + CodeRichTextBox.Lines[i][j];

                        else                       
                            str = str + CodeRichTextBox.Lines[i][j];
                        

                        str = str + ' ' + "/-"; // знак /- перенос на новую строку 
                    }
                }
                
                str += ' '; // Разделяем строки richtextbox пробелом
                
            }

            return str;
        }
        private void AnalysisButton_Click(object sender, EventArgs e)
        {
            if (CodeRichTextBox.Text == "")
                MessageBox.Show("Пустой текстовый блок!", "Лексический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {              
                LexicalAnalysis(); // Производим лексический анализ
                tokens = Token.GeneratingListTokens(lexemes, keyWords, separators, variables, literals); // Получаем список токенов
            }
        }

        /// <summary>
        /// Выполняет лексический анализ
        /// </summary>
        private void LexicalAnalysis()
        {
            try
            {
                string text = "";
                bool error = false, correctSymbol = false;

                text = GetDataFromRichTextBox(text);

                for (int i = 0; i < text.Length; i++)
                {
                    if (status == Status.None && Correctness.CharIsEnglishChar(text[i])) // Если анг буква
                        status = Status.I;

                    if (status == Status.None && char.IsDigit(text[i])) // Если число
                        status = Status.D;

                    if (status == Status.None && separators.Contains(text[i].ToString())) // Если символ
                        status = Status.R;


                    if (status != Status.None)
                    {
                        if (status == Status.I) // Ветка буквы
                        {
                            correctSymbol = (Correctness.CharIsEnglishChar(text[i]) || char.IsDigit(text[i])); // проверка на корректность символа 

                            if (correctSymbol && text.Length - i != 1) // Если не пробел и не последний символ
                            {
                                buffer += text[i];

                            }
                            else if (text.Length - i == 1 && correctSymbol) // если последний символ richtextbox
                            {
                                buffer += text[i];

                                error = SuccessfulLexemeAddition(buffer, 'I'); // Проверка на длину строки и сохранение лексемы в случае успеха

                                ClearOfBuffer();
                            }
                            else if (text[i] == ' ') // Если пробел
                            {
                                correctSymbol = true;
                                error = SuccessfulLexemeAddition(buffer, 'I'); // Проверка на длину строки и сохранение лексемы в случае успеха

                                ClearOfBuffer();

                            }

                            else
                                error = true;
                        }

                        if (status == Status.D) // Ветка числа
                        {
                            correctSymbol = char.IsDigit(text[i]);

                            if (correctSymbol && text.Length - i != 1) // Если не конец richtextbox и символ - число
                            {
                                buffer += text[i];
                            }

                            else if (correctSymbol && text.Length - i == 1) // Если последний символ richtextbox
                            {
                                buffer += text[i];
                                error = SuccessfulLexemeAddition(buffer, 'D'); // Проверка на длину строки и сохранение лексемы в случае успеха

                                ClearOfBuffer();
                            }
                            else if (text[i] == ' ') // Если пробел
                            {
                                correctSymbol = true;
                                error = SuccessfulLexemeAddition(buffer, 'D'); // Проверка на длину строки и сохранение лексемы в случае успеха

                                ClearOfBuffer();

                            }
                            else
                                error = true;

                        }

                        if (status == Status.R)
                        {
                            correctSymbol = separators.Contains(text[i].ToString());

                            if (correctSymbol)
                            {
                                if (buffer.Length < 2)
                                    buffer += text[i];
                                else
                                {
                                    error = true;
                                    ClearOfBuffer();
                                }
                            }
                            else if (text[i] == ' ' || Correctness.CharIsEnglishChar(text[i]) || char.IsDigit(text[i]))
                            {
                                if (separators.Contains(buffer))
                                {
                                    Lexeme lex = new Lexeme(buffer, 'R');
                                    lexemes.Add(lex);

                                    correctSymbol = true;

                                    ClearOfBuffer();
                                }
                            }
                            else
                                error = true;
                        }
                    }
                    else
                    {
                        if (text[i] != ' ' && !Correctness.CharIsEnglishChar(text[i]) && !char.IsDigit(text[i]) && !separators.Contains(text[i].ToString()))
                            error = true;

                    }

                    if (error || !correctSymbol) // Обнаружен некорректный символ или превысили планку длины лексемы
                    {
                        string errorMessage = string.Format("Был найден некорректный символ в лексеме - {0}",text[i]);
                        MessageBox.Show(errorMessage, "Лексический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        ResultButton.Enabled = false;

                        ClearOfBuffer();
                        lexemes.Clear();

                        break;
                    }
                }

                if (!error && correctSymbol) // Ошибок нет 
                {
                    MessageBox.Show("Лексический анализ был успешно произведён!", "Лексический анализ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResultButton.Enabled = true;                  
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Лексический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Очищает буффер и аннулирует статус
        /// </summary>
        private void ClearOfBuffer() 
        {
            buffer = "";
            status = Status.None;
        
        }

        /// <summary>
        /// Добавляет лексему в список лексем
        /// </summary>
        /// <param name="buffer">Лексема</param>
        /// <param name="type">Тип лексемы</param>
        /// <returns>Есть ли ошибка в добавлении лексемы в список лексем </returns>
        private bool SuccessfulLexemeAddition(string buffer, char type)
        {
            bool error = false;

            if (type == 'I')
                error = Correctness.IsErrorInLengthOfString(buffer);
            else if (type == 'D')
                error = Correctness.IsErrorInValueOfNumber(buffer);

            if (!error) // Если нет проблем для добавления лексемы
            {
                Lexeme lex = new Lexeme(buffer, type);
                lexemes.Add(lex); // Добавляем лексему в список

                if (type == 'I' && !keyWords.Contains(buffer) && !variables.Contains(buffer)) // Если это переменная, которой нет в списке переменных
                    variables.Add(lex.Name); // Добавляем в список переменных

                else if (type == 'D' && !literals.Contains(buffer)) // Если это литерал, которого нет в списке литералов
                    literals.Add(lex.Name); // Добавляем в список литералов

            }

            return error;
        }

        private void ResultButton_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(lexemes,separators, keyWords, variables,literals,tokens);
            form2.Show();

            ClearningOfLists(); // Очищаем список, чтобы не было повторов
        }

        /// <summary>
        /// Метод очистки методов
        /// </summary>
        private void ClearningOfLists() 
        {
            lexemes.Clear();
            variables.Clear();
            literals.Clear();
            tokens.Clear();
        }
    }
}
