using System;
using System.Collections.Generic;
using System.Text;

namespace Machines
{
    /// <summary>
    /// Класс-хранилище списков лексем и их классификаций
    /// </summary>
    public static class Classification
    {

        static char[] singleSeparations = { '+', '-', '=', '/', '<', '>', '*', '(', ')', ',' }; // Массив одинарных разделителей
        static string[] doubleSeparations = { ">=", "<=", "<>", "/-", "()"}; // Массив двойных разделителей

        public static List<Lexeme> lexemes = new List<Lexeme>(); // Список лексем
        public static List<string> variables = new List<string>(); // Список переменных
        public static List<string> literals = new List<string>(); // Список литералов
        public static List<Token> tokens = new List<Token>(); // Список токенов

        static List<string> keyWords = new List<string> { "Integer", "Short", "Double", "Dim", "As", "If", "Then", "Else", "End", "Sub", "Main", "And", "Or", "Xor" }; // список ключевый слов
        public static List<string> separators = GetListSeparators(); // Список переменных

        // Поля только для чтения
        public static string[] DoubleSeparations { get => doubleSeparations; }
        public static char[] SingleSeparations { get => singleSeparations; }
        public static List<string> KeyWords { get => keyWords; }

        /// <summary>
        /// Формирование списка разделителей
        /// </summary>
        /// <returns>Список разделителей</returns>
        private static List<string> GetListSeparators()
        {
            List<string> separators = new List<string>();

            for (int i = 0; i < singleSeparations.Length; i++)
            {
                separators.Add(singleSeparations[i].ToString());
            }

            for (int i = 0; i < doubleSeparations.Length; i++)
            {
                separators.Add(doubleSeparations[i]);
            }
            return separators;
        }

        /// <summary>
        /// Метод получения лексемы
        /// </summary>
        /// <param name="token">Токен</param>
        /// <returns>Лексема</returns>
        public static string GetLexeme(Token token) 
        {
            string lexeme = "";

            // Если ключевое слово
            if (token.NumberTable == 1)
            {
                lexeme = keyWords[token.IndexLexeme];
            }
            // Если разделитель
            else if (token.NumberTable == 2)
            {
                lexeme = separators[token.IndexLexeme];
            }
            // Если переменная
            else if (token.NumberTable == 3)
            {
                lexeme = variables[token.IndexLexeme];
            }
            // Если число
            else if (token.NumberTable == 4) 
            {
                lexeme = literals[token.IndexLexeme];
            }
            

            return lexeme;
        }

        /// <summary>
        /// Является ли лексема переменной
        /// </summary>
        /// <param name="lexeme">Лексема</param>
        /// <returns>Является ли лексема переменной</returns>
        public static bool isId(string lexeme) 
        {
            if (variables.Contains(lexeme))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Является ли лексема литералом
        /// </summary>
        /// <param name="lexeme">Лексема</param>
        /// <returns>Является ли лексема литералом</returns>
        public static bool isLiteral(string lexeme)
        {
            if (literals.Contains(lexeme))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Метод очистки списков
        /// </summary>
        public static void Clear() 
        {
            variables.Clear();
            literals.Clear();
            tokens.Clear();
            lexemes.Clear();

        }
    }
}

