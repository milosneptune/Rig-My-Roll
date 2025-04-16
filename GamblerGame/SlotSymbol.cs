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
        
        public SlotSymbol(string name)
        {
            this.name = name;
        }

        internal double GetScore()
        {
            switch (name)
            {
                case "1":
                    return 0;

                case "2":
                    return 1;

                case "3":
                    return 2;

                default:
                    return 50;
            }
        }
    }
}
