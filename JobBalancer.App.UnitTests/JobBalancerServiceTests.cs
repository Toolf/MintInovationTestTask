using System;
using System.Collections.Generic;
using System.Linq;
using JobBalancer.App.Exceptions;
using JobBalancer.App.Services;
using JobBalancer.Shared.Entities;
using NUnit.Framework;

namespace JobBalancer.App.UnitTests
{
    [TestFixture]
    public class JobBalancerServiceTests
    {
        private IJobBalancerService _jobBalancerService;

        [SetUp]
        public void Setup()
        {
            _jobBalancerService = new JobBalancerService();
        }

        [Test]
        public void TestTotalTime()
        {
            var processingTimes = new List<int> {2, 3, 4};
            const int imageCount = 1000;

            var actualTotalTime = _jobBalancerService.TotalJobTime(imageCount, processingTimes);

            const double expectedTotalTime = 924;
            Assert.AreEqual(expectedTotalTime, actualTotalTime);
        }

        [Test]
        public void TestIndividualJob()
        {
            var processingTimes = new List<int> {2, 3, 4};

            const int imageCount = 1000;

            var works = _jobBalancerService.SplitJob(imageCount, processingTimes);

            var actualTotalTime = processingTimes.Select((t, i) => works[i] * t).Prepend(0).Max();
            var actualImageEdited = works.Sum();

            const double expectedTotalTime = 924;
            const int expectedImageEdited = 1000;
            Assert.AreEqual(expectedTotalTime, actualTotalTime);
            Assert.AreEqual(expectedImageEdited, actualImageEdited);
        }

        [Test]
        public void TestIndividualJob2()
        {
            var processingTimes = new List<int> {7, 14, 35};

            const int imageCount = 1000;

            var works = _jobBalancerService.SplitJob(imageCount, processingTimes);

            var actualTotalTime = processingTimes.Select((t, i) => works[i] * t).Prepend(0).Max();
            var actualImageEdited = works.Sum();

            const double expectedTotalTime = 4123;
            const int expectedImageEdited = 1000;
            Assert.AreEqual(expectedTotalTime, actualTotalTime);
            Assert.AreEqual(expectedImageEdited, actualImageEdited);
        }

        [Test]
        public void TestIndividualJob3()
        {
            var processingTimes = new List<int> {171, 635, 683};

            const int imageCount = 7217;

            var works = _jobBalancerService.SplitJob(imageCount, processingTimes);

            var actualTotalTime = processingTimes.Select((t, i) => works[i] * t).Prepend(0).Max();
            var actualImageEdited = works.Sum();

            const double expectedTotalTime = 812165;
            const int expectedImageEdited = 7217;
            Assert.AreEqual(expectedTotalTime, actualTotalTime);
            Assert.AreEqual(expectedImageEdited, actualImageEdited);
        }

        [Test]
        public void TestIgnoreWorkerWithNotPositiveTimeProcessing()
        {
            var processingTimes = new List<int> {2, 3, 4, -5};

            const int imageCount = 1000;

            var works = _jobBalancerService.SplitJob(imageCount, processingTimes);

            var actualTotalTime = processingTimes.Select((t, i) => works[i] * t).Prepend(0).Max();
            var actualImageEdited = works.Sum();


            const double expectedTotalTime = 924;
            const int expectedImageEdited = 1000;
            Assert.AreEqual(expectedTotalTime, actualTotalTime);
            Assert.AreEqual(expectedImageEdited, actualImageEdited);
        }

        [Test]
        public void TestWithoutWorkers()
        {
            var processingTimes = new List<int>();

            const int imageCount = 1000;

            Assert.Throws<NoWorkersException>(() => { _jobBalancerService.SplitJob(imageCount, processingTimes); });
        }

        [Test]
        public void TestNoWorkerWhichCanWork()
        {
            var processingTimes = new List<int> {0, -2, -1};

            const int imageCount = 1000;

            Assert.Throws<NoWorkersWhichCanWorkException>(() => { _jobBalancerService.SplitJob(imageCount, processingTimes); });
        }
    }
}