using System;

namespace AnkiEditor
{
    /// <summary>
    /// Based on https://github.com/dae/anki/blob/master/anki/utils.py
    /// Used for GUID generation like ANKI
    /// </summary>
    public static class Utils
    {

        private static Random random = new Random();

        private static string BaseX(ulong number, char[] baseTable)
        {
            uint x = (uint)baseTable.Length;

            var num = number;
            var result = "";
            while (num > 0)
            {
                var i = num % x;
                num = num / x;
                result = baseTable[i] + result;
            }

            return result;
        }

        /// <summary>
        /// # all printable characters minus quotes, backslash and separators
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private static string Base91(ulong num)
        {
            char[] table = new char[91];

            int i = 0;

            for (char j = '\x21'; j <= '\x7e'; j++)
            {
                if (j != '\x27' && j != '\x2d' && j != '\x5c')
                    table[i++] = j;
            }

            return BaseX(num, table);
        }

        /// <summary>
        /// "Return a base91-encoded 64bit random number."
        /// </summary>
        /// <returns></returns>
        public static string Guid64()
        {
            byte[] buffer = new byte[8];
            random.NextBytes(buffer);
            var ulongrand = BitConverter.ToUInt64(buffer, 0);

            return Base91(ulongrand);
        }
    }
}
