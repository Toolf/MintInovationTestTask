using System.Collections.Generic;
using Dissolve.Shared.Entities;

namespace Dissolve.Shared.DTO
{
    public class DissolveJobRequestDto
    {
        public int ImageCount { get; set; }
        public List<ImageEditWorker> Workers { get; set; }
    }
}