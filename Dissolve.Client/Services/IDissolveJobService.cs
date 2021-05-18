
using System.Collections.Generic;
using System.Threading.Tasks;
using Dissolve.Shared.Entities;

namespace Dissolve.Client.Services
{
    public interface IDissolveJobService
    {
        Task<Dictionary<ImageEditWorker, int>> DissolveJob(int imageCount, List<ImageEditWorker> workers);

        Task<int> TotalTimeJob(int imageCount, List<ImageEditWorker> workers);
    }
}