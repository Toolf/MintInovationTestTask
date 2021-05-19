using System.Collections.Generic;
using JobBalancer.Shared.Entities;

namespace JobBalancer.Shared.DTO
{
    public class JobBalancerRequestDto
    {
        public int ImageCount { get; set; }
        public List<ImageEditWorker> Workers { get; set; }
    }
}