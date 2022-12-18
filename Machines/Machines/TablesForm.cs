using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Machines
{
    public partial class TablesForm : Form
    {
        
        public TablesForm()
        {  
            InitializeComponent();
        }


        private void TablesForm_Load(object sender, EventArgs e) 
        {

            ClearTreeViews(); // Очищаем данные со старыми данными

            OutputLexmes(); // Выводим лексемы
            OutputClassifiedLexemes(Classification.KeyWords, 1); // Выводим ключевые слова
            OutputClassifiedLexemes(Classification.separators, 2);  // Выводим разделители 
            OutputClassifiedLexemes(Classification.variables, 3); // Выводим переменные
            OutputClassifiedLexemes(Classification.literals, 4); // Выводим литералы

            OutputTokens(); // Выводим токены

        }

        //private void Form2_Load(object sender, EventArgs e)
        //{

        //    ClearTreeViews(); // Очищаем данные со старыми данными

        //    OutputLexmes(); // Выводим лексемы
        //    OutputClassifiedLexemes(Classification.KeyWords, 1); // Выводим ключевые слова
        //    OutputClassifiedLexemes(Classification.separators, 2);  // Выводим разделители 
        //    OutputClassifiedLexemes(Classification.variables, 3); // Выводим переменные
        //    OutputClassifiedLexemes(Classification.literals, 4); // Выводим литералы

        //    OutputTokens(); // Выводим токены

        //    // Очищаем списки переменных и литералов
        //    Classification.variables.Clear();
        //    Classification.literals.Clear();

        //}

        /// <summary>
        /// Вывод лексем построчно
        /// </summary>
        private void OutputLexmes()
        {
            for (int i = 0; i < Classification.lexemes.Count; i++)
            {
                LexemesTreeView.Nodes.Add(Classification.lexemes[i].Name + " " + "-" + " " + Classification.lexemes[i].Type);
            }
        }

        /// <summary>
        /// Вывод токенов построчно
        /// </summary>
        private void OutputTokens()
        {
            for (int i = 0; i < Classification.tokens.Count; i++)
            {
                TokensTreeView.Nodes.Add(Classification.tokens[i].NumberTable + " " + ";" + " " + Classification.tokens[i].IndexLexeme);
            }
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


        }

        /// <summary>
        /// Метод очистки таблиц с данными 
        /// </summary>
        private void ClearTreeViews()
        {
            LexemesTreeView.Nodes.Clear();
            KeyWordsTreeView.Nodes.Clear();
            SeparatorsTreeView.Nodes.Clear();
            VariablesTreeView.Nodes.Clear();
            LiteralsTreeView.Nodes.Clear();
            TokensTreeView.Nodes.Clear();
        }

        private void TablesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Очищаем списки
            Classification.Clear();
        }
    }
}
