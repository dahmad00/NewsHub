using System;
using System.Threading.Tasks;
using System.Reflection;
using NewsAPI;
using NewsAPI.Models;
using NewsAPI.Constants;
using Tech360.Models; // Ensure this namespace matches your project structure
using Microsoft.Extensions.DependencyInjection;
using Tech360.Data;

namespace Tech360.Jobs
{
    public class HistoricalNewsFetcher
    {
        private readonly NewsApiClient _newsApiClient;
        private readonly Tech360Context _dbContext;

        public HistoricalNewsFetcher(Tech360Context dbContext, string apiKey)
        {
            _dbContext = dbContext;
            _newsApiClient = new NewsApiClient(apiKey);
        }

        public async Task FetchAllTimeNews()
        {
            if (_dbContext.InitialFetchComplete)
            {
                Console.WriteLine("Historical fetch already completed.");
                return;
            }

            try
            {
                DateTime fromDate = new DateTime(2024, 6, 5); // Assuming you want news starting from 2024
                DateTime toDate = DateTime.Today;
                var categories = new[] { "business", "entertainment", "general", "health", "science", "sports", "technology" };

                foreach (var category in categories)
                {
                    var articlesResponse = _newsApiClient.GetEverything(new EverythingRequest
                    {
                        Q = category,
                        SortBy = SortBys.PublishedAt,
                        Language = Languages.EN,
                        From = fromDate,
                        To = toDate
                    });

                    if (articlesResponse.Status == Statuses.Ok)
                    {
                        Console.WriteLine($"Fetched {articlesResponse.Articles.Count} articles for category: {category}");
                        foreach (var article in articlesResponse.Articles)
                        {
                            Console.WriteLine("Article Details:");
                            foreach (PropertyInfo property in article.GetType().GetProperties())
                            {
                                Console.WriteLine($"{property.Name}: {property.GetValue(article, null)}");
                            }
                            Console.WriteLine("--------------------------------------------------------");

                            var news = new News
                            {
                                Title = article.Title,
                                Description = article.Description,
                                Url = article.Url,
                                Author = article.Author,
                                PublishedAt = article.PublishedAt ?? DateTime.Now,
                                Source = article.Source.Name,
                                ImageUrl = article.UrlToImage,
                                Category = category
                            };

                            _dbContext.News.Add(news);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to fetch news. Response details:");
                        Console.WriteLine($"Status: {articlesResponse.Status}");
                        Console.WriteLine($"Code: {articlesResponse.Error?.Code}");
                        Console.WriteLine($"Message: {articlesResponse.Error?.Message}");
                    }
                }

                _dbContext.InitialFetchComplete = true;
                await _dbContext.SaveChangesAsync();
                Console.WriteLine($"Fetched news from {fromDate:yyyy-MM-dd} to {toDate:yyyy-MM-dd}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
