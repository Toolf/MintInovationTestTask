using System.Collections.Generic;
using Dissolve.Shared.Entities;

namespace Dissolve.Server.Services
{
    public interface IDissolveJobService
    {
        public Dictionary<ImageEditWorker, int> SplitJob(int imageCount, List<ImageEditWorker> workers);

        public int TotalJobTime(int imageCount, List<ImageEditWorker> workers);
    }
}