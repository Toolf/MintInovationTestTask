using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using JobBalancer.Shared.DTO;

namespace JobBalancer.Client.Services
{
    public class JobBalancerService : IJobBalancerService
    {
        private readonly HttpClient _http;

        public JobBalancerService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<int>> SplitJob(int imageCount, List<int> processingTimes)
        {
            using var response = await _http.PostAsJsonAsync(
                "https://localhost:5001/api/jobBalancer/split",
                new JobBalancerRequestDto {ImageCount = imageCount, ProcessingTimes = processingTimes});
            var responseDto =
                await response.Content.ReadFromJsonAsync<JobBalancerResponseDto>();
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = response.ReasonPhrase;
                return new List<int>();
            }

            return responseDto.Works;
        }

        public async Task<int> TotalTimeJob(int imageCount, List<int> processingTimes)
        {
            using var response = await _http.PostAsJsonAsync(
                "https://localhost:5001/api/jobBalancer/totalTime",
                new JobBalancerRequestDto {ImageCount = imageCount, ProcessingTimes = processingTimes});
            var totalTime = await response.Content.ReadFromJsonAsync<int>();

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = response.ReasonPhrase;
                return 0;
            }

            return totalTime;
        }
    }
}