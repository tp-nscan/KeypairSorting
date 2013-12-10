using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MathUtils.Collections;

namespace MathUtils.Rand
{
    public interface IRandy
    {
        Guid Guid { get; }
        IRando Rando { get; }
    }

    public static class Randy
    {
        public static IRandy Make(Guid guid)
        {
            return new RandyImpl(guid, Enumerable.Empty<IRandy>());
        }

        public static IRandy Make(Guid guid, IEnumerable<IRandy> randys)
        {
            return new RandyImpl(guid, randys);
        }
    }

    class RandyImpl : IRandy
    {
        public RandyImpl(Guid guid, IEnumerable<IRandy> randys)
        {
            _guid = guid;
            _rando = Rand.Rando.Fast(Guid.ToHash());
            _randies = randys.ToDictionary(r => r.Guid);
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        private readonly IRando _rando;
        public IRando Rando
        {
            get { return _rando; }
        }

        private readonly IDictionary<Guid, IRandy> _randies;
        private IDictionary<Guid, IRandy> Randies
        {
            get { return _randies; }
        }

        public IRandy MakeRandy(Guid guid)
        {
            if (Randies.ContainsKey(guid))
            {
                return _randies[guid];
            }
            return Randy.Make(guid.Add(Guid));
        }
    }

}
