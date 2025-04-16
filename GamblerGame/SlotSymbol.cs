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
        internal double scoreValue;

        public SlotSymbol(string name)
        {
            this.name = name;
        }
    }
}
