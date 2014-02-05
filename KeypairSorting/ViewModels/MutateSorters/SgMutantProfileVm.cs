using System;
using System.Linq;
using Newtonsoft.Json;
using SorterEvo.Evals;
using WpfUtils;

namespace KeypairSorting.ViewModels.MutateSorters
{
    public interface ISgMutantProfileVm
    {
        double AverageScore { get; }
        Guid ParentGuid { get; }
        string Report { get; }
    }

    public static class SgMutantProfileVm
    {
        public static ISgMutantProfileVm ToSgMutantProfileVm(this ISgMutantProfile sgMutantProfile)
        {
            return new SgMutantProfileVmImpl(sgMutantProfile);
        }
    }

    class SgMutantProfileVmImpl : ViewModelBase, ISgMutantProfileVm
    {
        public SgMutantProfileVmImpl(ISgMutantProfile sgMutantProfile)
        {
            _sgMutantProfile = sgMutantProfile;
            _report = JsonConvert.SerializeObject(
                _sgMutantProfile.Scores.GroupBy(s => s)
                    .OrderBy(g=>g.Key)
                    .Select(g => new[] {g.Key, g.Count()})
                    .ToList()
                );

            _parentGuid = _sgMutantProfile.ParentGenomeEval.Guid;
            _averageScore = _sgMutantProfile.Scores.Average();
        }

        private readonly ISgMutantProfile _sgMutantProfile;

        private readonly string _report;
        public string Report
        {
            get { return _report; }
        }

        private readonly Guid _parentGuid;

        public Guid ParentGuid
        {
            get { return _parentGuid; }
        }

        private readonly double _averageScore;
        public double AverageScore
        {
            get { return _averageScore; }
        }

    }


}
