using System.Collections.Generic;
using System.Globalization;

namespace Digimon_Project.Utils
{
    public static class StringHex
    {
        public static byte[] Hex2Binary(string hex)
        {
            hex = hex.Replace(" ", "");
            var chars = hex.ToCharArray();
            var bytes = new List<byte>();
            for (int index = 0; index < chars.Length; index += 2)
            {
                var chunk = new string(chars, index, 2);
                bytes.Add(byte.Parse(chunk, NumberStyles.AllowHexSpecifier));
            }
            return bytes.ToArray();
        }

        public static byte Hex2Binary(char hex)
        {
            var chars = hex;
            var bytes = new byte();

            bytes = byte.Parse(chars.ToString(), NumberStyles.AllowHexSpecifier);

            return bytes;
        }
    }
}
