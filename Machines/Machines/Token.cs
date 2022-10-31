using System;
using System.Collections.Generic;
using System.Text;

namespace Machines
{

    /// <summary>
    /// Класс для хранения данных о таблице лексемы и индекса лексемы
    /// </summary>
    public class Token
    {
        int numberTable, indexLexeme;

        /// <summary>
        /// Номер таблицы
        /// </summary>
        public int NumberTable { get { return numberTable; } }
        /// <summary>
        /// Индекс лексемы в таблице
        /// </summary>
        public int IndexLexeme { get { return indexLexeme; } }

        public Token(int numberTable, int indexLexeme)
        {
            this.numberTable = numberTable;
            this.indexLexeme = indexLexeme;
        }

        /// <summary>
        /// Метод для создания токенов, хранящих данные о номере таблицы, в которой находится лексема, и индексе лексемы в таблице
        /// </summary>
        /// <returns>Список токенов</returns>
        public static List<Token> GeneratingListTokens(List<Lexeme> lexemes, List<string> keyWords, List<string> separators, List<string> variables, List<string> literals)
        {
            List<Token> tokens = new List<Token>();

            foreach (var lexeme in lexemes)
            {
                if (lexeme.Type == 'I') // Если лексема является идентификатором 
                {
                    if (keyWords.Contains(lexeme.Name)) // Если лексема является ключевым словом
                    {
                        Token token = new Token(1, keyWords.IndexOf(lexeme.Name)); // Создаём токен для этого ключевого слова
                        tokens.Add(token); // Добавляем токен ключевого слова в список токенов
                    }
                    else if (variables.Contains(lexeme.Name)) // Если лексема является литералом
                    {
                        Token token = new Token(3, variables.IndexOf(lexeme.Name)); // Создаём токен для этого литерала
                        tokens.Add(token); // Добавляем токен литерала в список токенов
                    }
                }
                else if (lexeme.Type == 'D')  // Если лексема является переменной
                {
                    if (literals.Contains(lexeme.Name)) // Проверка на наличие переменной в списке переменных
                    {
                        Token token = new Token(4, literals.IndexOf(lexeme.Name)); // Создаём токен для этой переменной
                        tokens.Add(token); // Добавляем токен переменной в список токенов
                    }
                }
                else if (lexeme.Type == 'R') // Если лексема является разделителем
                {
                    if (separators.Contains(lexeme.Name)) // Проверка на наличие разделителя в списке разделителей
                    {
                        Token token = new Token(2, separators.IndexOf(lexeme.Name)); // Создаём токен для этого разделителя
                        tokens.Add(token); // Добавляем токен разделителя в список токенов
                    }
                }
            }

            return tokens;
        }
    }
}

