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
                    result.translator_comments += line.Substring(2).Trim() + "\n";
                if (line.StartsWith("#."))
                    result.extracted_comments += line.Substring(2).Trim() + "\n";
                if (line.StartsWith("#:"))
                    result.reference += line.Substring(2).Trim() + "\n";
                if (line.StartsWith("#|"))
                    result.previous_untranslated += line.Substring(2).Trim() + "\n";
                if (line.StartsWith("#,"))
                {
                    foreach (string tag in line.Substring(2).Trim().ToLower().Split(','))
                    {
                        result.Tags.Add(tag);
                    }
                }

                line = input.ReadLine();
                if (line == null)
                    return null;
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
    }
}
