using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ardoPOParser
{
    public class POParser
    {

        private static Regex lineReader = new Regex(@"^\s*""(([^""]|\\"")*)""\s*$" ,RegexOptions.Compiled);

        public static List<POEntry> Load(StreamReader input)
        {
            List<POEntry> result = new List<POEntry>();
            POEntry entry = null;

            while ((entry = readEntry(input)) != null)
            {
                result.Add(entry);
            }

            return result;
        }


        public static void Write(List<POEntry> pofile, StreamWriter output)
        {
            foreach (POEntry entry in pofile)
            {
                writeEntry(entry, output);
            }
        }


        private static POEntry readEntry(StreamReader input)
        {
            POEntry result = new POEntry();

            string line = input.ReadLine();
            if (line == null)
                return null;

            // ignore initial whitespace
            while (line.Trim() == string.Empty)
            {
                line = input.ReadLine();
                if (line == null)
                    return null;
            }

            // comments
            while (line.StartsWith("#"))
            {
                if (line.StartsWith("# "))
                    result.translator_comments += (result.translator_comments == null ? "":"\n") + line.Substring(2).Trim();
                if (line.StartsWith("#."))
                    result.extracted_comments += (result.extracted_comments == null ? "" : "\n") + line.Substring(2).Trim();
                if (line.StartsWith("#:"))
                    result.reference += (result.reference == null ? "" : "\n") + line.Substring(2).Trim();
                if (line.StartsWith("#|"))
                    result.previous_untranslated += (result.previous_untranslated == null ? "" : "\n") + line.Substring(2).Trim();
                if (line.StartsWith("#,"))
                {
                    foreach (string tag in line.Substring(2).Trim().ToLower().Split(','))
                    {
                        result.Tags.Add(tag.Trim());
                    }
                }

                line = input.ReadLine();
                if (line == null)
                    return null;

                while (line.Trim() == "")
                {
                    line = input.ReadLine();
                    if (line == null)
                        return null;
                }
            }

            // TODO: msgctx
            // TODO: msg_plural
            // expecting msgid
            if (!line.Trim().StartsWith("msgid "))
            {
                // should have my own exception, non?
                throw new Exception("Parser Error");
            }
            else
            {
                line = line.Trim().Substring("msgid ".Length).Trim();
            }

            while (!line.Trim().StartsWith("msgstr "))
            {
                result.msgid += lineReader.Match(line).Groups[1].Value;

                line = input.ReadLine();
                if (line == null)
                    return result;
            }

            // msgstr
            // TODO: plural

            line = line.Substring("msgstr ".Length);

            while (!line.Trim().Equals(string.Empty))
            {
                Match m = lineReader.Match(line);
                if (m.Success)
                    result.msgstr += m.Groups[1].Value;

                line = input.ReadLine();
                if (line == null)
                    return result;
            }

            return result;
        }

        private static void writeEntry(POEntry entry, StreamWriter output)
        {
            if (entry.translator_comments != null)
            {
                foreach (string comment_line in entry.translator_comments.Split('\n'))
                {
                    output.WriteLine("# " + comment_line);
                }
            }

            if (entry.extracted_comments != null)
            {
                foreach (string comment_line in entry.extracted_comments.Split('\n'))
                {
                    output.WriteLine("#. " + comment_line);
                }
            }

            if (entry.reference != null)
            {
                foreach (string comment_line in entry.reference.Split('\n'))
                {
                    output.WriteLine("#: " + comment_line);
                }
            }

            if (entry.Tags.Count > 0)
            {
                output.WriteLine("#, " + String.Join(", ", entry.Tags.ToArray()));
            }

            if (entry.previous_untranslated != null)
            {
                foreach (string comment_line in entry.previous_untranslated.Split('\n'))
                {
                    output.WriteLine("#| " + comment_line);
                }
            }


            if (entry.msgctx != null)
            {
                writeString("msgctx", entry.msgctx, output);
            }


            writeString("msgid", entry.msgid, output);
            writeString("msgstr", entry.msgstr, output);

            output.WriteLine();
        }

        private static void writeString(string section, string value, StreamWriter output)
        {
            output.Write(section + " ");

            if (value.Length < 70)
            {
                output.WriteLine("\"" + value + "\"");
            }
            else
            {
                output.WriteLine("\"\"");
                string current = "";

                foreach (string s in value.Split(' '))
                {
                    if (s.EndsWith("\\n"))
                    {
                        output.WriteLine("\"" + current + " " + s + "\"");
                        current = "";
                    }
                    else if (s.Length + current.Length > 72)
                    {
                        output.WriteLine("\"" + current + " \"");
                        current = s;
                    } 
                    else 
                    {
                        current += ((current == "")?"":" ") + s;
                    }
                }
                if (current != "")
                    output.WriteLine("\"" + current + "\"");
            }
        }
    }
}
