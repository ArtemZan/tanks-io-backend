using System;

namespace TanksIO.Game
{
    class Utilities
    {
        public delegate bool Validator(string id);

        public static string GenId(Validator isValid)
        {
            const string validSymbols = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            const int length = 6;

            string id;

            do
            {
                id = "";
                for (int c = 0; c < length; c++)
                {
                    id += validSymbols[(int)System.Math.Floor(new Random().NextDouble() * validSymbols.Length)];
                }
            }
            while (!isValid(id));

            return id;
        }
    }
}
