using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace KleinCompiler.BackEndCode
{
    // based on http://stackoverflow.com/a/34679657
    public class TemplateEngine
    {
        private static readonly Regex FunctionRegex = new Regex(@"\[func:(\w*?)]");

        public static string RenderFunctions(string templateString, Dictionary<string, object> parameters)
        {
            return FunctionRegex.Replace(templateString, match =>
            {
                var param = match.Groups[1].Value;
                if (parameters.ContainsKey(param) == false)
                    throw new KeyNotFoundException($"RenderFunctions could not find parameter '{param}' in dictionary");
                return parameters[param].ToString();
            });
        }

        private static readonly Regex LabelRegex = new Regex(@"\[label:(\w*?)]");

        public static string RenderLabels(string templateString, Dictionary<string, object> parameters)
        {
            return LabelRegex.Replace(templateString, match =>
            {
                var param = match.Groups[1].Value;
                if (parameters.ContainsKey(param) == false)
                    throw new KeyNotFoundException($"RenderLabels could not find parameter '{param}' in dictionary");
                return parameters[param].ToString();
            });
        }
    }
}