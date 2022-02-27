using System;

namespace TanksIO.Utils
{
    class IdGenerator
    {
        private enum IdType
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
                        Random(length, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
                        break;
                    }
                case IdType.NUMRICAL:
                    {
                        Random(length, "0123456789");
                        break;
                    }
                case IdType.ALPHA_NUM:
                    {
                        Random(length, "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
                        break;
                    }
                    case 
            }
        }
    }
}