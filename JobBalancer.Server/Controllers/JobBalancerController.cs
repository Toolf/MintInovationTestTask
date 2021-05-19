using System;
using System.Linq;
using JobBalancer.Shared.DTO;
using JobBalancer.Server.Exceptions;
using JobBalancer.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobBalancer.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobBalancerController : ControllerBase
    {
        [HttpPost]
        [Route("split")]
        public ActionResult<JobBalancerResponseDto> SplitJob([FromBody] JobBalancerRequestDto req)
        {
            var imageCount = req.ImageCount;
            var workers = req.Workers;

            try
            {
                var service = new DissolveJobService();
                var splitJob = service.SplitJob(imageCount, workers);
                var responseDto = new JobBalancerResponseDto()
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
                return UnprocessableEntity("List of workers should not be empty.");
            }
            catch (ArgumentException e)
            {
                
                return UnprocessableEntity(e.Message);
            }
        }

        [HttpPost]
        [Route("totalTime")]
        public ActionResult<int> TotalJobTime([FromBody] JobBalancerRequestDto req)
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
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}