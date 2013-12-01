using System;

namespace SorterEvo.Test.StateMonad
{
    public class State<TS, TA>
    {
        public Func<TS, Tuple<TS, TA>> Computation { get; set; } 
    }


    public class Unit { }

    public static class State
    {
        public static State<TS, TS> Get<TS>()
        {
            return new State<TS, TS>
            {
                Computation = sx => Tuple.Create(sx, sx)
            };
        }

        public static State<TS, Unit> Set<TS>(TS state)
        {
            return new State<TS, Unit>
            {
                Computation = sx => Tuple.Create<TS, Unit>(sx, null)
            };
        }

        public static State<TS, TB> Bind<TS, TA, TB>(this State<TS, TA> a, Func<TA, State<TS, TB>> func)
        {
            return new State<TS, TB>
            {
                Computation = x =>
                {
                    var stateContent = a.Computation(x);
                    return func(stateContent.Item2).Computation(stateContent.Item1);
                }
            };
        }

        public static State<TS, TA> ToState<TS, TA>(this TA value)
        {
            return new State<TS, TA>
            {
                Computation = state => Tuple.Create(state, value)
            };
        }

        public static State<int, TC> SelectMany<TA, TB, TC>
            (
                this State<int, TA> a,
                Func<TA, State<int, TB>> func,
                Func<TA, TB, TC> select
            )
        {
            return a.Bind(aVal =>
                    func(aVal).Bind(bVal =>
                        select(aVal, bVal).ToState<int, TC>()));
        }

        public static State<int, short> GetRandom()
        {
            var s = State.Get<int>();
            return s.Bind(s0 =>
            {
                var rand = LcgRandom.NextShort(s0);
                var setState = State.Set(rand.Item2);
                return setState.Bind(_ =>
                        new State<int, short>
                        {
                            Computation = a => Tuple.Create(rand.Item2, rand.Item1)
                        }
                    );

            });
        }

    }
}