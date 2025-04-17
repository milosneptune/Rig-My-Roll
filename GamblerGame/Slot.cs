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
        Lemon = 30,
        Lime = 50,
        Grape = 70,
        Pineapple = 60,
        Melon = 10,
        Orange = 60,
        Kiwi = 50,
        Apple = 10,
        Strawberry = 10,
        Bananas = 30,
    }
    internal class Slot
    {
        // list of symbol textures
        //private List<Texture2D> pictures;
        private List<Symbol> symbols; // the unchanged list of symbols
        private List<Symbol> newSymbols; // this is the list that will change if an item is used

        private ScriptManager sm;

        /// <summary>
        /// for getting the result of a roll from the slot 
        /// </summary>
        public Symbol Result { get; private set; }
        public SymbolName ResultName { get { return Result.Name; } }
        

        public Slot(ContentManager ct)
        {
            //symbols = new List<Symbol>();
            //newSymbols = new List<Symbol>();
            sm = new ScriptManager("SymbolsFile");
            LoadSymbols(ct);
        }

        // method to use an item

        // method to load symbolsnames into symbosl
        private void LoadSymbols(ContentManager ct)
        {

            // check the SymbolsFile.txt, the scriptmanager needs
            // to split them in the name and the texture, then the Symbol() class
            // needs to intialize each of them with it's name and appropriate texture, 
            // then it needs to be added to both the symbols list and new symbols

            
            List<string> names = new List<string>();
            names = sm.GetNames();
            foreach (string name in names)
            {
                symbols.Add(new Symbol(Enum.Parse<SymbolName>(name), ct));
                newSymbols.Add(new Symbol(Enum.Parse<SymbolName>(name), ct));
            }

        }

        // method to roll the slot
        public void Roll(Random rng)
        {
            // if their as item being used, the use item method will manipulate the private list
            // example, an item increases the chance of cherry being rolled, then the use item method 
            // would replace one of the symbol names with cherry
            Result = symbols[rng.Next(1, symbols.Count)];
        }

    }
}
