using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                var watch = Stopwatch.StartNew();
                List<POEntry> pofile = POParser.Load(sr);
                watch.Stop();

                Console.WriteLine("Loaded in {0} ms", watch.ElapsedMilliseconds);


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
