using Quartz;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Tech360.Models;
using Microsoft.Extensions.DependencyInjection;
using Tech360.Data;

namespace Tech360.Jobs
{
    public class FetchNewsJob : IJob
    {
        private readonly IServiceProvider _serviceProvider;

        public FetchNewsJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Tech360Context>();

                string baseUrl = "https://newsapi.org/v2/top-headlines";
                string apiKey = "310161df8cde46de89e690b40eb8b448";
                var parameters = new Dictionary<string, string>
                {
                    {"category", "technology"},
                    {"apiKey", apiKey}
                };

                var queryString = new FormUrlEncodedContent(parameters).ReadAsStringAsync().Result;
                string requestUri = $"{baseUrl}?{queryString}";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Tech360 (contact@example.com)");

                    HttpResponseMessage response = await client.GetAsync(requestUri);
                    string responseBody = await response.Content.ReadAsStringAsync();

                    Console.WriteLine($"HTTP Response: {response}");
                    Console.WriteLine($"Response Body: {responseBody}");

                    if (response.IsSuccessStatusCode)
                    {
                        var newsResult = JsonConvert.DeserializeObject<NewsResult>(responseBody); // Ensure NewsResult matches JSON structure

                        foreach (var newsItem in newsResult.Articles)
                        {
                            var news = new News
                            {
                                Title = newsItem.Title,
                                Description = newsItem.Description,
                                Url = newsItem.Url,
                                Author = newsItem.Author,
                                PublishedAt = newsItem.PublishedAt,
                                Source = newsItem.Source.Name,
                                ImageUrl = newsItem.UrlToImage
                            };

                            dbContext.News.Add(news);
                        }

                        await dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        Console.WriteLine("Failed to retrieve news.");
                    }
                }
            }
        }
    }
}
