using System;

namespace SorterEvo.Test.StateMonad
{
    public class LcgRandom
    {

        public static Tuple<short, int> NextShort(int state)
        {
            const int multiplier = 214013;
            const int increment = 25311011;
            const int modulus = int.MaxValue;
            var newState = multiplier*state + increment;
            var rand = (short) ((newState & modulus) >> 16);
            return Tuple.Create(rand, newState);
        }
    }
}