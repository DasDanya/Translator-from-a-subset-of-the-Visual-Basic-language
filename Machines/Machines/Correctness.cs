using System;
using System.Linq;
using System.Windows.Forms;

namespace Machines
{
    /// <summary>
    /// Класс, содержащий методы для проверок на корректность
    /// </summary>
   public static class Correctness
    {

        /// <summary>
        /// Проверяет превышена ли длина строки заданному значению
        /// </summary>
        /// <param name="str">Проверяемая строка</param>
        /// <returns>Превышено ли длина строки</returns>
        public static bool IsErrorInLengthOfString(string str)
        {

            if (str.Length <= 8)
                return false;
            else
            {
                MessageBox.Show($"Превышена максимальная длина у идентификатора {str}. Максимальная длина - 8 символов", "Лексический анализ", MessageBoxButtons.OK, MessageBoxIcon.Error);                
                return true;
            }

        }

        /// <summary>
        /// Проверяет превышено ли значение числа max значению int
        /// </summary>
        /// <param name="str">Проверяемое число</param>
        /// <returns>Превышено ли значение числа</returns>
        public static bool IsErrorInValueOfNumber(string str)
        {
            try
            {
                if (int.Parse(str) <= int.MaxValue)
                    return false;
                else
                    return true;
            }
            catch 
            {
                throw new Exception($"Число превысело максимальное значение: {int.MaxValue}");
            }

        }

        /// <summary>
        /// Проверка символа на его наличие в английском алфавите
        /// </summary>
        /// <param name="symbol">Проверяемый символ</param>
        /// <returns>Есть ли символ в английском алфавите </returns>
        public static bool CharIsEnglishChar(char symbol)
        {
            char[] letters = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            if (letters.Contains(symbol))
                return true;
            else
                return false;

        }
    }
}
