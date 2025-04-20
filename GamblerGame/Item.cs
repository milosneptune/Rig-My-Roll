using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GamblerGame
{
    internal class Item : Button
    {
        private ScriptManager itemsFile = new ScriptManager("ItemsFile.txt");
        private string name;
        private string description;
        private int price;
        private char type;
        private string action;

        public string Name { get { return name; } }
        public string Description { get { return description; } }
        public int Price { get { return price; } }
        public char Type { get { return type; } }
        private string Action { get { return action; } }

        public Item(int itemIndex, GraphicsDevice device, Rectangle position, String text, SpriteFont font, Color color, List<Texture2D> textures)
            : base(device, position, text, font, color, textures)
        {
            name = itemsFile.GetName(itemIndex);
            description = itemsFile.GetDescription(name);
            price = itemsFile.GetPrice(name);
            type = itemsFile.FindItemType(name);
            action = itemsFile.GetItemAction(name);
        }

    }
}
