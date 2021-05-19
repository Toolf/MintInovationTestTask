using System.Collections.Generic;
using JobBalancer.Shared.Entities;

namespace JobBalancer.App.Services
{
    public interface IJobBalancerService
    {
        public List<int> SplitJob(int imageCount, List<int> processingTimes);

        public int TotalJobTime(int imageCount, List<int> processingTimes);
    }
}