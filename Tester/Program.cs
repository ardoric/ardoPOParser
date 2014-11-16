using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ardoPOParser;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader sr = new StreamReader(@"..\..\..\xymon_4.3.17-4_pt.po"))
            {
                List<POEntry> pofile = POParser.Load(sr);

                foreach (var entry in pofile)
                {
                    Console.WriteLine(entry.msgid);
                    Console.WriteLine();
                    Console.WriteLine(entry.msgstr);
                    Console.WriteLine();
                }

            }
        }
    }
}
