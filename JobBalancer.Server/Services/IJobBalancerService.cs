using System.Collections.Generic;
using JobBalancer.Shared.Entities;

namespace JobBalancer.Server.Services
{
    public interface IJobBalancerService
    {
        public Dictionary<ImageEditWorker, int> SplitJob(int imageCount, List<ImageEditWorker> workers);

        public int TotalJobTime(int imageCount, List<ImageEditWorker> workers);
    }
}