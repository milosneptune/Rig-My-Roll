using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GamblerGame
{
    internal class SlotMachine
    {

        private List<Slot> slots;
        private double rollScore;
        private int matchedSymbols;
        //private Random rng = new Random();

        public double RollTotal { get; private set; }
        public double RollScore { get; private set; }
        public List<double> ScoreList { get; private set; }
        public List<Symbol> SymbolList { get; private set; }

        public SlotMachine(ContentManager ct)
        {
            slots = new List<Slot>()
            {
                new Slot(ct),
                new Slot(ct),
                new Slot(ct)
            };
        }


        /// <summary>
        /// Calls each of the slot roll methods and then adds up the scores. 
        /// After, it calls check Multiply and handles the round scoring
        /// </summary>
        internal void Roll(Random rng)
        {
            ScoreList = new List<double>();
            SymbolList = new List<Symbol>();
            rollScore = 0;
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].Roll(rng);
                rollScore += (int)slots[i].ResultName;
                ScoreList.Add((int)slots[i].ResultName);
                SymbolList.Add(slots[i].Result);
            }

            RollTotal = rollScore * CheckMultiply();
        }

        /// <summary>
        /// Checks the number of matching symbols 
        /// and returns a value for the score to get multiplied by
        /// </summary>
        /// <returns></returns>
        private double CheckMultiply()
        {
            matchedSymbols = 0;

            if (slots[0].ResultName.ToString() == slots[1].ResultName.ToString())
            {
                matchedSymbols++;
                if (slots[0].ResultName.ToString() == slots[2].ResultName.ToString())
                {
                    matchedSymbols++;
                }
            }
            else if (slots[0].ResultName.ToString() == slots[2].ResultName.ToString())
            {
                matchedSymbols++;
            }
            else if (slots[1].ResultName.ToString() == slots[2].ResultName.ToString())

            {
                matchedSymbols++;
            }

            if (matchedSymbols == 0)
            {
                return 1;
            }
            else if (matchedSymbols == 1)
            {
                return 1.5;
            }
            else if (matchedSymbols == 2)
            {
                return 2;
            }
            // This shouldn't happen
            else
            {
                return 50;
            }
        }

        /// <summary>
        /// draws RESULTS of a roll for now
        /// </summary>
        public void DrawSymbols(SpriteBatch sb)
        {
        //    Vector2 position = new Vector2(300, 300);
            int x = 300;
            int y = 300;

            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].Result.DrawSymbol(sb, new Vector2(x + i*10, y));
            }
        }
    }
}
