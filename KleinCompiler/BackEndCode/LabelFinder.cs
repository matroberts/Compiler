using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace KleinCompiler.BackEndCode
{
    public class LabelFinder
    {
        public static Dictionary<string, object> FindLabels(string targetCode)
        {
            // a label in the source code looks like this:
            //     * label:label3
            // an instrunction in the source code looks like this:
            //     17:  etc.
            // must find the label, and then add the first line number after it, and populate the dictionary with these values
            var labelRegex = new Regex(@"\* label:(\w*)\s.*?(\d+):", RegexOptions.Singleline);
            return labelRegex.Matches(targetCode)
                             .Cast<Match>()
                             .ToDictionary(m => m.Groups[1].Value, m => (object)m.Groups[2].Value);
        }
    }
}