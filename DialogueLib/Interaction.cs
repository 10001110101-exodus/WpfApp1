using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogueLib
{
    public class Interaction
    {
        private List<DialogueLine> _lines;
        private DialogueLine _line;
        private int _index = 0;
        public string Name { get; }
        public DialogueLine Line { get { return _line; } }

        public Interaction(List<DialogueLine> lines, string name)
        {
            this._lines = lines;
            this.Name = name; 
            this._line = this._lines[0];
        }
        /// <summary>
        /// go to next dialogue line in this interaction 
        /// </summary>
        /// <returns>true if end of dialogue</returns>
        public bool next()
        {
            if (_index < _lines.Count-1)
            {
                _index++;
                _line = this._lines[this._index];
                return false;
            }
            return true;
        }
    }
}
