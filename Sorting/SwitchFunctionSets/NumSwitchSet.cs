using System;
using System.Text;
using Roslyn.Scripting.CSharp;
using Sorting.KeyPairs;

namespace Sorting.SwitchFunctionSets
{
    internal class UshortSwitchSet : NumSwitchSet<ushort>
    {
        public UshortSwitchSet(int keyCount)
            : base(keyCount)
        {
            MakeSortedTester();
        }

        void MakeSortedTester()
        {
            var engine = new ScriptEngine();
            var session = engine.CreateSession();

            session.AddReference("System.Core");
            session.AddReference(AppDomain.CurrentDomain.BaseDirectory + "\\MathUtils.dll");
            session.AddReference("Sorting.dll");

            session.Execute("using System;");
            session.Execute("using System.Linq.Expressions;");
            session.Execute("using System.Linq;");
            session.Execute("using Sorting;");
            session.Execute("using Sorting.SwitchFunctionSets;");
            session.Execute("using MathUtils.Interval;");

            if (KeyCount == 16)
            {
                _isSorted = (Func<ushort, bool>)session.Execute("new Func<ushort, bool>(a=> SortedNumberEval.IsSorted(a))");
                return;
            }

            var expr = string.Format("new Func<ushort, bool>(a=> SortedNumberEval.IsSorted((ushort) (a << {0})))",
                16 - KeyCount);

            _isSorted = (Func<ushort, bool>)session.Execute(expr);
        }

        private Func<ushort, bool> _isSorted;
        public override bool IsSorted(ushort item)
        {
            return _isSorted(item);
        }
    }


    internal class UintSwitchSet : NumSwitchSet<uint>
    {
        public UintSwitchSet(int keyCount) : base(keyCount)
        {
           MakeSortedTester();
        }

        void MakeSortedTester()
        {
            if (KeyCount < 16)
            {
                var exprA = string.Format("new Func<uint, bool>(a=> Sorting.SwitchFunctionSets.SortedNumberEval.IsSortedForSmall((a << {0})))", 16 - KeyCount);
                _isSorted = (Func<uint, bool>)Session.Execute(exprA);
                return;
            }

            if (KeyCount == 16)
            {
                _isSorted = (Func<uint, bool>)Session.Execute("new Func<uint, bool>(a=> Sorting.SwitchFunctionSets.SortedNumberEval.IsSortedForSmall(a))");
                return;
            }

            var expr = string.Format("new Func<uint, bool>(a=> Sorting.SwitchFunctionSets.SortedNumberEval.IsSorted((a << {0})))", 32 - KeyCount);
            _isSorted = (Func<uint, bool>)Session.Execute(expr);
        }

        private Func<uint, bool> _isSorted;
        public override bool IsSorted(uint item)
        {
            return _isSorted(item);
        }
    }

    public class UlongSwitchSet : NumSwitchSet<ulong>
    {
        public UlongSwitchSet(int keyCount)
            : base(keyCount)
        {
        }

        public override bool IsSorted(ulong item)
        {
            return SortedNumberEval.IsSorted(item << (64 - KeyCount));
        }
    }

    public abstract class NumSwitchSet<T> : IKeyPairSwitchSet<T>
    {
        static NumSwitchSet()
        {
            var engine = new ScriptEngine();
            _session = engine.CreateSession();

            Session.AddReference("System.Core");
            Session.AddReference(AppDomain.CurrentDomain.BaseDirectory + "\\MathUtils.dll");
            Session.AddReference("Sorting.dll");

            Session.Execute("using System;");
            Session.Execute("using System.Linq.Expressions;");
            Session.Execute("using System.Linq;");
            Session.Execute("using Sorting;");
            Session.Execute("using MathUtils.Interval;");
        }

        private static readonly Roslyn.Scripting.Session _session;
        protected static Roslyn.Scripting.Session Session
        {
            get
            {
                return _session;
            }
        }

        protected NumSwitchSet(int keyCount)
        {
            _keyCount = keyCount;
            _switchFuncs = new Func<T, Tuple<T, bool>>[KeyPairRepository.KeyPairSetSizeForKeyCount(KeyCount)];
            MakeSwitchFuncs();
        }

        public abstract bool IsSorted(T item);

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        public Type SwitchableDataType
        {
            get { return typeof(T); }
        }

        public Func<T, Tuple<T, bool>> SwitchFunction(IKeyPair keyPair)
        {
            return _switchFuncs[keyPair.Index];
        }

        Func<T, Tuple<T, bool>>[] _switchFuncs;

        void MakeSwitchFuncs()
        {
            var combo = new StringBuilder();

            foreach (var keyPair in KeyPairRepository.KeyPairsForKeyCount(KeyCount))
            {
                combo.AppendLine(SwitchMaskDefinition(SwitchableDataType.Name, keyPair));
            }
            Session.Execute(combo.ToString());
            combo.Clear();

            for (var i = 0; i < KeyCount; i++)
            {
                combo.AppendLine(ShiftDefinition(SwitchableDataType.Name, i));
            }
            Session.Execute(combo.ToString());
            combo.Clear();

            combo.AppendLine(String.Format("new Func<{0}, Tuple<{0}, bool>>[] {{", SwitchableDataType.Name));
            foreach (var keyPair in KeyPairRepository.KeyPairsForKeyCount(KeyCount))
            {
                combo.AppendLine((keyPair.Index == 0) ? string.Empty : ",");
                combo.AppendLine(SwitchExpression(SwitchableDataType.Name, keyPair));
            }
            combo.AppendLine("}");

            //System.Diagnostics.Debug.WriteLine(combo.ToString());
            _switchFuncs = (Func<T, Tuple<T, bool>>[])Session.Execute(combo.ToString());
        }

        public static string SwitchMaskDefinition(string dataType, IKeyPair keyPair)
        {
            return string.Format("const {0} {1} = ((({0})1 << {2}) + 1) << {3};", dataType, KeyPairMaskName(keyPair), keyPair.HiKey - keyPair.LowKey, keyPair.LowKey);
        }

        public static string KeyPairMaskName(IKeyPair keyPair)
        {
            return string.Format("{0}{1}To{2}", "Mask", keyPair.LowKey, keyPair.HiKey);
        }

        public static string ShiftDefinition(string dataType, int index)
        {
            return string.Format("const {0} {1} = ({0})1 << {2};", dataType, ShiftName(index), index);
        }

        public static string ShiftName(int index)
        {
            return string.Format("CShift{0}", index);
        }

        //switchExpr = a => ((a & Mask0To1) == CShift0) ? new Tuple<ushort, bool>((ushort) (a^Mask0To1), true) : new Tuple<ushort, bool>(a, false);
        public static string SwitchExpression(string dataType, IKeyPair keyPair)
        {
            return String.Format
            (
                "new Func<{3}, Tuple<{3}, bool>>({0} => (({0} & {1}) == {2}) ? new Tuple<{3}, bool>(({3}) ({0} ^ {1}), true) : new Tuple<{3}, bool>({0}, false))",
                "a",
                KeyPairMaskName(keyPair),
                ShiftName(keyPair.LowKey),
                dataType
            );
        }
    }


}
