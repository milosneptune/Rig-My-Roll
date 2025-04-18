using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GamblerGame
{
    internal class Symbol
    {
        private SymbolName name;
        private Texture2D texture;

        public SymbolName Name { get { return name; } }

        public Symbol(SymbolName name, ContentManager ct)
        {
            this.name = name;
            this.texture = ct.Load<Texture2D>("UI/Symbols/" + name.ToString());
        }

        public override string ToString()
        {
            return name.ToString();
        }

        public void DrawSymbol(SpriteBatch sb)
        {
            sb.Draw(texture, new Vector2(300, 300), Color.White);
        }
    }
}
