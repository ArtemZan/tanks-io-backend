using System;

namespace TanksIO.Utils
{
    class IdGenerator
    {
        public enum IdType
        {
            ALPHABETIC,
            NUMRICAL,
            ALPHA_NUM,
            ALL_CHARS
        }

        public static string Random(int length, string symbols)
        {
            string res = "";

            for(int s = 0; s < length; s++)
            {
                res += symbols[new Random().Next(0, symbols.Length)];
            }

            return res;
        }

        public static string Random(int length, IdType type)
        {
            switch (type)
            {
                case IdType.ALPHABETIC:
                    {
                        return Random(length, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
                    }
                case IdType.NUMRICAL:
                    {
                        return Random(length, "0123456789");
                    }
                case IdType.ALPHA_NUM:
                    {
                        return Random(length, "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
                    }
            }

            return "";
        }
    }
}