using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace GamblerGame
{
    internal class SlotSymbol
    {
        // For differentiation in methods
        string name;
        Texture2D symbol;
        double scoreValue;

        public double ScoreValue { get { return scoreValue; } }
        
        public SlotSymbol(string name)
        {
            this.name = name;
            switch (name)
            {
                case "1":
                    scoreValue = 0;
                    break;

                case "2":
                    scoreValue = 1;
                    break;

                case "3":
                    scoreValue = 2;
                    break;

                default:
                    scoreValue = 50;
                    break;
            }
        }
    }
}
