using System;
using System.ComponentModel;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using Entities.BackgroundWorkers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entities.Test.BackgroundWorkers
{
    [TestClass]
    public class BackgroundWorkerFixture
    {
        [TestMethod]
        public async Task TestBackgroundWorker()
        {
            var bkWkr = Entities.BackgroundWorkers.BackgroundWorker.Make<string, string>
                (
                    s =>
                    {
                            var k = 1;
                            Thread.Sleep(1000);
                            for (var i = 0; i < 1000000; i++)
                            {
                                k++;
                            }
                            return s + k;
                        }
                );

            var res = TryThis(bkWkr);
            bkWkr.Cancel();
            res.Wait();
        }

        async Task<string> TryThis(IBackgroundWorker<string, string> backgroundWorker)
        {
            return await backgroundWorker.RunAsync("youuyw");
        }
    }
}
