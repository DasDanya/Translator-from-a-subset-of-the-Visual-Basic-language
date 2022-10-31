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

        static char[] singleSeparations = { '+', '-', '=', '/', '<', '>', '*', '(', ')' }; // Массив одинарных разделителей
        static string[] doubleSeparations = { "<>", "/-" }; // Массив двойных разделителей

        public static List<Lexeme> lexemes = new List<Lexeme>(); // Список лексем
        public static List<string> variables = new List<string>(); // Список переменных
        public static List<string> literals = new List<string>(); // Список литералов
        public static List<Token> tokens = new List<Token>(); // Список токенов

        static List<string> keyWords = new List<string> { "Dim", "Integer", "Boolean", "Double", "As", "If", "Then", "Else", "End", "Not", "And", "Or", "Xor" }; // список ключевый слов
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
    }
}

