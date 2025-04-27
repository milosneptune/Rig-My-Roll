using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace GamblerGame
{
    enum SymbolName
    {
        Cherries = 10,
        Seven = 100,
        Lemon = 35,
        Lime = 55,
        Pineapple = 60,
        Melon = 25,
        Orange = 65,
        Kiwi = 50,
        Apple = 15,
        Strawberry = 20,
        Bananas = 30,
    }
    internal class Slot
    {

        private List<Symbol> symbols; // the unchanged list of symbols
        private List<Symbol> newSymbols; // this is the list that will change if an item is used
        private ScriptManager symbolFile;

        /// <summary>
        /// for getting the Symbol result of a roll from the slot 
        /// </summary>
        public Symbol Result { get; private set; }
        /// <summary>
        /// For getting the Symbol NAME of the Result
        /// </summary>
        public SymbolName ResultName { get { return Result.Name; } }
        /// <summary>
        /// true if an item has been used, false otherwise
        /// </summary>
        public bool UsedItem { get; set; }
        public List<Symbol> Symbols { get; private set; }
        public bool Frozen { get; private set; }

        /// <summary>
        /// Intializes symbols and new symbols list, 
        /// also calls LoadSymbols method
        /// </summary>
        /// <param name="ct"></param>
        public Slot(ContentManager ct)
        {
            symbols = new List<Symbol>();
            newSymbols = new List<Symbol>();
            Frozen = false;
            UsedItem = false;
            LoadSymbols(ct);
        }

        /// <summary>
        /// Intializes script manager, loads the names from file, and adds the symbols
        /// to the unchanged and changed lists 
        /// </summary>
        /// <param name="ct"></param>
        private void LoadSymbols(ContentManager ct)
        {
            // check the SymbolsFile.txt, the scriptmanager needs
            // to split them in the name and the texture, then the Symbol() class
            // needs to intialize each of them with it's name and appropriate texture, 
            // then it needs to be added to both the symbols list and new symbols
            symbolFile = new ScriptManager("SymbolsFile.txt");

            List<string> names = new List<string>();
            names = symbolFile.GetNames();
            foreach (string name in names)
            {
                Texture2D texture = ct.Load<Texture2D>("UI/Symbols/" + name.ToString());
                symbols.Add(new Symbol(Enum.Parse<SymbolName>(name), texture));
                newSymbols.Add(new Symbol(Enum.Parse<SymbolName>(name), texture));
            }
            Symbols = symbols;
        }

        /// <summary>
        /// Chooses a random index from the symbols list
        /// </summary>
        /// <param name="rng"></param>
        public void Roll(Random rng)
        {
            // if their as item being used, the use item method will manipulate the private list
            // example, an item increases the chance of cherry being rolled, then the use item method 
            // would replace one of the symbol names with cherry
            if (UsedItem)
            {
                Result = newSymbols[rng.Next(1, newSymbols.Count)];
                UsedItem = false; // set it back to false
            }
            if (Frozen) // skips rolling 
            {
                return;
            }
            else
            {
                Result = symbols[rng.Next(1, symbols.Count)];
            }
        }

        /// <summary>
        /// Rolls a specific symbol using an item.
        /// </summary>
        /// <param name="name"></param>
        public void RollSpecificSymbol(string name)
        {
            Result = symbols[symbolFile.FindIndex(name)];
        }

        /// <summary>
        /// Increases the chance of a symbol in this slot.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="chance"></param>
        public void IncreaseSymbolChance(string name, int chance)
        {
            // grab the targeted symbol 
            Symbol target = symbols[symbolFile.FindIndex(name)];

            // loop for however many times the chance is called
            for (int i = 0; i < chance; i++)
            {
                newSymbols.Add(target);
                if (newSymbols[i] != target) // remove other symbols if they aren't the target
                {
                    newSymbols.RemoveAt(i);
                }
            }
            UsedItem = true;
        }

        /// <summary>
        /// Toggles the Frozen bool
        /// </summary>
        public void Freeze()
        {
            Frozen = true; 
        }
    }
}
