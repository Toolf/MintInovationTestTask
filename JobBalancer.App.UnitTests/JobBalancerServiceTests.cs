using System;
using System.Collections.Generic;
using System.Linq;
using JobBalancer.App.Exceptions;
using JobBalancer.App.Services;
using NUnit.Framework;

namespace JobBalancer.App.UnitTests
{
    public class TestData
    {
        public readonly int ImageCount;
        public readonly List<int> ProcessingTimes;
        public readonly int ExpectedTotalTime;
        public readonly int ExpectedImageEdited;

        public TestData(int imageCount, List<int> processingTimes, int expectedTotalTime, int expectedImageEdited)
        {
            ImageCount = imageCount;
            ProcessingTimes = processingTimes;
            ExpectedTotalTime = expectedTotalTime;
            ExpectedImageEdited = expectedImageEdited;
        }
    }

    [TestFixture]
    public class JobBalancerServiceTests
    {
        private IJobBalancerService _jobBalancerService;

        [SetUp]
        public void Setup()
        {
            _jobBalancerService = new JobBalancerService();
        }

        private static readonly TestData[] Cases =
        {
            new TestData(
                1000,
                new List<int> {2, 3, 4},
                924,
                1000
            ),
            new TestData(
                1000,
                new List<int> {7, 14, 35},
                4123,
                1000
            ),
            new TestData(
                7217,
                new List<int> {171, 635, 683},
                812165,
                7217
            ),
            new TestData(
                1000,
                new List<int> {2, 3, 4, -5},
                924,
                1000
            ),
            new TestData(
                0,
                new List<int> {2, 3, 4},
                0,
                0
            )
        };

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void TestTotalTime(TestData testCase)
        {
            var actualTotalTime = _jobBalancerService.TotalJobTime(testCase.ImageCount, testCase.ProcessingTimes);
            Assert.AreEqual(testCase.ExpectedTotalTime, actualTotalTime);
        }

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void TestIndividualJob(TestData testCase)
        {
            var works = _jobBalancerService.SplitJob(testCase.ImageCount, testCase.ProcessingTimes);
            var actualImageEdited = works.Sum();
            Assert.AreEqual(testCase.ExpectedImageEdited, actualImageEdited);
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

            Assert.Throws<NoWorkersWhichCanWorkException>(() =>
            {
                _jobBalancerService.SplitJob(imageCount, processingTimes);
            });
        }

        [Test]
        public void TestNegativeImageCount()
        {
            var processingTimes = new List<int> {2, 3, 5};

            const int imageCount = -1000;

            Assert.Throws<ArgumentException>(() => { _jobBalancerService.SplitJob(imageCount, processingTimes); });
        }
    }
}