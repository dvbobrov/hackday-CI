using System.Text.RegularExpressions;

namespace Extensions
{
    static class StringExtensions
    {
        public static bool LooksLikeNamespaceDefinition(this string line)
        {
            return Regex.IsMatch(line, @"\s*namespace\s+\w+");
        }

        public static bool LooksLikeClassDefinition(this string line)
        {
            // [modifiers aren't specified ]
            //  some spaces as \s+
            //  class keyword as class
            //  one space at least before class' name
            //  class' name as \w+
            //  possible spaces as \s*
            //  possible separator for inheritance list as :?
            
            return Regex.IsMatch(line, @"\s*class\s+\w+\s*:?");
        }

        public static bool LooksLikeMethodDefinition(this string line)
        {
            // [modifiers aren't specified ]
            //  return type as \w+) 
            //  some spaces as \s+
            //  foo name as \w+
            //  possible spaces before brace as \s*
            //  opening brace as \( 
            //  any symbols as .* 
            //  closing brace as \)

            return Regex.IsMatch(line, @"\w+\s+\w+\s*\(.*\)") && !line.Contains("await"); 
        }
    }
}