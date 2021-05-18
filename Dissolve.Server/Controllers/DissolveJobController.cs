using System;
using System.Linq;
using Dissolve.Server.Services;
using Dissolve.Shared.DTO;
using Dissolve.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Dissolve.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DissolveJobController : ControllerBase
    {
        [HttpPost]
        [Route("split")]
        public ActionResult<DissolveJobResponseDto> SplitJob([FromBody] DissolveJobRequestDto req)
        {
            var imageCount = req.ImageCount;
            var workers = req.Workers;

            try
            {
                var service = new DissolveJobService();
                var splitJob = service.SplitJob(imageCount, workers);
                var responseDto = new DissolveJobResponseDto()
                {
                    Work = splitJob.Select((x) => new IndividualWork()
                    {
                        Worker = x.Key,
                        ImageEdit = x.Value
                    }).ToList()
                };
                return Ok(responseDto);
            }
            catch (NoWorkersException e)
            {
                return BadRequest("List of workers should not be empty.");
            }
        }

        [HttpPost]
        [Route("totalTime")]
        public ActionResult<int> TotalJobTime([FromBody] DissolveJobRequestDto req)
        {
            var imageCount = req.ImageCount;
            var workers = req.Workers;

            try
            {
                var service = new DissolveJobService();
                var totalTime = service.TotalJobTime(imageCount, workers);
                return Ok(totalTime);
            }
            catch (NoWorkersException e)
            {
                return BadRequest("List of workers should not be empty.");
            }
        }
    }
}