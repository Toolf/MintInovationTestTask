using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobBalancer.Client.Services
{
    public interface IJobBalancerService
    {
        Task<List<int>> SplitJob(int imageCount, List<int> workers);

        Task<int> TotalTimeJob(int imageCount, List<int> workers);
    }
}