using System;
using System.Collections.Generic;
using Dissolve.Server.Services;
using Dissolve.Shared.Entities;
using Dissolve.Shared.Exceptions;
using NUnit.Framework;

namespace Dissolve.Server.UnitTests
{
    [TestFixture]
    public class DissolveJobServiceTests
    {
        private IDissolveJobService dissolveJobService;

        [SetUp]
        public void Setup()
        {
            dissolveJobService = new DissolveJobService();
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
            var imageCount = 1000;

            var splitJob = dissolveJobService.SplitJob(1000, workers);
            var actualTotalTime = dissolveJobService.TotalJobTime(1000, workers);

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

            var imageCount = 1000;

            var splitJob = dissolveJobService.SplitJob(1000, workers);

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

            var imageCount = 1000;

            var splitJob = dissolveJobService.SplitJob(1000, workers);

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
        public void TestWithoutWorkers()
        {
            var workers = new List<ImageEditWorker>();

            const int imageCount = 1000;

            Assert.Throws<NoWorkersException>(() => { dissolveJobService.SplitJob(imageCount, workers); });
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

            Assert.Throws<NoWorkersWhichCanWorkException>(() => { dissolveJobService.SplitJob(imageCount, workers); });
        }
        
    }
}