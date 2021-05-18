using System.Collections.Generic;
using Dissolve.Shared.Entities;

namespace Dissolve.Shared.DTO
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

    public class DissolveJobResponseDto
    {
        public List<IndividualWork> Work { get; set; }
    }
}