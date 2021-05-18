using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dissolve.Shared.DTO;
using Dissolve.Shared.Entities;

namespace Dissolve.Client.Services
{
    public class DissolveJobService : IDissolveJobService
    {
        private readonly HttpClient _http;
        public DissolveJobService(HttpClient http)
        {
            _http = http;
        }
        public async Task<Dictionary<ImageEditWorker, int>> DissolveJob(int imageCount, List<ImageEditWorker> workers)
        {
            using var response = await _http.PostAsJsonAsync(
                "https://localhost:5001/api/dissolveJob/split", 
                new DissolveJobRequestDto {ImageCount = imageCount, Workers = workers});
            var responseDto =
                await response.Content.ReadFromJsonAsync<DissolveJobResponseDto>();
            
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
                "https://localhost:5001/api/dissolveJob/totalTime",
                new DissolveJobRequestDto {ImageCount = imageCount, Workers = workers});
            var totalTime = await response.Content.ReadFromJsonAsync<int>();
            
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = response.ReasonPhrase;
                Console.WriteLine($"There was an error! {errorMessage}");
                return 0;
            }

            return totalTime;
        }
    }
}