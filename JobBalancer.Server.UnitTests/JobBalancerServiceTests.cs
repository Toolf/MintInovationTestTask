using System;
using System.Collections.Generic;
using JobBalancer.Server.Services;
using JobBalancer.Server.Exceptions;
using JobBalancer.Shared.Entities;
using NUnit.Framework;


namespace Dissolve.Server.UnitTests
{
    [TestFixture]
    public class DissolveJobServiceTests
    {
        private IJobBalancerService jobBalancerService;

        [SetUp]
        public void Setup()
        {
            jobBalancerService = new DissolveJobService();
        }

        [Test]
        public void TestTotalTime()
        {
            var workers = new List<ImageEditWorker>
            {
                new ImageEditWorker(1, 2),
                new ImageEditWorker(2, 3),
                new ImageEditWorker(3, 4)
            };
            const int imageCount = 1000;

            var actualTotalTime = jobBalancerService.TotalJobTime(imageCount, workers);

            const double expectedTotalTime = 924;
            Assert.AreEqual(expectedTotalTime, actualTotalTime);
        }

        [Test]
        public void TestIndividualJob()
        {
            var workers = new List<ImageEditWorker>
            {
                new ImageEditWorker(1, 2),
                new ImageEditWorker(2, 3),
                new ImageEditWorker(3, 4)
            };

            const int imageCount = 1000;

            var splitJob = jobBalancerService.SplitJob(imageCount, workers);

            var actualTotalTime = 0.0;
            var actualImageEdited = 0;
            foreach (var (worker, individualImageCount) in splitJob)
            {
                // ReSharper disable once PossibleInvalidOperationException
                actualTotalTime = Math.Max(actualTotalTime, individualImageCount * worker.TimeProcessing);
                actualImageEdited += individualImageCount;
            }

            const double expectedTotalTime = 924;
            const int expectedImageEdited = 1000;
            Assert.AreEqual(expectedTotalTime, actualTotalTime);
            Assert.AreEqual(expectedImageEdited, actualImageEdited);
        }

        [Test]
        public void TestIndividualJob2()
        {
            var workers = new List<ImageEditWorker>
            {
                new ImageEditWorker(1, 7),
                new ImageEditWorker(2, 14),
                new ImageEditWorker(3, 35)
            };

            const int imageCount = 1000;

            var splitJob = jobBalancerService.SplitJob(imageCount, workers);

            var actualTotalTime = 0.0;
            var actualImageEdited = 0;
            foreach (var (worker, individualImageCount) in splitJob)
            {
                // ReSharper disable once PossibleInvalidOperationException
                actualTotalTime = Math.Max(actualTotalTime, individualImageCount * worker.TimeProcessing);
                actualImageEdited += individualImageCount;
            }

            const double expectedTotalTime = 4123;
            const int expectedImageEdited = 1000;
            Assert.AreEqual(expectedTotalTime, actualTotalTime);
            Assert.AreEqual(expectedImageEdited, actualImageEdited);
        }

        [Test]
        public void TestIndividualJob3()
        {
            var workers = new List<ImageEditWorker>
            {
                new ImageEditWorker(1, 171),
                new ImageEditWorker(2, 635),
                new ImageEditWorker(3, 683)
            };

            const int imageCount = 7217;

            var splitJob = jobBalancerService.SplitJob(imageCount, workers);

            var actualTotalTime = 0.0;
            var actualImageEdited = 0;
            foreach (var (worker, individualImageCount) in splitJob)
            {
                // ReSharper disable once PossibleInvalidOperationException
                actualTotalTime = Math.Max(actualTotalTime, individualImageCount * worker.TimeProcessing);
                actualImageEdited += individualImageCount;
            }

            const double expectedTotalTime = 812165;
            const int expectedImageEdited = 7217;
            Assert.AreEqual(expectedTotalTime, actualTotalTime);
            Assert.AreEqual(expectedImageEdited, actualImageEdited);
        }

        [Test]
        public void TestWithoutWorkers()
        {
            var workers = new List<ImageEditWorker>();

            const int imageCount = 1000;

            Assert.Throws<NoWorkersException>(() => { jobBalancerService.SplitJob(imageCount, workers); });
        }

        [Test]
        public void TestWorkerWithNotPositiveTimeProcessing()
        {
            var workers = new List<ImageEditWorker>
            {
                new ImageEditWorker(1, 0),
                new ImageEditWorker(2, -2),
                new ImageEditWorker(3, -1)
            };

            const int imageCount = 1000;

            Assert.Throws<NoWorkersWhichCanWorkException>(() => { jobBalancerService.SplitJob(imageCount, workers); });
        }
    }
}