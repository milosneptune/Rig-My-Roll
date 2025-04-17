using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamblerGame
{
    /// <summary>
    /// Takes in the name of the file that needs to be loaded, does not require ""../../../TextData/".
    /// All public methods take in the name of the item. GetNames() returns a list of all the names.
    /// </summary>
    internal class ScriptManager
    {
        // Constants for the symbols usesd for file indication.
        private const char splitContentSymbol = '@';
        private const char priceSymbol = '$';

        private string fileName;
        private StreamReader reader;
        private string[] contents;
        public ScriptManager(string fileName)
        {
            this.fileName = fileName;
            string entireFile = "";
            string line;

            // Attempts to read the file and separate each item with "@".
            try
            {
                reader = new StreamReader($"../../../TextData/{fileName}");
                while ((line = reader.ReadLine()) != null)
                {
                    entireFile += line;
                }

                // Splits the file.
                contents = entireFile.Split(splitContentSymbol);
                reader.Close();
            }
            catch
            { 
                SmartConsole.PrintError($"Unable to load script.{fileName}");
            }
        }

        /// <summary>
        /// Returns all the names of contents.
        /// </summary>
        /// <returns></returns>
        public List<string> GetNames()
        {
            List<string> names = new List<string>();

            foreach (string content in contents)
            {
                string[] info = content.Split('/');

                // Adds the name of each content into the list.
                names.Add(info[0]);

            }
            return names;
        }

        /// <summary>
        /// Returns the description of each item.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetDescription(string name)
        {
            string[] info = GetInfo(name);
            string description = info[1];
            return description;
        }

        /// <summary>
        /// returns the price of the item.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetPrice(string name)
        {
            string[] info = GetInfo(name);
            int price = 0;

            if (int.TryParse(info[2].Trim(priceSymbol), out price))
            {
                return price;
            }
            else
            {
                // Returns 0 to avoid errors.
                SmartConsole.PrintError("Unable to parse price (file format error?)");
                return 0;
            }
        }

        /// <summary>
        /// Checks the item type using the special symbol in index 3 of the content info. 
        /// Useful when determining which type of method to run.
        /// </summary>
        /// <returns></returns>
        public char FindItemType(string name)
        {
            string[] info = GetInfo(name);
            char[] chars = info[3].ToCharArray();

            // Takes the first character in the section.
            char type = chars[0];
            return type;
        }

        /// <summary>
        /// Takes in a name and finds the action of the item (probability, influenced symbol, etc).
        /// Returns a string to avoid parsing error.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetItemAction(string name)
        {
            string[] info = GetInfo(name);

            // Returns the action of the item without the item type symbol.
            return info[3].Trim(FindItemType(name));
        }
        /// <summary>
        /// Finds the index of the content in the file using the name. Returns -1 if unable to find content.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal int FindIndex(string name)
        {
            int index = -1;

            // Goes through each content to find the name.
            if (contents.Length > 0)
            {
                for (int i = 0; i < contents.Length; i++)
                {
                    if (contents[i].Contains(name))
                    {
                        index = i;
                        break;
                    }
                }
            }

            // Print error for troubleshooting.
            if (index == -1)
            {
                SmartConsole.PrintError("Unable to find index (name not found!)");
            }

            return index;
        }

        /// <summary>
        /// Takes in the name of the content and finds index using the FindIndex method. 
        /// Returns an array containing the info of that item.
        /// Format as follows: name, description, price, action.
        /// </summary>
        /// <param name="index"></param>
        internal string[] GetInfo(string name)
        {
            int index = FindIndex(name);
            string[] info = new string[4];

            try
            {
                // Splits the content into each info type.
                info = contents[index].Split('/');
            }
            catch
            {
                SmartConsole.PrintError("Unable to find info (item index out of bounds!)");
            }

            return info;
        }
    }
}
