using System.Collections.Generic;
using JobBalancer.Shared.Entities;

namespace JobBalancer.Shared.DTO
{
    public class IndividualWork
    {
        public ImageEditWorker Worker { get; set; }
        public int ImageEdit { get; set; }

        public IndividualWork()
        {
            Worker = new ImageEditWorker();
            ImageEdit = 0;
        }
    }

    public class JobBalancerResponseDto
    {
        public List<IndividualWork> Work { get; set; }
    }
}