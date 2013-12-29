using System;
using System.Text;

namespace MathUtils
{
    public static class StringExt
    {
        public static string TabChunk(this string stringIn, int chunkSize)
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < stringIn.Length; i += chunkSize)
            {
                if (i > 0)
                {
                    stringBuilder.Append("\t");
                }
                var adjChunk = Math.Min(chunkSize, stringIn.Length - i);
                stringBuilder.Append(stringIn.Substring(i, adjChunk));
            }
            return stringBuilder.ToString();
        }
    }
}
