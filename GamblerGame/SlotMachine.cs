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
        private double multiplierToAdd;
        private double scoreToAdd;

        public double Multiplier { get; set; }
        public double RollTotal { get; private set; }
        public double RollScore { get; private set; }
        public List<double> ScoreList { get; private set; }
        public List<Symbol> SymbolList { get; private set; }
        public List<Slot> SlotList { get; private set; }

        public SlotMachine(ContentManager ct)
        {
            slots = new List<Slot>()
            {
                new Slot(ct),
                new Slot(ct),
                new Slot(ct)
            };
            SlotList = slots;
            ScoreList = new List<double>();
            Multiplier = 1;
            scoreToAdd = 0;
            multiplierToAdd = 0;
        }

        /// <summary>
        /// Calls each of the slot roll methods and then adds up the scores. 
        /// After, it calls check Multiply and handles the round scoring
        /// </summary>
        internal void Roll(Random rng)
        {

            // ScoreList = new List<double>();
            SymbolList = new List<Symbol>();
            rollScore = 0;
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].Roll(rng);
                rollScore += (int)slots[i].ResultName;
                ScoreList.Add((int)slots[i].ResultName);
                SymbolList.Add(slots[i].Result);
            }

            RollTotal = (rollScore + scoreToAdd) * CheckMultiply();
            scoreToAdd = 0;
            multiplierToAdd = 0;
        }

        /// <summary>
        /// Checks the number of matching symbols and returns a value for the score to get multiplied by
        /// Also adds a value if an item is used
        /// </summary>
        /// <returns></returns>
        private double CheckMultiply()
        {
            matchedSymbols = 0;
            Multiplier = 1;

            if (slots[0].ResultName == slots[1].ResultName)
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
                return Multiplier += multiplierToAdd;
            }
            else if (matchedSymbols == 1)
            {
                return Multiplier = 1.5 + multiplierToAdd;
            }
            else if (matchedSymbols == 2 + multiplierToAdd)
            {
                return Multiplier = 2 + multiplierToAdd;
            }
            // This shouldn't happen
            else
            {
                return Multiplier = 50;
            }
        }

        /*
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
        */

        /// <summary>
        /// Rolls a specific symbol in one slot.
        /// </summary>
        /// <param name="action"></param>
        public void RollSpecificSymbol(string action)
        {
            string[] info = action.Split(',');
            slots[int.Parse(info[0])-1].RollSpecificSymbol(info[1]);
        }

        /// <summary>
        /// Increases a chance of a symbol in a slot.
        /// </summary>
        /// <param name="action"></param>
        public void IncreaseSymbolChance(string action)
        {
            string[] info = action.Split(',');
            slots[int.Parse(info[0])-1].IncreaseSymbolChance(info[1], int.Parse(info[2]));
        }

        /// <summary>
        /// Freezes the slot.
        /// </summary>
        /// <param name="action"></param>
        public void FreezeSlot(string action)
        {
            slots[int.Parse(action)-1].Freeze();
        }

        /// <summary>
        /// Resets the slot machine symbols for new runs
        /// </summary>
        public void Reset()
        {
            if (SymbolList != null)
            {
                SymbolList.Clear();
            }
            foreach(Slot slot in slots)
            {
                slot.Reset();
            }
            ScoreList = new List<double>();
            Multiplier = 1;
            scoreToAdd = 0;
            multiplierToAdd = 0;
        }

        /// <summary>
        /// Increases multiplier by multiplying it with an integer parameter
        /// </summary>
        /// <param name="inc"></param>
        public void IncreaseMultipler(int inc)
        {
            multiplierToAdd = Multiplier * ((double)inc /100);
        }

        /// <summary>
        /// Increases the Roll score by adding an integer
        /// </summary>
        /// <param name="inc"></param>
        public void IncreasePoints(int inc)
        {
            scoreToAdd = inc;
        }
    }
}
