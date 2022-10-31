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
            return str;
        }
        private void AnalysisButton_Click(object sender, EventArgs e)
        {
            if (CodeTextBox.Text == "")
                MessageBox.Show("Пустой текстовый блок!", "Лексический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                LexicalAnalysis(); // Производим лексический анализ
                Classification.tokens = Token.GeneratingListTokens(Classification.lexemes, Classification.KeyWords, Classification.separators, Classification.variables, Classification.literals); // Получаем список токенов
            }
        }
        /// <summary>
        /// Выполняет лексический анализ
        /// </summary>
        private void LexicalAnalysis()
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

                if (status == Status.None && Classification.SingleSeparations.Contains(text[i])) // Если символ
                    status = Status.R;

                if (status != Status.None)
                {

                    if (status == Status.I) // Ветка буквы
                    {
                        correctSymbol = (Correctness.CharIsEnglishChar(text[i]) || char.IsDigit(text[i])); // буква или число

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
                        correctSymbol = char.IsDigit(text[i]); // Число или нет
                        if (correctSymbol && text.Length - i != 1) // Если не конец richtextbox и символ - число
                        {
                            buffer += text[i];
                        }
                        else if (correctSymbol && text.Length - i == 1) // Если последний символ richtextbox
                        {
                            buffer += text[i];
                            error = SuccessfulLexemeAddition(buffer, 'D'); //  Проверка на длину строки и сохранение лексемы в случае успеха

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
                    if (status == Status.R) // Ветка символа
                    {
                        correctSymbol = Classification.SingleSeparations.Contains(text[i]); // Проверка на корректный символ
                        if (correctSymbol) // Если корректный и собираем разделитель в буфере
                        {
                            if (buffer.Length < 2) // Если менее трех символов 
                                buffer += text[i];
                            else // Если разделить из трех символов
                            {
                                error = true;
                                ClearOfBuffer();
                            }
                        }
                        else if (text[i] == ' ' || Correctness.CharIsEnglishChar(text[i]) || char.IsDigit(text[i])) // Если закончился разделитель
                        {
                            if (buffer.Length == 1) // Если одиночный
                            {
                                Lexeme lex = new Lexeme(buffer, 'R');
                                Classification.lexemes.Add(lex);
                                correctSymbol = true;
                                ClearOfBuffer();
                            }
                            else if (buffer.Length == 2) // Если двойной
                            {
                                if (Classification.DoubleSeparations.Contains(buffer)) // Есть ли в списке двойных разделителей
                                {
                                    Lexeme lex = new Lexeme(buffer, 'R');
                                    Classification.lexemes.Add(lex);
                                    correctSymbol = true;
                                    ClearOfBuffer();
                                }
                                else
                                    error = true;
                            }
                        }
                        else
                            error = true;
                    }
                }
                else
                {
                    // Первый символ буфера - некорректный символ (не буква, не число, не разрешенный символ)
                    if (text[i] != ' ' && !Correctness.CharIsEnglishChar(text[i]) && !char.IsDigit(text[i]) && !Classification.SingleSeparations.Contains(text[i]))
                        error = true;
                }

                if (error || !correctSymbol && text[i] != ' ') // Обнаружен некорректный символ или превысили планку длины лексемы
                {
                    //if (text[i] == ' ') // Если проб
                    //    continue;

                    MessageBox.Show("Была найдена некорректная лексема!", "Лексический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ResultButton.Enabled = false;
                    ClearOfBuffer();
                    Classification.lexemes.Clear();
                    break;
                }
            }
            if (!error && correctSymbol) // Ошибок нет 
            {
                MessageBox.Show("Лексический анализ был успешно произведён!", "Лексический анализ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ResultButton.Enabled = true;
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
        /// Добавляет лекскему в список лексем
        /// </summary>
        /// <param name="buffer">Лексема</param>
        /// <param name="type">Тип лексемы</param>
        /// <returns>Есть ли ошибка в добавлении лексемы в список лексем </returns>
        private bool SuccessfulLexemeAddition(string buffer, char type)
        {
            bool error = false;

            if (type == 'I') // Если id
                error = Correctness.IsErrorInLengthOfString(buffer);
            else if (type == 'D') // Если число
                error = Correctness.IsErrorInValueOfNumber(buffer);

            if (!error) // Если нет проблем для добавления лексемы
            {
                Lexeme lex = new Lexeme(buffer, type);
                Classification.lexemes.Add(lex); // Добавляем лексему в список

                if (type == 'I' && !Classification.KeyWords.Contains(buffer) && !Classification.variables.Contains(buffer)) // Если это переменная, которой нет в списке переменных
                    Classification.variables.Add(lex.Name); // Добавляем в список переменных

                else if (type == 'D' && !Classification.literals.Contains(buffer)) // Если это литерал, которого нет в списке литералов
                    Classification.literals.Add(lex.Name); // Добавляем в список литералов

            }

            return error;
        }

        

        private void ResultButton_Click(object sender, EventArgs e)
        {
            ResultButton.Enabled = false;

            Form2 form2 = new Form2();
            form2.Show(); 

        }
    }
}

