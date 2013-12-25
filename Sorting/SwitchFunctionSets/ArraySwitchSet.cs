using System;
using System.Linq;
using System.Text;
using MathUtils.Bits;
using Roslyn.Scripting.CSharp;
using Sorting.KeyPairs;

namespace Sorting.SwitchFunctionSets
{
    internal class BitArraySwitchSet : ArraySwitchSet<bool[]>
    {
        public BitArraySwitchSet(int keyCount) : base(keyCount)
        {
        }

        public override bool IsSorted(bool[] item)
        {
            return item.IsSorted();
        }

        public override string OperatorName
        {
                get { return " && !"; }
        }
    }

    internal class IntArraySwitchSet : ArraySwitchSet<int[]>
    {
        public IntArraySwitchSet(int keyCount)
            : base(keyCount)
        {
        }

        public override bool IsSorted(int[] item)
        {
            return item.IsSorted();
        }

        public override string OperatorName
        {
            get { return " > "; }
        }
    }

    public abstract class ArraySwitchSet<T> : IKeyPairSwitchSet<T>
    {
        static ArraySwitchSet()
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

        protected ArraySwitchSet(int keyCount)
        {
            _keyCount = keyCount;
            _switchFuncs = new  Func<T, Tuple<T, bool>>[KeyPairRepository.KeyPairSetSizeForKeyCount(KeyCount)];
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
            get { return typeof (T); }
        }

        public Func<T, Tuple<T, bool>> SwitchFunction(IKeyPair keyPair)
        {
            return _switchFuncs[keyPair.Index];
        }

        readonly Func<T, Tuple<T, bool>>[] _switchFuncs;
        public abstract string OperatorName
        {
            get;
        }

        private static readonly Roslyn.Scripting.Session _session;
        protected static Roslyn.Scripting.Session Session
        {
            get
            {
                return _session;
            }
        }

        void MakeSwitchFuncs()
        {
            var returnValueType = string.Format("Tuple<{0}, bool>", SwitchableDataType.Name);
           
            foreach (var keyPair in KeyPairRepository.KeyPairsForKeyCount(KeyCount).ToList())
            {
                var strInitializer = string.Format(
                    "new Func<{0}, {1}>(a => (a[{3}]{2}a[{4}]) ? new {1}({5}, true) : new {1}(a, false))",
                     SwitchableDataType.Name, 
                     returnValueType,
                     OperatorName,
                     keyPair.LowKey, 
                     keyPair.HiKey, 
                     ArrayBuilder("a", keyPair, KeyCount)
                     );

                _switchFuncs[keyPair.Index] = (Func<T, Tuple<T, bool>>)Session.Execute(strInitializer);
            }
        }

        public static string ArrayBuilder(string arrayName, IKeyPair keyPair, int keyCount)
        {
            var stringBuilder = new StringBuilder("new[] { ");

            for (var i = 0; i < keyPair.LowKey; i++)
            {
                stringBuilder.Append(arrayName + "[" + i + "], ");
            }

            stringBuilder.Append(arrayName + "[" + keyPair.HiKey + "], ");

            for (var i = keyPair.LowKey + 1; i < keyPair.HiKey; i++)
            {
                stringBuilder.Append(arrayName + "[" + i + "], ");
            }

            stringBuilder.Append((keyPair.HiKey == keyCount - 1) ? arrayName + "[" + keyPair.LowKey + "] " : arrayName + "[" + keyPair.LowKey + "], ");

            for (var i = keyPair.HiKey + 1; i < keyCount; i++)
            {
                stringBuilder.Append(arrayName + "[" + i + "]" + ((i == keyCount - 1) ? "" : ", "));
            }

            return stringBuilder.Append(" }").ToString();
        }
    }
}
