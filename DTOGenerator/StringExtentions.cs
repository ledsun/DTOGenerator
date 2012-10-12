using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTOGenerator
{
    static class StringExtentions
    {
        public static string SnakeCaseToCamelCase(this string input)
        {
            return string.Join(
                "",
                input
                    .Split('_')
                    .Select(m => m.Substring(0, 1) + m.Substring(1).ToLower())
          );
        }
    }
}
