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
        Random rng = new Random();

        public double RoundScore { get; private set; }
        public double RollScore { get; private set; }

        public SlotMachine()
        {
            slots = new List<Slot>()
            { 
                new Slot(),
                new Slot(),
                new Slot()
            };
        }



        internal void Roll()
        {
            roundScore = 0;
            rollScore = 0;
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].Roll(rng);
                rollScore += (int)slots[i].Result;
            }

            RoundScore = rollScore * CheckMultiply();
        }

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
            //This shouldn't happen
            else
            {
                return 50;
            }
        }


    }
}
