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
        int matchedSymbols;
        Random rng = new Random();

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
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].Roll(rng);
            }
        }

        internal double CheckMultiply()
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
