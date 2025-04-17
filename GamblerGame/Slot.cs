using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace GamblerGame
{
    enum SymbolName
    {
        Cherry = 10,
        Seven = 100,
        Lemon = 30,
        Lime = 50,
        Grape = 70,
        Pineapple = 60,
        Watermelon = 10,
        Orange = 60,
        Kiwi = 50,
        Apple = 10,
        Strawberry = 10,
        Banana = 30,
    }
    internal class Slot
    {
        // list of symbol textures
        private List<Texture2D> pictures;
        private List<SymbolName> symbols;

        /// <summary>
        /// for getting the result of a roll from the slot 
        /// </summary>
        public SymbolName Result { get; private set; }

        public Slot()
        {
            // pictures will be intialized here from file using the script manager 
        }

        // method to use an item


        // method to roll the slot
        public void Roll(Random rng)
        {
            // if item is being used 
            // then manipulate the indices of the list
            // otherwise, its rolling at even chances
            Result = symbols[rng.Next(1, symbols.Count)];
        }

    }
}
