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
            Console.WriteLine(Console.Out.Encoding);
            Console.Out.Close();
            StreamWriter console_out = new StreamWriter(Console.OpenStandardOutput());
            
            console_out.AutoFlush = true;
            Console.SetOut(console_out);

            Console.WriteLine(console_out.Encoding);

            using (StreamReader sr = new StreamReader(@"..\..\..\xymon_4.3.17-4_pt.po"))
            {
                var watch = Stopwatch.StartNew();
                List<POEntry> pofile = POParser.Load(sr);
                watch.Stop();

                Console.WriteLine("Loaded in {0} ms", watch.ElapsedMilliseconds);

                POParser.Write(pofile, console_out);


            }

            using (StreamReader sr = new StreamReader(@"..\..\..\pioneers_15.3-1_pt.po"))
            {
                var watch = Stopwatch.StartNew();
                List<POEntry> pofile = POParser.Load(sr);
                watch.Stop();

                Console.WriteLine("Loaded in {0} ms", watch.ElapsedMilliseconds);


                POParser.Write(pofile, console_out);

            }

            console_out.Close();
        }
    }
}
