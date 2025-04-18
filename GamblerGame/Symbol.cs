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

        public void DrawSymbol(SpriteBatch sb, Vector2 position)
        {
            // TODO: Gabe feel free to change the vector, I hardcoded it for now just to see if it would show up
            // but it would be nice if it could take the vector as a parameter and be called from slot machine
            sb.Draw(texture, position, Color.White);
        }
    }
}
