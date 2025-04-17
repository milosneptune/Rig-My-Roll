using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamblerGame
{
    internal class Symbol
    {
        private SymbolName name;
        private Texture2D texture;
        //Type underlyingType;

        public SymbolName Name { get { return name; } }

        //public int Value 
        //{ get { Convert.ChangeType(name, underlyingType); } }

        public Symbol(SymbolName name, ContentManager ct)
        {
            this.name = name;
            this.texture = ct.Load<Texture2D>("UI/Symbols/" + name.ToString());
            //underlyingType = Enum.GetUnderlyingType(name.GetType());
        }

        //public int GetValue()
        //{
        //    return Convert.ChangeType(name, underlyingType);
        //}

        public override string ToString()
        {
            return name.ToString();
        }

    }
}
