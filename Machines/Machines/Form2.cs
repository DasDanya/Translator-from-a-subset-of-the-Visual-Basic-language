using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Machines
{
    public partial class Form2 : Form
    {

        List<Lexeme> lexemes = new List<Lexeme>(); // Список лексем

        List<string> separators = new List<string>(); // Список разделителей
        List<string> keyWords = new List<string>(); // Список ключевых слов
        List<string> variables = new List<string>(); // Список переменных
        List<string> literals = new List<string>(); // Список литералов
        List<Token> tokens = new List<Token>(); // Список токенов
        public Form2(List<Lexeme> lexemes, List<string> separators, List<string> keyWords, List<string> variables, List<string> literals, List<Token> tokens)
        {
            InitializeComponent();

            this.lexemes = lexemes;
            this.separators = separators;
            this.keyWords = keyWords;
            this.variables = variables;
            this.literals = literals;
            this.tokens = tokens;
            
        }


        private void Form2_Load(object sender, EventArgs e)
        {

            OutputLexmes(); // Выводим лексемы
            OutputClassifiedLexemes(keyWords, 1); // Выводим ключевые слова
            OutputClassifiedLexemes(separators, 2);  // Выводим разделители 
            OutputClassifiedLexemes(variables, 3); // Выводим переменные
            OutputClassifiedLexemes(literals, 4); // Выводим литералы

            OutputTokens(); // Выводим токены
        }

        /// <summary>
        /// Выводит лексемы построчно
        /// </summary>
        private void OutputLexmes()
        {
            for (int i = 0; i < lexemes.Count; i++)
            {
                LexemesTreeView.Nodes.Add(lexemes[i].Name + " " + "-" + " " + lexemes[i].Type);
            }
            lexemes.Clear();
        }

        /// <summary>
        /// Вывод токенов построчно
        /// </summary>
        private void OutputTokens() 
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                TokensTreeView.Nodes.Add(tokens[i].NumberTable + " " + ";" + " " + tokens[i].IndexLexeme);
            }
            tokens.Clear();
        }

       /// <summary>
       /// Выводит определенные лексемы
       /// </summary>
       /// <param name="classification">Список определённых лексем</param>
       /// <param name="numberTable">Номер таблицы</param>
        private void OutputClassifiedLexemes(List<string> classification, int numberTable)  
        {
            foreach (var item in classification)
            {
                if (numberTable == 1)
                    KeyWordsTreeView.Nodes.Add(item); // Вывод ключевых слов

                else if (numberTable == 2)
                    SeparatorsTreeView.Nodes.Add(item); // Вывод разделителей

                else if (numberTable == 3)
                    VariablesTreeView.Nodes.Add(item); // Вывод переменных

                else if (numberTable == 4)
                    LiteralsTreeView.Nodes.Add(item); // Вывод литералов
            }

            classification.Clear();
        }
    }
}
