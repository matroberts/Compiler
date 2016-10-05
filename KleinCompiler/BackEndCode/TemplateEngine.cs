using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace KleinCompiler.BackEndCode
{
    // based on http://stackoverflow.com/a/34679657
    public class TemplateEngine
    {
        private static readonly Regex Regex = new Regex(@"\[(\w*?)]");

        public static string Render(string templateString, Dictionary<string, object> parameters)
        {
            return Regex.Replace(templateString, match =>
            {
                var param = match.Groups[1].Value;
                if (parameters.ContainsKey(param) == false)
                    throw new KeyNotFoundException($"TemplateEngine could not find parameter '{param}' in dictionary");
                return parameters[param].ToString();
            });
        }
    }
}