using System;
using System.Linq;

namespace Sorting.KeyPairs
{

    //class KeyPair : IKeyPair
    //{
    //    public KeyPair(int index, int lowKey, int hiKey)
    //    {
    //        _index = index;
    //        _lowKey = lowKey;
    //        _hiKey = hiKey;
    //    }

    //    private readonly int _lowKey;
    //    public int LowKey
    //    {
    //        get { return _lowKey; }
    //    }

    //    private readonly int _hiKey;
    //    public int HiKey
    //    {
    //        get { return _hiKey; }
    //    }

    //    private readonly int _index;
    //    public int Index
    //    {
    //        get { return _index; }
    //    }

    //    public override string ToString()
    //    {
    //        return String.Format("[{0},{1},{2}]", Index, LowKey, HiKey);
    //    }

    //    public static KeyPair Parse(string strVal)
    //    {
    //        try
    //        {
    //            var pcs = strVal
    //                .Replace("[", string.Empty)
    //                .Replace("]", string.Empty)
    //                .Split(",".ToCharArray()[0])
    //                .Select(int.Parse)
    //                .ToArray();

    //            return new KeyPair(pcs[0], pcs[1], pcs[2]);
    //        }
    //        catch (Exception)
    //        {
    //            throw new Exception("Error parsing kepair: " + strVal);
    //        }
    //    }
    //}
}
