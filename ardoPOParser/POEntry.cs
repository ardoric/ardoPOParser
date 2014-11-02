using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ardoPOParser
{
    public class POEntry
    {

        public string msgid { get; set; }
        public string msgstr { get; set; }

        // TODO plurals and stuff
        public bool Fuzzy { get; set; }
    }
}
