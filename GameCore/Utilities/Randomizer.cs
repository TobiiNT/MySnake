using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

namespace GameCore.Utilities
{
    public static class Randomizer
    {
        private static DateTime RandomCreateTime = DateTime.Now;
        private static Random _Randomizer;

        public static Random NewRandom
        {
            get
            {
                if (_Randomizer == null || DateTime.Now.Subtract(RandomCreateTime).TotalMilliseconds > 100)
                {
                    RandomCreateTime = DateTime.Now;

                    byte[] data = new byte[4];
                    new RNGCryptoServiceProvider().GetBytes(data);
                    _Randomizer = new Random(BitConverter.ToInt32(data, 0));
                }
                return _Randomizer;
            }
        }

        public static bool NextBoolean() => NewRandom.Next(0, 2) == 0;
        public static double NextDouble() => NewRandom.NextDouble();
        public static double NextPercentage() => NewRandom.NextDouble() * 100;
        public static double Next(double MinValue, double MaxValue) => NewRandom.NextDouble() * (MaxValue - MinValue) + MinValue;
        public static double Next(double MaxValue) => NewRandom.NextDouble() * MaxValue;
        public static int Next(int MinValue, int MaxValue) => NewRandom.Next(MinValue, MaxValue + 1);
        public static int Next(int MaxValue) => Next(0, MaxValue + 1);
        public static double NextDelta(double Value, double Delta) => Next(Value * (1 - Delta), Value * (1 + Delta));
        public static double NextRandomAngle() => NewRandom.NextDouble() * Math.PI * 2;

        public static int GetRandomNumberByValueRate(int FromNumber, int ToNumber)
        {
            int MaxRate = (FromNumber + ToNumber) * (ToNumber + 1 - FromNumber) / 2;
            int CurrentRate = Next(MaxRate);

            int TotalCount = 0;
            for (int Result = FromNumber; Result <= ToNumber; Result++)
            {
                TotalCount += ((ToNumber + FromNumber) - Result);
                if (TotalCount >= CurrentRate)
                    return Result;
            }
            return 0;
        }
        public static T GetRandomObjectWithRate<T>(Dictionary<object, double> KeyPair)
        {
            double RandomNumber = Next(KeyPair.Values.Sum(i => i) * 100);
            double CurrentNumber = 0;
            foreach (var Pair in KeyPair)
            {
                CurrentNumber += Pair.Value * 100;
                if (RandomNumber <= CurrentNumber)
                    return (T)Convert.ChangeType(Pair.Key, typeof(T));
            }
            return default;
        }
        public static T GetRandomObject<T>(List<T> Objects)
        {
            object RandomObject = Objects.OrderBy(x => Guid.NewGuid()).FirstOrDefault();

            return (T)Convert.ChangeType(RandomObject, typeof(T));
        }
        public static T GetRandomObjectInEnums<T>(Type Type)
        {
            PropertyInfo[] Properties = Type.GetProperties();

            return (T)Properties[Next(Properties.Length - 2)].GetValue(null, null);
        }
    }
}
