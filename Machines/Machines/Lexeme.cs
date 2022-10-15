
namespace Machines
{
    /// <summary>
    /// Класс, получающий и выводящий данные о лексеме
    /// </summary>
    public class Lexeme
    {
        private string name; // лексема
        private char type; // тип лексемы

        public string Name { get { return name; } } 
        public char Type { get { return type; } } 

        public Lexeme(string name, char type) 
        {
            this.name = name;
            this.type = type;
        
        }

    }
}
