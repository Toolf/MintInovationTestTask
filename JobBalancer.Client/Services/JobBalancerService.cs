using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using JobBalancer.Shared.DTO;
using JobBalancer.Shared.Entities;

namespace JobBalancer.Client.Services
{
    public class JobBalancerService : IJobBalancerService
    {
        private readonly HttpClient _http;

        public JobBalancerService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Dictionary<ImageEditWorker, int>> SplitJob(int imageCount, List<ImageEditWorker> workers)
        {
            using var response = await _http.PostAsJsonAsync(
                "https://localhost:5001/api/jobBalancer/split",
                new JobBalancerRequestDto {ImageCount = imageCount, Workers = workers});
            var responseDto =
                await response.Content.ReadFromJsonAsync<JobBalancerResponseDto>();
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = response.ReasonPhrase;
                return new Dictionary<ImageEditWorker, int>();
            }

            var res = responseDto.Work.ToDictionary(x => x.Worker, x => x.ImageEdit);
            return res;
        }

        public async Task<int> TotalTimeJob(int imageCount, List<ImageEditWorker> workers)
        {
            using var response = await _http.PostAsJsonAsync(
                "https://localhost:5001/api/jobBalancer/totalTime",
                new JobBalancerRequestDto {ImageCount = imageCount, Workers = workers});
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