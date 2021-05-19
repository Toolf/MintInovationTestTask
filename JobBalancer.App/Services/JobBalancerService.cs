﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using JobBalancer.Shared.Entities;
using JobBalancer.App.Exceptions;

namespace JobBalancer.App.Services
{
    public class JobBalancerService : IJobBalancerService
    {
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
                if (workers.Count != 0)
                {
                    throw new NoWorkersWhichCanWorkException();
                }

                throw new NoWorkersException();
            }

            if (imageCount < 0)
            {
                throw new ArgumentException("Bad image count. Image count must be not negative number.");
            }

            var orderedWorkers = filteredWorkers.OrderBy((w) => w.JobExecutingSpeed()).ToList();
            for (var slowestWorkerIndex = 0; slowestWorkerIndex < orderedWorkers.Count; slowestWorkerIndex++)
            {
                double totalFrequency = 0;
                for (var i = slowestWorkerIndex; i < orderedWorkers.Count; i++)
                {
                    var w = orderedWorkers[i];
                    // ReSharper disable once PossibleLossOfFraction
                    totalFrequency += (double) 1 / w.TimeProcessing;
                }

                // Min time for processing all images.
                var t1 = imageCount / totalFrequency;
                // Rounded by step to the largest time.
                var individualSpeed = orderedWorkers[slowestWorkerIndex].JobExecutingSpeed();
                t1 = t1 % individualSpeed == 0 ? t1 : t1 + (individualSpeed - t1 % individualSpeed);
                // Images edited by all person by time t1.
                var totalImageEdited = 0;
                for (var i = slowestWorkerIndex; i < orderedWorkers.Count; i++)
                {
                    totalImageEdited += (int) (t1 / orderedWorkers[i].JobExecutingSpeed());
                }

                if (totalImageEdited > imageCount)
                {
                    t1 -= orderedWorkers[slowestWorkerIndex].JobExecutingSpeed();
                }

                // Calculate how many images will be edited by slowest worker in step.
                // At the next step this worker will not be taken into account.
                var imageEdited = (int) (t1 / orderedWorkers[slowestWorkerIndex].JobExecutingSpeed());
                splitJob[orderedWorkers[slowestWorkerIndex]] = imageEdited;
                imageCount -= imageEdited;
            }

            return workers.Select(worker => splitJob[worker]).ToList();
        }

        public int TotalJobTime(int imageCount, List<int> processingTimes)
        {
            var works = SplitJob(imageCount, processingTimes);

            return processingTimes.Select((t, i) => works[i] * t).Prepend(0).Max();
        }
    }
}