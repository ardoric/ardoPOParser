using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ardoPOParser
{
    public class POEntry
    {
        private HashSet<string> _tags;

        public POEntry()
        {
            _tags = new HashSet<string>();
        }

        public string msgid { get; set; }
        public string msgstr { get; set; }

        public HashSet<string> Tags 
        {
            get {
                return _tags; 
            }  
        }

        public string msgctx { get; set; }

        public string translator_comments { get; set; }
        public string extracted_comments { get; set; }
        public string reference { get; set; }
        public string previous_untranslated { get; set; }


        // TODO plurals

        public bool Fuzzy {
            get
            {
                return _tags.Contains("fuzzy");
            }

            set
            {
                if (value)
                    _tags.Add("fuzzy");
                else
                    _tags.Remove("fuzzy");
            }
        }

    }
}
