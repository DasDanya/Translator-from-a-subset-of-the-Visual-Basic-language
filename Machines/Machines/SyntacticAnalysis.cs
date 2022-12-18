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
                    MessageBox.Show("Продолжение синтаксического анализа невозможно", "Синтаксический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                
        }

        
        private bool ListOfActions() // список действий
        {
            if (Action())
            {
                // Здесь должен быть Next
                return true;
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
                return false;
            }
            else if (actualLexeme == "If")
            {
                return false;
            }
            else 
            {
                GetMessageErrorAction();
                return false;
            }

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
            MessageBox.Show($"Ожидался перемнос на новую строку или знак +, -, * или / , а встретилось {actualLexeme}", "Синтаксический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        /// <summary>
        /// Метод вывода сообщения о ошибке в правиле "Знак"
        /// </summary>
        private void GetMessageErrorSign()
        {
            MessageBox.Show($"Ожидался знак +, -, * или / , а встретилось {actualLexeme}", "Синтаксический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}
