using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using JobBalancer.App.Exceptions;
using JobBalancer.Core.Entities;

namespace JobBalancer.App.Services
{
    /// <summary>Class <c>JobBalancerService</c> has methods for balance
    /// jobs and determinate total time for execution jobs.</summary>
    public class JobBalancerService : IJobBalancerService
    {
        /// <summary>Method <c>SplitJob</c> split images between all worker by their processing time.
        /// <example>For example:
        /// <code>
        ///     IJobBalancerService service = new JobBalancerService();
        ///     var works = service.SplitJob(5, new List<int>(){2,3,4});
        /// </code>
        /// result <c>works</c> is (3, 1, 1) 
        /// </example>
        /// </summary>
        /// <exception cref="NoWorkersException"></exception>
        /// <exception cref="NoWorkersWhichCanWorkException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public List<int> SplitJob(int imageCount, List<int> processingTimes)
        {
            var workers = processingTimes.Select((time, index) => new ImageEditWorker(index, time)).ToList();
            var splitJob = workers.ToDictionary(worker => worker, worker => 0);

            // Filter workers with positive time processing.
            var filteredWorkers = (from worker in workers
                where worker.TimeProcessing > 0
                select worker).ToList();


            if (filteredWorkers.Count == 0)
            {
                if (workers.Count != 0) throw new NoWorkersWhichCanWorkException();

                throw new NoWorkersException();
            }

            if (imageCount < 0)
                throw new ArgumentException("Bad image count. Image count must be not negative number.");

            var orderedWorkers = filteredWorkers.OrderBy((w) => w.TimeProcessing).ToList();
            for (var slowestWorkerIndex = 0; slowestWorkerIndex < orderedWorkers.Count; slowestWorkerIndex++)
            {
                double totalFrequency = 0;
                for (var i = slowestWorkerIndex; i < orderedWorkers.Count; i++)
                {
                    var w = orderedWorkers[i];
                    totalFrequency += (double) 1 / w.TimeProcessing;
                }

                // Min time for processing all images.
                var t1 = imageCount / totalFrequency;
                // Rounded by step to the largest time.
                var individualSpeed = orderedWorkers[slowestWorkerIndex].TimeProcessing;
                t1 = t1 % individualSpeed == 0 ? t1 : t1 + (individualSpeed - t1 % individualSpeed);
                // Images edited by all person by time t1.
                var totalImageEdited = 0;
                for (var i = slowestWorkerIndex; i < orderedWorkers.Count; i++)
                    totalImageEdited += (int) (t1 / orderedWorkers[i].TimeProcessing);

                if (totalImageEdited > imageCount) t1 -= orderedWorkers[slowestWorkerIndex].TimeProcessing;

                // Calculate how many images will be edited by slowest worker in step.
                // At the next step this worker will not be taken into account.
                var imageEdited = (int) (t1 / orderedWorkers[slowestWorkerIndex].TimeProcessing);
                splitJob[orderedWorkers[slowestWorkerIndex]] = imageEdited;
                imageCount -= imageEdited;
            }

            return workers.Select(worker => splitJob[worker]).ToList();
        }

        /// <summary>Method <c>TotalJobTime</c> calculate minimal time for processing images
        /// <example>For example:
        /// <code>
        ///     IJobBalancerService service = new JobBalancerService();
        ///     var totalTime = service.TotalJobTime(5, new List<int>(){2,3,4});
        /// </code>
        /// result <c>totalTime</c> is 6
        /// </example>
        /// </summary>
        /// <exception cref="NoWorkersException"></exception>
        /// <exception cref="NoWorkersWhichCanWorkException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public int TotalJobTime(int imageCount, List<int> processingTimes)
        {
            var works = SplitJob(imageCount, processingTimes);

            return processingTimes.Select((t, i) => works[i] * t).Prepend(0).Max();
        }
    }
}