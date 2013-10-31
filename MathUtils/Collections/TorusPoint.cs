using System.Collections.Generic;

namespace MathUtils.Collections
{
    public struct TorusPoint
    {
        public bool Equals(TorusPoint other)
        {
            return _x == other._x && _y == other._y && _maxX == other._maxX && _maxY == other._maxY;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = _x;
                hashCode = (hashCode*397) ^ _y;
                hashCode = (hashCode*397) ^ _maxX;
                hashCode = (hashCode*397) ^ _maxY;
                return hashCode;
            }
        }

        private readonly int _x;
        private readonly int _y;

        public TorusPoint(int x, int y, int maxX, int maxY)
        {
            _x = x;
            _y = y;
            _maxX = maxX;
            _maxY = maxY;
        }

        public int X
        {
            get { return _x; }
        }

        public int Y
        {
            get { return _y; }
        }

        private readonly int _maxX;
        public int MaxX
        {
            get { return _maxX; }
        }

        private readonly int _maxY;
        public int MaxY
        {
            get { return _maxY; }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TorusPoint && Equals((TorusPoint) obj);
        }

        public IEnumerable<TorusPoint> EightNeighbors
        {
            get
            {
                yield return new TorusPoint((X + MaxX - 1) % MaxX,  Y,                    MaxX, MaxY);
                yield return new TorusPoint((X + MaxX - 1) % MaxX, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
                yield return new TorusPoint( X,                    (Y + MaxY - 1) % MaxY, MaxX, MaxY);
                yield return new TorusPoint((X + 1) % MaxX,        (Y + MaxY - 1) % MaxY, MaxX, MaxY);
                yield return new TorusPoint((X + 1) % MaxX,         Y,                    MaxX, MaxY);
                yield return new TorusPoint((X + 1) % MaxX,        (Y + 1) % MaxY,        MaxX, MaxY);
                yield return new TorusPoint( X,                    (Y + 1) % MaxY,        MaxX, MaxY);
                yield return new TorusPoint((X + MaxX - 1) % MaxX, (Y + 1) % MaxY,        MaxX, MaxY);
            }
        }

        public IEnumerable<TorusPoint> FourNeighbors
        {
            get
            {
                yield return new TorusPoint((X + MaxX - 1) % MaxX, Y,                    MaxX, MaxY);
                yield return new TorusPoint( X,                   (Y + MaxY - 1) % MaxY, MaxX, MaxY);
                yield return new TorusPoint((X + 1) % MaxX,        Y,                    MaxX, MaxY);
                yield return new TorusPoint( X,                   (Y + 1) % MaxY,        MaxX, MaxY);
            }
        }

        //public IEnumerable<TorusPoint> Sprinkle4Neighbors(int randomizer)
        //{
        //    var start = randomizer % 4;

        //    if (start == 0)
        //    {
        //        while (true)
        //        {
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, Y, MaxX, MaxY);
        //            yield return new TorusPoint(X, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, Y, MaxX, MaxY);
        //            yield return new TorusPoint(X, (Y + 1) % MaxY, MaxX, MaxY);
        //        }
        //    }

        //    if (start == 1)
        //    {
        //        while (true)
        //        {
        //            yield return new TorusPoint(X, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, Y, MaxX, MaxY);
        //            yield return new TorusPoint(X, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, Y, MaxX, MaxY);
        //        }
        //    }

        //    if (start == 2)
        //    {
        //        while (true)
        //        {
        //            yield return new TorusPoint((X + 1) % MaxX, Y, MaxX, MaxY);
        //            yield return new TorusPoint(X, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, Y, MaxX, MaxY);
        //            yield return new TorusPoint(X, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //        }
        //    }

        //    if (start == 3)
        //    {
        //        while (true)
        //        {
        //            yield return new TorusPoint(X, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, Y, MaxX, MaxY);
        //            yield return new TorusPoint(X, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, Y, MaxX, MaxY);
        //        }
        //    }
        //}

        //public IEnumerable<TorusPoint> Sprinkle8Neighbors(int randomizer)
        //{
        //    var start = randomizer % 8;

        //    if (start == 0)
        //    {
        //        while (true)
        //        {
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint(X, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, Y, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint(X, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, Y, MaxX, MaxY);
        //        }
        //    }

        //    if (start == 1)
        //    {
        //        while (true)
        //        {
        //            yield return new TorusPoint(X, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, Y, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint(X, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, Y, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //        }
        //    }

        //    if (start == 2)
        //    {
        //        while (true)
        //        {
        //            yield return new TorusPoint((X + 1) % MaxX, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, Y, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint(X, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, Y, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint(X, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //        }
        //    }

        //    if (start == 3)
        //    {
        //        while (true)
        //        {
        //            yield return new TorusPoint((X + 1) % MaxX, Y, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint(X, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, Y, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint(X, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //        }
        //    }

        //    if (start == 4)
        //    {
        //        while (true)
        //        {
        //            yield return new TorusPoint((X + 1) % MaxX, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint(X, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, Y, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint(X, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, Y, MaxX, MaxY);
        //        }
        //    }

        //    if (start == 5)
        //    {
        //        while (true)
        //        {
        //            yield return new TorusPoint(X, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, Y, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint(X, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, Y, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, (Y + 1) % MaxY, MaxX, MaxY);
        //        }
        //    }

        //    if (start == 6)
        //    {
        //        while (true)
        //        {
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, Y, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint(X, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, Y, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint(X, (Y + 1) % MaxY, MaxX, MaxY);
        //        }
        //    }

        //    if (start == 7)
        //    {
        //        while (true)
        //        {
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, Y, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint(X, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, (Y + MaxY - 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, Y, MaxX, MaxY);
        //            yield return new TorusPoint((X + 1) % MaxX, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint(X, (Y + 1) % MaxY, MaxX, MaxY);
        //            yield return new TorusPoint((X + MaxX - 1) % MaxX, (Y + 1) % MaxY, MaxX, MaxY);
        //        }
        //    }
        //}
    }
}
