using System;
using System.Linq;
using JobBalancer.App.Exceptions;
using JobBalancer.App.Services;
using JobBalancer.Shared.DTO;
using JobBalancer.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JobBalancer.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobBalancerController : ControllerBase
    {
        private readonly IJobBalancerService _jobBalancerService;

        public JobBalancerController(IJobBalancerService jobBalancerService)
        {
            _jobBalancerService = jobBalancerService;
        }

        [HttpPost]
        [Route("split")]
        public ActionResult<JobBalancerResponseDto> SplitJob([FromBody] JobBalancerRequestDto req)
        {
            var imageCount = req.ImageCount;
            var processingTimes = req.ProcessingTimes;
            try
            {
                var splitJob = _jobBalancerService.SplitJob(imageCount, processingTimes);
                var responseDto = new JobBalancerResponseDto()
                {
                    Work = splitJob
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
            var processingTimes = req.ProcessingTimes;

            try
            {
                var totalTime = _jobBalancerService.TotalJobTime(imageCount, processingTimes);
                return Ok(totalTime);
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
    }
}