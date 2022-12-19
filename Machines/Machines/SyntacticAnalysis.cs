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
        int numLexeme = 0;
        string actualLexeme = "";
        List<Token> tokens = new List<Token>();

        public SyntacticAnalysis(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        private void Next() 
        {
            numLexeme++;

            if (tokens.Count > numLexeme)
                actualLexeme = Classification.GetLexeme(tokens[numLexeme]);
            else
                actualLexeme = "Пустота";
        }

        

        private void Past() 
        {
            numLexeme--;
            actualLexeme = Classification.GetLexeme(tokens[numLexeme]);
        }
       
        public void StartAnalysis()  // программа
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
                        GetMessageEliminationRecursionAction();
                        return false;
                    }
                }
                else
                {
                    GetMessageErrorEnd();
                    return false;
                }
            }
            else
            {
                GetMessageErrorTransferNewLine();
                return false;
            }
        }

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

        private bool ConditionalOperator() // Условный оператор
        {
            if (actualLexeme != "If")
            {
                GetMessageErrorKeyWords("If");
                return false;
            }
            else
                Next();

            Expr();

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

        private void Expr() // Метод-заглушка сложжного логического выражения 
        {
            while (actualLexeme != "Then") 
            {
                Next();
            }
        }

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


        private bool Operand()  // Оператор
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
            MessageBox.Show($"Ожидались If или Dim или переменная, а встретилось {actualLexeme}", "Синтаксический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            MessageBox.Show($"Ожидался перенос на новую строку или знак +, -, * или / , а встретилось {actualLexeme}", "Синтаксический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        /// <summary>
        /// Метод вывода сообщения о ошибке в правиле "Знак"
        /// </summary>
        private void GetMessageErrorSign()
        {
            MessageBox.Show($"Ожидался знак +, -, * или / , а встретилось {actualLexeme}", "Синтаксический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        /// <summary>
        /// Метод вывода сообщения об ошибке в правиле "Устранение левой факторизации список переменных"
        /// </summary>
        private void GetMessageErrorEliminationLeftFactorListOfVariables() 
        {
            MessageBox.Show($"Ожидалось ключевое слово As или запятая, а встретилось {actualLexeme}", "Синтаксический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Метод вывода сообщения об ошибке в правиле "Тип"
        /// </summary>
        private void GetMessageErrorType() 
        {
            MessageBox.Show($"Ожидался тип данных: Double или Short или Integer, а встретилось {actualLexeme}", "Синтаксический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Метод вывода сообщения об ошибке в правиле "Устранение левой факторизации описание"
        /// </summary>
        private void GetMessageEliminationLeftFactorDescription() 
        {
            MessageBox.Show($"Ожидался переход на новую строку или знак =, а встретилось {actualLexeme}", "Синтаксический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Метод вывода сообщения о невозможности продолжения выполнения кода
        /// </summary>
        private void GetMessageErrorEnd() 
        {
            MessageBox.Show("Продолжение синтаксического анализа невозможно", "Синтаксический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Метод вывода сообщения об ошибке в правиле "Устранение левой рекурсии действие"
        /// </summary>
        private void GetMessageEliminationRecursionAction() 
        {
            MessageBox.Show($"Ожидался If или Dim или переменная или End или Else, а встретилось {actualLexeme}", "Синтаксический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Метод вывода сообщение об ошибке в правиле "Устранение левой факторизации условный оператор"
        /// </summary>
        private void GetMessageErrorEliminationLeftFactorConditionalOperator() 
        {
            MessageBox.Show($"Ожидался End или Else, а встретилось {actualLexeme}", "Синтаксический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
