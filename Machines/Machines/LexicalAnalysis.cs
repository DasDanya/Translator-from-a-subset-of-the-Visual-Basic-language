using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace Machines
{
    /// <summary>
    /// Класс транслятора 
    /// </summary>
    public class LexicalAnalysis
    {
        string buffer = ""; // Строка для лексического анализа
        Status status = Status.None; // Статус состояния
        bool errorInLength = false; // ошибка превышения длины лексемы

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
        /// Выполняет лексический анализ    
        /// </summary>
        public bool Analysis(string text, Button ResultButton)
        {

            bool error = false, correctSymbol = false;

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
                            {
                                buffer += text[i];

                                if (!Classification.separators.Contains(buffer)) 
                                {
                                    error = true;
                                }
                            }
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

                if (error || !correctSymbol & text[i] != ' ') // Обнаружен некорректный символ или превысили планку длины лексемы
                {

                    // Формируем сообщение об ошибке
                    string errorMessage = string.Format("Был найден некорректный символ {0} в коде", text[i]);

                    if (!errorInLength)
                    {
                        MessageBox.Show(errorMessage, "Лексический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    ResultButton.Enabled = false;
                    ClearOfBuffer();
                    Classification.lexemes.Clear();
                    errorInLength = false;
                    return false;
                }


            }
            if (!error && correctSymbol) // Ошибок нет 
            {
                MessageBox.Show("Лексический анализ был успешно произведён!", "Лексический анализ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ResultButton.Enabled = true;              
            }

            return true;
        }

        /// <summary>
        /// Добавляет лекскему в список лексем
        /// </summary>
        /// <param name="lexeme">Лексема</param>
        /// <param name="type">Тип лексемы</param>
        /// <returns>Есть ли ошибка в добавлении лексемы в список лексем </returns>
        private bool SuccessfulLexemeAddition(string lexeme, char type)
        {
            
            bool error = false;
            try
            {
                if (type == 'I') // Если id
                    error = Correctness.IsErrorInLengthOfString(lexeme);
                else if (type == 'D') // Если число
                    error = Correctness.IsErrorInValueOfNumber(lexeme);

                if (!error) // Если нет проблем для добавления лексемы
                {
                    Lexeme lex = new Lexeme(lexeme, type);
                    Classification.lexemes.Add(lex); // Добавляем лексему в список

                    if (type == 'I' && !Classification.KeyWords.Contains(lexeme) && !Classification.variables.Contains(lexeme)) // Если это переменная, которой нет в списке переменных
                        Classification.variables.Add(lex.Name); // Добавляем в список переменных

                    else if (type == 'D' && !Classification.literals.Contains(lexeme)) // Если это литерал, которого нет в списке литералов
                        Classification.literals.Add(lex.Name); // Добавляем в список литералов

                }
                else 
                {
                    errorInLength = true;
                    //MessageBox.Show($"Превышена длина у лексемы: {lexeme}", "Лексический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex) 
            {
                errorInLength = true;
                MessageBox.Show(ex.Message, "Лексический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = true;
            }

            return error;
        }

        /// <summary>
        /// Очищает буффер и аннулирует статус
        /// </summary>
        private void ClearOfBuffer()
        {
            buffer = "";
            status = Status.None;
        }
    }
}

