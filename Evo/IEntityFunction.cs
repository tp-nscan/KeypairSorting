using System;

namespace Evo
{
    public interface IEntityFunction<TS, out T>
        where T : class 
    {
        string Key { get; }
        Func<TS, T> Function { get; }
    }
}