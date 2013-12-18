using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genomic.Genomes;
using MathUtils.Collections;

namespace Genomic.Table
{
    public interface IDataTable<TR,TC, out TD>
        where TR : IGuid
        where TC : IGuid
    {
        IEnumerable<TR> RowHeaders { get; } 
        IEnumerable<TC> ColumnHeaders { get; }
        TD GetDataValue(TR row, TC column);
    }

    public static class GenomeTable
    {
        public static IDataTable<TR, TC, TD> Make<TR, TC, TD>
            (
                IEnumerable<Tuple<TR, TC, TD>> data
            )
            where TR : IGuid
            where TC : IGuid
        {
            return new DataTableImpl<TR, TC, TD>(data);
        }

        public static string Print<TR, TC, TD>
            (
                this IDataTable<TR, TC, TD> table,
                Func<TR, string> rowLabeler,
                Func<TC, string> columnLabeler,
                Func<TD, string> dataFormater 
            )
            where TR : IGuid
            where TC : IGuid
        {
            var rowHeaders = table.RowHeaders.ToList();
            var columnHeaders = table.ColumnHeaders.ToList();

            var strRet = new StringBuilder();

            strRet.Append(columnHeaders.Aggregate("", (s, r) => s + "\t" + columnLabeler(r)));
            strRet.Append("\n");
            foreach (var rowHeader in rowHeaders)
            {
                strRet.Append(rowLabeler(rowHeader));
                foreach (var columnHeader in columnHeaders)
                {
                    var data = table.GetDataValue(rowHeader, columnHeader);
                    var displayed = (data == null) ? String.Empty : dataFormater(data);
                    strRet.Append("\t" + displayed);
                }
                strRet.Append("\n");
            }

            return strRet.ToString();
        }
    }

    class DataTableImpl<TR, TC, TD> : IDataTable<TR,TC,TD>
        where TR : IGuid
        where TC : IGuid
    {

        public DataTableImpl(IEnumerable<Tuple<TR,TC,TD>> data)
        {
            foreach (var tuple in data)
            {
                if (RowHeaders.All(r => r.Guid != tuple.Item1.Guid))
                {
                    _rowHeaders.Add(tuple.Item1);
                }

                if (ColumnHeaders.All(c => c.Guid != tuple.Item2.Guid))
                {
                    _columnHeaders.Add(tuple.Item2);
                }

                _dataDictionary[tuple.Item1.Guid.Add(tuple.Item2.Guid)] = tuple.Item3;
            }
        }


        private readonly List<TR> _rowHeaders = new List<TR>();
        public IEnumerable<TR> RowHeaders
        {
            get { return _rowHeaders; }
        }

        private readonly List<TC> _columnHeaders =new List<TC>();
        public IEnumerable<TC> ColumnHeaders
        {
            get { return _columnHeaders; }
        }


        readonly Dictionary<Guid, TD> _dataDictionary = new Dictionary<Guid, TD>(); 
        public TD GetDataValue(TR row, TC column)
        {
            return _dataDictionary[row.Guid.Add(column.Guid)];
        }
    }
}
