using System.Collections.Generic;

namespace JobBalancer.App.Services
{
    public interface IJobBalancerService
    {
        public List<int> SplitJob(int imageCount, List<int> processingTimes);

        public int TotalJobTime(int imageCount, List<int> processingTimes);
    }
}