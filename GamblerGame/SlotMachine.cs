using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamblerGame
{
    internal class SlotMachine
    {
        List<Slot> slots;
        double roundScore;
        double rollScore;

        int matchedSymbols;
        //Random rng = new Random();

        public double RoundScore { get; private set; }
        public double RollScore { get; private set; }

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
            roundScore = 0;
            rollScore = 0;
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].Roll(rng);
                rollScore += (int)slots[i].ResultName;
            }

            RoundScore = rollScore * CheckMultiply();
        }

        /// <summary>
        /// Checks the number of matching symbols 
        /// and returns a value for the score to get multiplied by
        /// </summary>
        /// <returns></returns>
        private double CheckMultiply()
        {
            matchedSymbols = 0;

            if (slots[0].Result == slots[1].Result)
            {
                matchedSymbols++;
            }
            else if (slots[0].Result == slots[2].Result)
            {
                matchedSymbols++;
            }
            else if (slots[1].Result == slots[2].Result)
            {
                matchedSymbols++;
            }
            
            if (matchedSymbols == 0)
            {
                return 1;
            }
            else if (matchedSymbols == 1)
            {
                return 1.3;
            }
            else if (matchedSymbols == 2)
            {
                return 1.6;
            }
            else if (matchedSymbols == 3)
            {
                return 2;
            }
            // This shouldn't happen
            else
            {
                return 50;
            }
        }

        public void DrawSymbols()
        {

        }

    }
}
