
using System.Collections.Generic;
using System.Threading.Tasks;
using JobBalancer.Shared.Entities;

namespace JobBalancer.Client.Services
{
    public interface IJobBalancerService
    {
        Task<Dictionary<ImageEditWorker, int>> SplitJob(int imageCount, List<ImageEditWorker> workers);

        Task<int> TotalTimeJob(int imageCount, List<ImageEditWorker> workers);
    }
}