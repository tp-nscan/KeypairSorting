using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Newtonsoft.Json;
using SorterEvo.Evals;
using SorterEvo.Genomes;
using SorterEvo.Json.Genomes;
using Sorting.CompetePools;
using Sorting.Sorters;
using WpfUtils;

namespace KeypairSorting.ViewModels.Parts
{
    public interface ISorterGenomeEvalVm
    {
        string SorterGenomeJson { get; }
        bool Success { get; }
        int SwitchUseCount { get; }
        string Ancestors { get; }
        string AncestorsJson { get; }
        Guid SwitchableGroupGuid { get; }
        string SwitchUseList { get; }
        ISorterGenomeEval GetSorterGenomeEval();
    }

    public static class SorterGenomeEvalVm
    {
        public static ISorterGenomeEvalVm ToSorterGenomeEvalVm(this ISorterGenomeEval sorterGenomeEval)
        {
            return new SorterGenomeEvalVmImpl(sorterGenomeEval);
        }

        public static ISorterGenomeEvalVm ToSorterGenomeEvalVm(this string reportString)
        {
            var pcs = reportString.Split("\t".ToCharArray());
            ISorterEval sorterEval;
            ISorterGenome sorterGenome;
            ISorter sorter;
            var switchableGuid = Guid.Empty;
            var success = false;
            var useCount = 0;
            var generation = 0;
            var ancestors = ImmutableStack<Guid>.Empty;

            if (pcs.Count() > 2)
            {
                sorterGenome = pcs[0].ToSorterGenome();
                sorter = sorterGenome.ToSorter();
                bool.TryParse(pcs[1], out success);
                int.TryParse(pcs[2], out useCount);
            }
            else
            {
                return new SorterGenomeEvalVmImpl(null);
            }

            if (pcs.Count() > 3)
            {
                ancestors = JsonConvert.DeserializeObject<ImmutableStack<Guid>>(pcs[3]);
            }
            else
            {
                sorterEval = SorterEval.Make
                    (
                        sorter: sorter,
                        switchableGroupGuid: switchableGuid,
                        success: success,
                        switchUseCount: useCount
                    );

                return new SorterGenomeEvalVmImpl
                (
                    SorterGenomeEval.Make
                    (
                        sorterGenome: sorterGenome,
                        ancestors: ancestors,
                        sorterEval: sorterEval,
                        generation: generation
                    )
                );
            }

            if (pcs.Count() > 4)
            {
                Guid.TryParse(pcs[4], out switchableGuid);
            }
            else
            {
                sorterEval = SorterEval.Make
                    (
                        sorter: sorter,
                        switchableGroupGuid: switchableGuid,
                        success: success,
                        switchUseCount: useCount
                    );

                return new SorterGenomeEvalVmImpl
                (
                    SorterGenomeEval.Make
                    (
                        sorterGenome: sorterGenome,
                        ancestors: ancestors,
                        sorterEval: sorterEval,
                        generation: generation
                    )
                );
            }

            var useList = JsonConvert.DeserializeObject<List<double>>(pcs[5]);

            sorterEval = SorterEval.Make
            (
                sorter: sorter,
                switchableGroupGuid: switchableGuid,
                success: success,
                switchUseList: useList
            );

            return new SorterGenomeEvalVmImpl
            (
                SorterGenomeEval.Make
                (
                    sorterGenome: sorterGenome,
                    ancestors: ancestors,
                    sorterEval: sorterEval, 
                    generation: generation
                )
            );

        }
    }

    public class SorterGenomeEvalVmImpl : ViewModelBase, ISorterGenomeEvalVm
    {
        public SorterGenomeEvalVmImpl(ISorterGenomeEval sorterGenomeEval)
        {
            _sorterGenomeEval = sorterGenomeEval;
        }

        public int SwitchUseCount
        {
            get { return _sorterGenomeEval.SorterEval.SwitchUseCount; }
        }

        public string Ancestors
        {
            get
            {
                return GetSorterGenomeEval().Ancestors
                    .Aggregate(string.Empty, (s, g) => s + ", " + g.ToString().Substring(0, 5));
            }
        }

        public string AncestorsJson
        {
            get
            {
              return JsonConvert.SerializeObject
                (
                   GetSorterGenomeEval().Ancestors,
                  Formatting.None
              );

            }
        }
        public Guid SwitchableGroupGuid
        {
            get { return _sorterGenomeEval.SorterEval.SwitchableGroupGuid; }
        }

        public string SwitchUseList
        {
            get
            {
                return JsonConvert.SerializeObject
                (
                    GetSorterGenomeEval().SorterEval.SwitchUseList,
                    Formatting.None
                );
            }
        }

        public bool Success
        {
            get { return _sorterGenomeEval.SorterEval.Success; }
        }

        public string SorterGenomeJson
        {
            get { return _sorterGenomeEval.SorterGenome.ToJsonString(); }
        }

        private readonly ISorterGenomeEval _sorterGenomeEval;

        public ISorterGenomeEval GetSorterGenomeEval()
        {
            return _sorterGenomeEval;
        }

    }
}
