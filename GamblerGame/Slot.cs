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
            // TODO: since we havent figured out the details of how items will interact
            // with the slots, this method just rolls them at an equal chance for now. 
            int roll = rng.Next(1, 13);

            switch (roll)
            {
                case 1:
                    Result = SymbolName.Cherry;
                    break;
                case 2:
                    Result = SymbolName.Seven;
                    break;
                case 3:
                    Result = SymbolName.Lemon;
                    break;
                case 4:
                    Result = SymbolName.Lime;
                    break;
                case 5:
                    Result = SymbolName.Grape;
                    break;
                case 6:
                    Result = SymbolName.Pineapple;
                    break;
                case 7:
                    Result = SymbolName.Watermelon;
                    break;
                case 8: 
                    Result = SymbolName.Orange;
                    break;
                case 9: 
                    Result = SymbolName.Kiwi;
                    break;
                case 10: 
                    Result = SymbolName.Apple;
                    break;
                case 11: 
                    Result = SymbolName.Strawberry;
                    break;
                case 12: 
                    Result = SymbolName.Banana;
                    break;
            }

        }

    }
}
