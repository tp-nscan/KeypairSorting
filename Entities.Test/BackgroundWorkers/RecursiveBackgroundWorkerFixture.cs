using System;
using System.Threading;
using Entities.BackgroundWorkers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entities.Test.BackgroundWorkers
{
    [TestClass]
    public class RecursiveBackgroundWorkerFixture
    {
        [TestMethod]
        public void TestCtor()
        {
            var tokenSource = new CancellationTokenSource();
            const int initialState = 42;
            const int totalIterations = 10;


            var ibw = RecursiveBackgroundWorker.Make
                (
                    initialState, 
                    (i,c) => IterationResult.Make(i + 1, ProgressStatus.StepComplete),
                    totalIterations, 
                    tokenSource
                );

            Assert.AreEqual(ibw.CurrentState, initialState);
            Assert.AreEqual(ibw.CurrentIteration, 0);
            Assert.AreEqual(ibw.TotalIterations, totalIterations);
        }

        [TestMethod]
        public void TestUpdate()
        {
            var tokenSource = new CancellationTokenSource();
            const int initialState = 42;
            const int totalIterations = 10;


            var ibw = RecursiveBackgroundWorker.Make
                (
                    initialState,
                    (i, c) => IterationResult.Make(i + 1, ProgressStatus.StepComplete),
                    totalIterations,
                    tokenSource
                );

            var nextResult = 0;
            ibw.OnIterationResult.Subscribe(r => nextResult = r.Data);
            ibw.Start();
            Thread.Sleep(100);

            Assert.AreEqual(ibw.CurrentState, nextResult);
            Assert.AreEqual(ibw.CurrentIteration, totalIterations);
        }

        [TestMethod]
        public void TestCancel()
        {
            var tokenSource = new CancellationTokenSource();
            const int initialState = 42;
            const int totalIterations = 10;


            var ibw = RecursiveBackgroundWorker.Make
                (
                    initialState,
                    (i, c) =>
                    {
                        Thread.Sleep(10);
                        return IterationResult.Make(i + 1, ProgressStatus.StepComplete);
                    },
                    totalIterations,
                    tokenSource
                );

            var nextResult = initialState;
            ibw.OnIterationResult.Subscribe(
                r =>
                {
                    nextResult = r.Data; 
                    tokenSource.Cancel();
                });

            ibw.Start();
            Thread.Sleep(100);

            Assert.IsTrue(ibw.CurrentState < initialState + totalIterations);
            Assert.IsTrue(ibw.CurrentIteration < totalIterations);
            Assert.AreEqual(ibw.CurrentState, nextResult);
        }

        [TestMethod]
        public void TestError()
        {
            var tokenSource = new CancellationTokenSource();
            const int initialState = 42;
            const int totalIterations = 10;


            var ibw = RecursiveBackgroundWorker.Make
                (
                    initialState,
                    (i, c) =>
                    {
                        Thread.Sleep(10);
                        return IterationResult.Make(i + 1, ProgressStatus.Error);
                    },
                    totalIterations,
                    tokenSource
                );

            var nextResult = initialState;
            ibw.OnIterationResult.Subscribe(
                r =>
                {
                    nextResult = r.Data;
                });

            ibw.Start();
            Thread.Sleep(100);

            Assert.AreEqual(ibw.CurrentState, nextResult);
            Assert.AreEqual(ibw.CurrentState, initialState);
            Assert.AreEqual(ibw.CurrentIteration, 0);
        }

    }
}
