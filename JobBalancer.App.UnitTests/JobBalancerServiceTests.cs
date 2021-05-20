using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        private const int MaxRandomWorkersCount = 20;
        private const int MaxRandomProcessingTime = 100;
        private const int MaxRandomImageCount = 10000;

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
            ),
            new TestData(
                9,
                new List<int> {2, 7, 9, 4, 9, 7, 6, 4, 2, 7, 6},
                6,
                9
            )
        };

        private static List<int> SplitJobSimulator(int imageCount, List<int> processingTimes)
        {
            var time = 1;
            var imageEdited = 0;
            var individualWork = new List<int>(processingTimes.Count);
            for (var i = 0; i < processingTimes.Count; i++)
            {
                individualWork.Add(0);
            }

            while (true)
            {
                for (var i = 0; i < processingTimes.Count; i++)
                {
                    var editImage = time % processingTimes[i] == 0;
                    if (!editImage) continue;

                    individualWork[i]++;
                    imageEdited++;
                    if (imageEdited == imageCount)
                    {
                        return individualWork;
                    }
                }

                time++;
            }
        }

        private static TestData RandomCase()
        {
            var rand = new Random();
            var workersCount = rand.Next(MaxRandomWorkersCount);
            var processingTimes = new List<int>(workersCount);
            for (var i = 0; i < workersCount; i++)
            {
                processingTimes.Add(rand.Next(1, MaxRandomProcessingTime));
            }

            var imageCount = rand.Next(MaxRandomImageCount);
            var individualWorks = SplitJobSimulator(imageCount, processingTimes);

            var expectedTotalTime = 0;
            for (var i = 0; i < workersCount; i++)
            {
                expectedTotalTime = Math.Max(expectedTotalTime, individualWorks[i] * processingTimes[i]);
            }

            var test = new TestData(imageCount, processingTimes, expectedTotalTime, imageCount);
            return test;
        }

        private static readonly TestData[] RandomCases = Enumerable.Repeat(RandomCase(), 100).ToArray();

        [Test]
        [TestCaseSource(nameof(Cases))]
        public void TestTotalTime(TestData testCase)
        {
            var actualTotalTime = _jobBalancerService.TotalJobTime(testCase.ImageCount, testCase.ProcessingTimes);
            Assert.AreEqual(testCase.ExpectedTotalTime, actualTotalTime);
        }

        [Test]
        [TestCaseSource(nameof(RandomCases))]
        public void TestTotalTimeRandomCases(TestData testCase)
        {
            var actualTotalTime = _jobBalancerService.TotalJobTime(testCase.ImageCount, testCase.ProcessingTimes);
            testCase.ProcessingTimes.ForEach(Console.WriteLine);
            Assert.AreEqual(testCase.ExpectedTotalTime, actualTotalTime,
                $"{testCase.ImageCount}, {testCase.ProcessingTimes}");
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
        [TestCaseSource(nameof(RandomCases))]
        public void TestIndividualJobRandomCases(TestData testCase)
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