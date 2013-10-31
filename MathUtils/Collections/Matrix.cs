using System;
using System.Collections.Generic;
using System.Linq;

namespace MathUtils.Collections
{
    public interface IMatrix
    {
        int ColumnCount { get; }
        int RowCount { get; }
        IEnumerable<IEnumerable<object>> Rows { get; } 
    }

    public interface IMatrix<out T> : IMatrix
    {
        new IEnumerable<IEnumerable<T>> Rows { get; } 
    }

    public static class Matrix
    {
        public static IMatrix<T> Make<T>(IEnumerable<IEnumerable<T>> rows, int columnCount) where T : class
        {
            return new Matrix<T>(rows, columnCount);
        }

        public static IMatrix Make(IEnumerable<IEnumerable<object>> rows, int columnCount)
        {
            return new Matrix<object>(rows, columnCount);
        }
    }

    public class Matrix<T> : IMatrix<T>, IMatrix where T : class
    {
        public Matrix(IEnumerable<IEnumerable<T>> rows, int columnCount)
        {
            _columnCount = columnCount;

            foreach (var newList in rows.Select(row => row.ToList()))
            {
                if (newList.Count != ColumnCount)
                {
                    throw new ArgumentException("ColumnCount is not correct");
                }
                _rows.Add(newList);
            }
        }

        private readonly int _columnCount;
        public int ColumnCount
        {
            get { return _columnCount; }
        }

        public int RowCount
        {
            get { return _rows.Count; }
        }

        IEnumerable<IEnumerable<object>> IMatrix.Rows
        {
            get { return _rows; }
        }

        private readonly List<List<T>> _rows = new List<List<T>>();
        public IEnumerable<IEnumerable<T>> Rows
        {
            get { return _rows; }
        }
    }

    public class TestStringMatrix : Matrix<string>
    {
        public TestStringMatrix()
            : base(StringData, ScolumnCount)
        {
        }

        public static List<List<string>> StringData
        {
            get
            {
                var retList = new List<List<string>>();
                
                for (var i = 0; i < SrowCount; i++)
                {
                    var curList = new List<string>();
                    for (int j = 0; j < ScolumnCount; j++)
                    {
                        curList.Add("value_" + i + "_" + j);
                    }

                    retList.Add(curList);
                }

                return retList;
            }
        }

        public static int ScolumnCount { get { return 10; } }
        public static int SrowCount { get { return 10; } }
    }
}
