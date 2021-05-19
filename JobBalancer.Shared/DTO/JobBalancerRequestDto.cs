using System.Collections.Generic;

namespace JobBalancer.Shared.DTO
{
    public class JobBalancerRequestDto
    {
        public int ImageCount { get; set; }
        public List<int> ProcessingTimes { get; set; }
    }
}