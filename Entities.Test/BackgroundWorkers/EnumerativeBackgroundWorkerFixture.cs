using System;
using System.Linq;
using System.Threading;
using Entities.BackgroundWorkers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entities.Test.BackgroundWorkers
{
    [TestClass]
    public class EnumerativeBackgroundWorkerFixture
    {
        [TestMethod]
        public void TestCtor()
        {
            var inputs = new[] {42,43,44,45};

            var ibw = EnumerativeBackgroundWorker.Make
                (
                    inputs,
                    (i, c) => IterationResult.Make(i + 1, ProgressStatus.StepComplete)
                );

            Assert.AreEqual(ibw.CurrentInput, inputs[0]);
            Assert.AreEqual(ibw.CurrentIteration, 0);
            Assert.AreEqual(ibw.TotalIterations, inputs.Count());
        }

        [TestMethod]
        public void TestUpdate()
        {
            var tokenSource = new CancellationTokenSource();
            var inputs = new[] { 42, 43, 44, 45 };

            var ibw = EnumerativeBackgroundWorker.Make
                (
                    inputs,
                    (i, c) => IterationResult.Make(i + 1, ProgressStatus.StepComplete)
                );

            var nextResult = 0;
            ibw.OnIterationResult.Subscribe(r => nextResult = r.Data);
            ibw.Start(tokenSource);
            Thread.Sleep(100);

            Assert.AreEqual(ibw.CurrentInput, default(int));
            Assert.AreEqual(ibw.CurrentIteration, inputs.Count());
            Assert.AreEqual(ibw.TotalIterations, inputs.Count());
            Assert.AreEqual(ibw.CurrentOutput, 46);
        }

        [TestMethod]
        public void TestCancel()
        {
            var tokenSource = new CancellationTokenSource();
            var inputs = new[] { 42, 43, 44, 45 };

            var ibw = EnumerativeBackgroundWorker.Make
                (
                    inputs,
                    (i, c) => IterationResult.Make(i + 1, ProgressStatus.StepComplete)
                );

            var nextResult = 0;
            ibw.OnIterationResult.Subscribe(
                r =>
                {
                    nextResult = r.Data;
                    tokenSource.Cancel();
                });

            ibw.Start(tokenSource);
            Thread.Sleep(100);

            Assert.IsTrue(ibw.CurrentOutput < 46);
            Assert.IsTrue(ibw.CurrentIteration < inputs.Length);
            Assert.AreEqual(ibw.CurrentOutput, nextResult);
        }
    }
}
