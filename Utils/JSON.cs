using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;

namespace TanksIO.Utils
{
    class JSON
    {
        private Dictionary<string, string> _fields = new();
        private bool _isArray = false;
        private Mutex _mutex = new Mutex();

        private static string Stringify(double number) => number.ToString().Replace(',', '.');

        private static string Stringify(float number) => number.ToString().Replace(',', '.');

        public JSON Set(string key, object value, bool wrapValueInQuotes = false)
        {
            if (!_fields.TryGetValue(key, out _))
            {
                _isArray = false;
            }

            string res = value.ToString();

            if (wrapValueInQuotes)
            {
                res = '"' + res + '"';
            }

            _fields.Add(key, res);
            return this;
        }

        public JSON Set(string key, double value) => Set(key, Stringify(value));
        public JSON Set(string key, float value) => Set(key, Stringify(value));

        public JSON Push(object value, bool wrapValueInQuotes = false)
        {
            if (_fields.Count != 0 && !_isArray)
            {
                throw new Exception("Trying to call 'Push' on a JSON object that is not array");
            }

            Set(_fields.Count.ToString(), value, wrapValueInQuotes);

            _isArray = true;
            return this;
        }

        public JSON Push(double value) => Push(Stringify(value));
        public JSON Push(float value) => Push(Stringify(value));

        public JSON PushAll<T>(IEnumerable<T> values, bool wrapValuesInQuotes = false)
        {
            if(values == null)
            {
                return this;
            }

            lock (values)
            {
                for(int i = 0; i < values.Count(); i++)
                {
                    Push(values.ElementAt(i), wrapValuesInQuotes);
                }
            }

            return this;
        }

        public JSON Merge(JSON json)
        {
            if (_isArray || json._isArray)
            {
                Console.WriteLine("Connot merge JSON arrays! (Maybe you wanted to call PushAll ?)");
                return this;
            }

            foreach ((string key, string value) in json._fields)
            {
                _fields.Add(key, value);
            }

            return this;
        }

        public JSON Clear()
        {
            _fields.Clear();
            _isArray = false;
            return this;
        }

        public override string ToString()
        {
            string res = "";

            if (_isArray)
            {
                res += "[";

                foreach (string value in _fields.Values)
                {
                    res += value + ",";
                }

                return res.Remove(res.Length - 1) + ']';
            }

            res += '{';

            foreach ((string key, string value) in _fields)
            {
                res += '"' + key + "\":" + value + ',';
            }

            return res.Remove(res.Length - 1) + '}';
        }
    }
}