using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Machines
{

    /// <summary>
    /// Класс для синтаксического анализа 
    /// </summary>
     public class SyntacticAnalysis
     {
        int success = 0;
        int numLexeme = 0; // Номер лексемы
        int numLogicOperation = 0; // номер логической операции
        string actualLexeme = ""; // текущая лексема
        List<Token> tokens = new List<Token>(); // список токенов
        Stack<string> E = new Stack<string>(); // операнд
        Stack<string> T = new Stack<string>(); // операции
        List<string> matrix = new List<string>(); // матрица сложных логических операций
        RichTextBox textBox = null;

        public SyntacticAnalysis(List<Token> tokens, RichTextBox richTextBox)
        {
            this.tokens = tokens;
            textBox = richTextBox;
        }

        /// <summary>
        /// Метод, который берет следующую лексему
        /// </summary>
        private void Next() 
        {
            numLexeme++;

            if (tokens.Count > numLexeme)
                actualLexeme = Classification.GetLexeme(tokens[numLexeme]);
            else
                actualLexeme = "Пустота";
        }

        
        /// <summary>
        /// Метод, который берет прошлую лексему
        /// </summary>
        private void Past() 
        {
            numLexeme--;
            actualLexeme = Classification.GetLexeme(tokens[numLexeme]);
        }
       
        /// <summary>
        /// Программа
        /// </summary>
        public void StartAnalysis()  
        {
            actualLexeme = Classification.GetLexeme(tokens[0]);

            
                if (actualLexeme != "Sub") 
                {
                    GetMessageErrorKeyWords("Sub");
                    return;
                }
                else
                    Next();

                if (actualLexeme != "Main")
                {
                    GetMessageErrorKeyWords("Main");
                    return;
                }
                else
                    Next();

                if (actualLexeme != "()")
                {
                    GetMessageErrorSymbol("()");
                    return;
                }
                else
                    Next();

            //if (actualLexeme != ")") 
            //{
            //    GetMessageErrorSymbol(")");
            //    return;
            //}
            //else
            //    Next();

            if (actualLexeme != "/-")
            {
                GetMessageErrorTransferNewLine();             
                return;
            }
            else
                Next();

            if (!ListOfActions())
            {
                return;
            }
            else 
            {
                if (actualLexeme != "/-") // Если это убрать, то будет ошибка в простом присваивании
                    Next();

            }


            if (actualLexeme != "/-")
            {
                GetMessageErrorTransferNewLine();
                return;
            }
            else
                Next();

            if (actualLexeme != "End")
            {
                GetMessageErrorKeyWords("End");
                return;
            }
            else
                Next();

            if (actualLexeme != "Sub")
            {
                GetMessageErrorKeyWords("Sub");
                return;
            }
            else 
            {
                if (tokens.Count - numLexeme == 1)
                    MessageBox.Show("Синтаксический анализ успешно произведён!", "Синтаксический анализ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    GetMessageErrorEnd();
            }
                
        }

        /// <summary>
        /// Список действий
        /// </summary>
        /// <returns>Успешен ли разбор</returns>
        private bool ListOfActions() // список действий
        {
            if (Action())
            {
                //MessageBox.Show($" action: {actualLexeme}");

                if (actualLexeme != "/-")
                    Next();

                return EliminationOfLeftFactorListOfAction();
            }
            else
                return false;


        }

        /// <summary>
        /// Действие
        /// </summary>
        /// <returns>Успешен ли разбор</returns>
        private bool Action() // действие
        {
            if (Classification.isId(actualLexeme))
            {
                return Assignment();

            }
            else if (actualLexeme == "Dim")
            {
                return Description();
            }
            else if (actualLexeme == "If")
            {
                return ConditionalOperator();
            }
            else 
            {
                GetMessageErrorAction();
                return false;
            }

        }

        /// <summary>
        /// Устранение левой факторизации список действий
        /// </summary>
        /// <returns></returns>
        private bool EliminationOfLeftFactorListOfAction() // устранение левой факторизации список действий
        {
            if (actualLexeme == "/-")
            {
                //MessageBox.Show(Classification.GetLexeme(tokens[numLexeme - 1]));
                if (tokens.Count - numLexeme > 1)
                {
                    Next();
                    //MessageBox.Show($" next lev rek {actualLexeme}");
                    if (actualLexeme == "End" || actualLexeme == "Else")
                    {
                        Past();
                        //MessageBox.Show($"past {actualLexeme}");
                        return true;
                    }

                    else if (actualLexeme == "Dim" || actualLexeme == "If" || Classification.isId(actualLexeme))
                    {
                        Past();
                        //MessageBox.Show(actualLexeme);

                        return EliminationOfLeftRecursionListOfAction();
                    }
                    else 
                    {
                        GetMessageEliminationLeftFactorListOfAction();
                        return false;
                    }
                }
                else
                {
                    GetMessageEliminationLeftFactorListOfAction();
                    //GetMessageErrorEnd();
                    return false;
                }
            }
            else
            {
                GetMessageErrorTransferNewLine();
                return false;
            }
        }

        /// <summary>
        /// Устранение левой рекурсии список действий
        /// </summary>
        /// <returns>Успешен ли разбор</returns>
        private bool EliminationOfLeftRecursionListOfAction() // устранение левой рекурсии список действий
        {
            if (actualLexeme != "/-")
            {
                GetMessageErrorTransferNewLine();
                return false;
            }
            else
                Next();

            if (!Action())
                return false;
            else
            {
                if (actualLexeme != "/-") // Если убрать это условие, то он будет пропускать /- , так как после действия текущая лексема - /-
                    Next();
            }

            return EliminationOfLeftFactorListOfAction();
        }

        /// <summary>
        /// Присваивание
        /// </summary>
        /// <returns>Успешен ли разбор</returns>
        private bool Assignment() // присваивание
        {
            if (!Classification.isId(actualLexeme))
            {
                GetMessageErrorId();
                return false;
            }
            else
                Next();

            if (actualLexeme != "=")
            {
                GetMessageErrorSymbol("=");
                return false;
            }
            else
                Next();

            if (!Classification.isLiteral(actualLexeme) & !Classification.isId(actualLexeme)) 
            {
                GetMessageErrorIdOrLiteral();
                return false;
            }
            else 
            {
                if (!Operand())
                {
                    return false;
                }
                else
                    Next();
                
            }

            //if (!EliminationLeftFactorAssignment())
            //{
            //    return false;
            //}
            //else
            //    return true;

            return EliminationLeftFactorAssignment();

        }

        /// <summary>
        /// Описание
        /// </summary>
        /// <returns>Успешен ли разбор</returns>
        private bool Description() // описание
        {
            if (actualLexeme != "Dim")
            {
                GetMessageErrorKeyWords("Dim");
                return false;
            }
            else
                Next();


            if (!ListOfVariables())
            {
                return false;
            }
            else
                Next();

            if (!Type())
            {
                return false;
            }
            else
                Next();


            return EliminationLeftFactorDescription();

        }

        /// <summary>
        /// Условный оператор
        /// </summary>
        /// <returns>Успешен ли разбор</returns>
        private bool ConditionalOperator() // Условный оператор
        {
            if (actualLexeme != "If")
            {
                GetMessageErrorKeyWords("If");
                return false;
            }
            else
                Next();

            if (actualLexeme == "Then" || actualLexeme == "/-" || actualLexeme == "Пустота")
            {
                GetMessageErrorWaitingExpr();
                return false;
            }
            else
            {
                Expr();

                if (success == 1 & matrix.Count > 0)
                {
                    textBox.Text += $"Логическое выражение №{++numLogicOperation}\n";

                    foreach (var item in matrix)
                    {
                        textBox.Text += item + "\n";    
                    }
                }
                else 
                {
                    return false;
                }

                matrix.Clear();
                success = 0;
            }

            if (actualLexeme != "Then")
            {
               
                GetMessageErrorKeyWords("Then");
                return false;
            }
            else
                Next();

            if (actualLexeme != "/-")
            {
                GetMessageErrorTransferNewLine();
                return false;
            }
            else
                Next();

            if (!ListOfActions())
            {
                return false;
            }
            else
            {
                if (actualLexeme != "/-") // Если это убрать, то будет ошибка в простом присваивании
                    Next();
            }

            if (actualLexeme != "/-")
            {
                GetMessageErrorTransferNewLine();
                return false;
            }
            else
                Next();

            return EliminationLeftFactorConditionalOperator();


        }


        private void Expr() // Метод проверки сложного логического выражения 
        {

            if (tokens.Count - numLexeme > 1)
            {
                if (Classification.isId(actualLexeme) || Classification.isLiteral(actualLexeme)) // Если текущая лексема - операнд
                {
                    Next();
                    if (actualLexeme == "(" || Classification.isId(actualLexeme) || Classification.isLiteral(actualLexeme)) // новое условие: после id или литерала не может стоять открывающаяся скобка
                    {
                        MessageBox.Show($"После литерала или переменной не может стоять ( , или литерал, или переменная", "Логическое выражение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        Past();
                        E.Push(actualLexeme);
                        Next();
                        Expr();
                    }
                }
                else if (actualLexeme == "(" || actualLexeme == "<" || actualLexeme == ">" || actualLexeme == "=" || actualLexeme == "<>" || actualLexeme == "Or" || actualLexeme == "Xor" || actualLexeme == "And" || actualLexeme == ")" || actualLexeme == "Then" || actualLexeme == "<=" || actualLexeme == ">=")
                {

                    if (actualLexeme != "Then" & actualLexeme != ")") // После сравнения должен быть литерал, id или открывающаяся скобка
                    {
                        Next();
                        if (Classification.isId(actualLexeme) || Classification.isLiteral(actualLexeme) || actualLexeme == "(")
                            Past();
                        else
                        {
                            MessageBox.Show($"Ожидался литерал, переменная или (, а встретилось {actualLexeme}", "Логическое выражение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    if (actualLexeme == "(")
                    {
                        D1();
                        Expr();
                    }
                    else if (actualLexeme == "<" || actualLexeme == ">" || actualLexeme == "=" || actualLexeme == "<>" || actualLexeme == "<=" || actualLexeme == ">=")
                    {
                        if (T.Count == 0 || T.Peek() == "(")
                        {
                            D1();
                            Expr();
                        }
                        else if (T.Peek() == "<" || T.Peek() == ">" || T.Peek() == "=" || T.Peek() == "<>" || T.Peek() == "<=" || T.Peek() == ">=")
                        {
                            if (D2())
                                Expr();
                            else
                            return;
                        }
                        else if (T.Peek() == "Or" || T.Peek() == "Xor" || T.Peek() == "And")
                        {
                            if (D4())
                                Expr();
                            else
                                return;
                            
                        }
                    }
                    else if (actualLexeme == "Or" || actualLexeme == "Xor")
                    {
                        if (T.Count == 0 || T.Peek() == "(" || T.Peek() == "<" || T.Peek() == ">" || T.Peek() == "=" || T.Peek() == "<>" || T.Peek() == "<=" || T.Peek() == ">=")
                        {
                            D1();
                            Expr();
                        }
                        else if (T.Peek() == "Xor" || T.Peek() == "Or")
                        {
                            if (D2())
                                Expr();
                            else
                                return;
                        }
                        else if (T.Peek() == "And")
                        {
                            if (D4())
                                Expr();
                            else
                                return;
                        }
                    }
                    else if (actualLexeme == "And")
                    {
                        if (T.Count == 0 || T.Peek() == "(" || T.Peek() == "<" || T.Peek() == ">" || T.Peek() == "=" || T.Peek() == "<>" || T.Peek() == "Or" || T.Peek() == "Xor" || T.Peek() == "<=" || T.Peek() == ">=")
                        {
                            D1();
                            Expr();
                        }
                        else if (T.Peek() == "And")
                        {
                            if (D2())
                                Expr();
                            else
                                return;
                        }
                    }
                    else if (actualLexeme == ")")
                    {
                        Next(); // Новое условие: после открывающей скобки не может быть открывающая
                        if (actualLexeme == "(")
                        {
                            MessageBox.Show($"Ожидался оператор сравнения или Xor, Or, And, а встретилось {actualLexeme}", "Логическое выражение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else 
                        {
                            Past();
                        }

                        if (T.Count == 0)
                        {
                            // Написать ошибку
                            MessageBox.Show($"Несбалансированная {actualLexeme}", "Логическое выражение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else if (T.Peek() == "(")
                        {
                            D3();
                            Expr();
                        }
                        else if (T.Peek() == "<" || T.Peek() == ">" || T.Peek() == "=" || T.Peek() == "<>" || T.Peek() == "Or" || T.Peek() == "Xor" || T.Peek() == "And" || T.Peek() == "<=" || T.Peek() == ">=")
                        {
                            if (D4())
                                Expr();
                            else
                                return;
                        }
                    }
                    else if (actualLexeme == "Then")
                    {
                        if (T.Count == 0)
                        {
                            //if (E.Count == 0)
                            //{
                            //    check = 1;
                            //}
                            //else 
                            //{
                            //    MessageBox.Show("Не хватает операции и операнда для проверки условия", "Синтаксический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //}

                            if (CheckOperands())
                            {
                                GetMessageErrorWaitingOperationAndOperand();
                            }
                            else
                            {
                                success = 1;
                            }

                            //MessageBox.Show($"T count: {T.Count}");
                            return;
                        }
                        else if (T.Peek() == "<" || T.Peek() == ">" || T.Peek() == "=" || T.Peek() == "<>" || T.Peek() == "Or" || T.Peek() == "Xor" || T.Peek() == "And" || T.Peek() == "<=" || T.Peek() == ">=")
                        {
                            if (D4())
                                Expr();
                            else
                                return;

                            if (T.Count != 0) // Недавно добавил чек
                            {
                                //MessageBox.Show($"Error symbol {T.Peek()}\nCount:{T.Count}");
                                return;
                            }
                        }
                        else if (T.Peek() == "(")
                        {
                            // Сообщение об ошибке
                            MessageBox.Show($"Несбалансированная {T.Peek()}", "Логическое выражение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                else
                {
                    GetMessageErrorLogicalExpression();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Ожидалось продолжение кода, а встретилась пустота", "Логическое выражение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //GetMessageErrorEnd();
                return;
            }

            //return success;
        }

        private void D1() // действие D1
        {
            T.Push(actualLexeme);
            Next();
        }


        private bool D2() 
        {

            if (!D4())
                return false;

            else
            {
                T.Push(actualLexeme);
                Next();
                return true;
            }
        }

        private void D3() 
        {
            T.Pop();
            //string shrck  = T.Pop(); // это убрать
            //MessageBox.Show(shrck); // это убрать
            Next();
        }

        private bool D4() 
        {
            if (E.Count >= 2)
            {
                string operand = T.Pop();
                //MessageBox.Show(operand);
                string firstOperation = E.Pop();
                string secondOperation = E.Pop();

                //if (CorrectAndOrXor(operand, firstOperation, secondOperation))
                //{
                    string bufferMatrix = $"Разбор{matrix.Count + 1}";

                    matrix.Add($"{bufferMatrix}: {operand} {secondOperation} {firstOperation}");
                    E.Push(bufferMatrix);
                    return true;
                //}
                //else 
                //{
                //    GetMessageErrorWaitingOperationAndOperand();
                //    return false;
                //}

                //string bufferMatrix = $"Разбор{matrix.Count + 1}";

                ////matrix.Add($"{bufferMatrix}: {T.Pop()} {E.Pop()} {E.Pop()}");
                //E.Push(bufferMatrix);
                //return true;
            }
            else 
            {
                MessageBox.Show("Не хватает операнда для проверки условия", "Логическое выражение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            
            }
            //MessageBox.Show($"T peek: {T.Peek()}");
        }

        /// <summary>
        /// Метод для проверки правильности работы And Xor Or
        /// </summary>
        /// <param name="operand">операнд</param>
        /// <param name="firstOperation">первый операнд</param>
        /// <param name="secondOperation">второй операнд</param>
        /// <returns></returns>
        private bool CorrectAndOrXor(string operand, string firstOperation, string secondOperation) 
        {
            bool success = true;
            if (operand == "And" || operand == "Or" || operand == "Xor")
            {
                if (firstOperation.Contains("б") & secondOperation.Contains("б"))
                {
                    success = true;
                }
                else 
                {
                    success = false;
                }
            }

            return success;
        }

        /// <summary>
        /// Метод проверки стека с операндами на наличие лишнего операнда
        /// </summary>
        /// <returns>Есть ли в стеке с операндами лишний операнд</returns>
        private bool CheckOperands() 
        {
            bool haveExtraOperand = false;

            string[] arrOperands = E.ToArray();

            for (int i = 0; i < arrOperands.Length; i++)
            {
                if (!arrOperands[i].Contains('б')) 
                {
                    haveExtraOperand = true;
                    break;
                }
            }

            return haveExtraOperand;
        }

        /// <summary>
        /// Устранение левой факторизации условный оператор
        /// </summary>
        /// <returns>Успешен ли разбор</returns>
        private bool EliminationLeftFactorConditionalOperator() // устранение левой факторизации условный оператор
        {
            if (actualLexeme == "End")
            {
                Next();

                if (actualLexeme != "If")
                {
                    GetMessageErrorKeyWords("If");
                    return false;
                }
                else
                    return true;
            }
            else if (actualLexeme == "Else")
            {
                Next();

                if (actualLexeme != "/-")
                {
                    GetMessageErrorTransferNewLine();
                    return false;
                }
                else
                    Next();

                if (!ListOfActions())
                {
                    return false;
                }
                else
                {
                    if (actualLexeme != "/-") // Если это убрать, то будет ошибка в простом присваивании
                        Next();
                }

                if (actualLexeme != "/-")
                {
                    GetMessageErrorTransferNewLine();
                    return false;
                }
                else
                    Next();

                if (actualLexeme != "End")
                {
                    GetMessageErrorKeyWords("End");
                    return false;
                }
                else
                    Next();

                if (actualLexeme != "If")
                {
                    GetMessageErrorKeyWords("If");
                    return false;
                }
                else
                    return true;
            }
            else 
            {
                GetMessageErrorEliminationLeftFactorConditionalOperator();
                return false;
            }

        }

        /// <summary>
        /// Список переменных
        /// </summary>
        /// <returns>Успешен ли разбор</returns>
        private bool ListOfVariables() // список переменных
        {
            if (!Classification.isId(actualLexeme))
            {
                GetMessageErrorId();
                return false;
            }
            else
                Next();


            return EliminationLeftFactorListOfVariables();
        }

        /// <summary>
        /// Устранение левой факторизации список переменных
        /// </summary>
        /// <returns>Успешен ли разбор</returns>
        private bool EliminationLeftFactorListOfVariables() // устранение левой факторизации список переменных
        {
            if (actualLexeme == "As")
            {
                return true;
            }
            else if (actualLexeme == ",")
            {
                return EliminationLeftRecursionListOfVariables();
            }
            else
            {
                GetMessageErrorEliminationLeftFactorListOfVariables();
                return false;
            }
        }

        /// <summary>
        /// Устранение левой рекурсии список переменных
        /// </summary>
        /// <returns>Успешен ли разбор</returns>
        private bool EliminationLeftRecursionListOfVariables() // устранение левой рекурсии список переменных
        {
            if (actualLexeme != ",")
            {
                GetMessageErrorSymbol(",");
                return false;
            }
            else
                Next();

            if (!Classification.isId(actualLexeme))
            {
                GetMessageErrorId();
                return false;
            }
            else
                Next();

            return EliminationLeftFactorListOfVariables();
        }

        /// <summary>
        /// Устранение левой факторизации описание
        /// </summary>
        /// <returns>Успешен ли разбор</returns>
        private bool EliminationLeftFactorDescription() // устранение левой факторизации описание 
        {
            if (actualLexeme == "/-")
            {
                return true;
            }

            else if (actualLexeme == "=")
            {
                Next();

                if (!Operand())
                    return false;
                else
                {
                    Next();
                    return EliminationLeftFactorAssignment();
                }
                    
            }
            else 
            {                
                GetMessageEliminationLeftFactorDescription();
                return false;
            }

        }

        /// <summary>
        /// Устранение левой факторизации присваивание
        /// </summary>
        /// <returns>Успешен ли разбор</returns>
        private bool EliminationLeftFactorAssignment() // устранение левой факторизации присваивание
        {
            if (actualLexeme == "/-")
            {
                return true;
            }
            else if (actualLexeme == "+" || actualLexeme == "-" || actualLexeme == "*" || actualLexeme == "/")
            {
                if (Sign())
                {
                    Next();
                    return Operand();
                }
                else
                {
                    GetMessageErrorSign();
                    return false;
                }
            }
            else 
            {
                GetMessageErrorEliminationLeftFactorAssignment();
                return false;
            }
        
        }

        /// <summary>
        /// Знак
        /// </summary>
        /// <returns>Успешен ли разбор</returns>
        private bool Sign() // Знак
        {
            if (actualLexeme == "+" || actualLexeme == "-" || actualLexeme == "*" || actualLexeme == "/")
            {
                return true;
            }
            else 
            {
                GetMessageErrorSign();
                return false;
            }

        }

        /// <summary>
        /// Операнд
        /// </summary>
        /// <returns>Успешен ли разбор</returns>
        private bool Operand()  // Операнд
        {
            if (!Classification.isLiteral(actualLexeme) & !Classification.isId(actualLexeme))
            {
                GetMessageErrorIdOrLiteral();
                return false;
            }
            else 
            {
                return true;
            }

        }

        /// <summary>
        /// Тип
        /// </summary>
        /// <returns>Успешен ли разбор</returns>
        private bool Type() // Тип
        {
            if (actualLexeme == "Double" || actualLexeme == "Short" || actualLexeme == "Integer")
                return true;
            else
            {
                GetMessageErrorType();
                return false;
            }
        }

        /// <summary>
        /// Метод вывода сообщения о том, что ожидалось ключевое слово
        /// </summary>
        /// <param name="keyWord">Ключевое слово</param>
        private void GetMessageErrorKeyWords(string keyWord) 
        {
            MessageBox.Show($"Ожидалось ключевое слово {keyWord}, а встретилось {actualLexeme}", "Синтаксический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Метод вывода сообщения о том, что ожидался символ
        /// </summary>
        /// <param name="symbol">Символ</param>
        private void GetMessageErrorSymbol(string symbol) 
        {
            MessageBox.Show($"Ожидался символ {symbol}, а встретилось {actualLexeme}", "Синтаксический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Метод вывода сообщения о том, что ожидался переход на новую строку
        /// </summary>
        private void GetMessageErrorTransferNewLine() 
        {
            MessageBox.Show($"Ожидался переход на новую строку, а встретилось {actualLexeme}", "Синтаксический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Метод вывода сообщения об ошибке в правиле "Действие"
        /// </summary>
        private void GetMessageErrorAction() 
        {
            MessageBox.Show($"Ожидались If или Dim или переменная, а встретилось {actualLexeme}", "Действие", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Метод вывода сообщения о том, что ожидалась переменная
        /// </summary>
        private void GetMessageErrorId() 
        {
            MessageBox.Show($"Ожидалась переменная, а встретилось {actualLexeme}", "Синтаксический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Метод вывода сообщения о том, что ожидалась переменная или литерал
        /// </summary>
        private void GetMessageErrorIdOrLiteral()
        {
            MessageBox.Show($"Ожидалась переменная или литерал, а встретилось {actualLexeme}", "Синтаксический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Метод вывода сообщения о ошибке в правиле "Устранение левой факторизации присваивание"
        /// </summary>
        private void GetMessageErrorEliminationLeftFactorAssignment()
        {
            MessageBox.Show($"Ожидался перенос на новую строку или знак +, -, * или / , а встретилось {actualLexeme}", "Устранение левой факторизации присваивание", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        /// <summary>
        /// Метод вывода сообщения о ошибке в правиле "Знак"
        /// </summary>
        private void GetMessageErrorSign()
        {
            MessageBox.Show($"Ожидался знак +, -, * или / , а встретилось {actualLexeme}", "Знак", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        /// <summary>
        /// Метод вывода сообщения об ошибке в правиле "Устранение левой факторизации список переменных"
        /// </summary>
        private void GetMessageErrorEliminationLeftFactorListOfVariables() 
        {
            MessageBox.Show($"Ожидалось ключевое слово As или запятая, а встретилось {actualLexeme}", "Устранение левой факторизации список переменных", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Метод вывода сообщения об ошибке в правиле "Тип"
        /// </summary>
        private void GetMessageErrorType() 
        {
            MessageBox.Show($"Ожидался тип данных: Double или Short или Integer, а встретилось {actualLexeme}", "Тип", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Метод вывода сообщения об ошибке в правиле "Устранение левой факторизации описание"
        /// </summary>
        private void GetMessageEliminationLeftFactorDescription() 
        {
            MessageBox.Show($"Ожидался переход на новую строку или знак =, а встретилось {actualLexeme}", "Устранение левой факторизации описание", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Метод вывода сообщения о невозможности продолжения выполнения кода
        /// </summary>
        private void GetMessageErrorEnd() 
        {
            MessageBox.Show("Продолжение синтаксического анализа невозможно", "Синтаксический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Метод вывода сообщения об ошибке в правиле "Устранение левой факторизации список действий"
        /// </summary>
        private void GetMessageEliminationLeftFactorListOfAction() 
        {
            MessageBox.Show($"Ожидался If или Dim или переменная или End или Else, а встретилось {actualLexeme}", "Устранение левой факторизации список действий", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Метод вывода сообщение об ошибке в правиле "Устранение левой факторизации условный оператор"
        /// </summary>
        private void GetMessageErrorEliminationLeftFactorConditionalOperator() 
        {
            MessageBox.Show($"Ожидался End или Else, а встретилось {actualLexeme}", "Устранение левой факторизации условный оператор", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Метод вывода сообщения о том, что ожидалось условие
        /// </summary>
        private void GetMessageErrorWaitingExpr() 
        {
            MessageBox.Show($"Ожидалось условие, а встретилось {actualLexeme}", "Синтаксический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        /// <summary>
        /// Метод вывода сообщения о том, что введен некорректный символ для разбора сложного логического выражения
        /// </summary>
        private void GetMessageErrorLogicalExpression() 
        {
            MessageBox.Show($"Допустимая лексема в условии: переменная или литерал, знак: (<, >, =, <>, <=, >=) или ключевое слово: OR, XOR, AND, Then, а встретилось {actualLexeme}", "Логическое выражение", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Метод вывода сообщения о том, что ожидались оператор и операнд
        /// </summary>
        private void GetMessageErrorWaitingOperationAndOperand() 
        {
            MessageBox.Show("Не хватает операции и операнда для проверки условия", "Логическое выражение", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
