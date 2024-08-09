using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Tech360.Data;
using Tech360.Models;
using Tech360.Jobs;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Tech360.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HistoricalNewsFetcher _newsFetcher;
        private readonly Tech360Context _dbContext;

        public HomeController(ILogger<HomeController> logger, HistoricalNewsFetcher newsFetcher, Tech360Context dbContext)
        {
            _newsFetcher = newsFetcher;
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> FetchHistoricalNews()
        {
            await _newsFetcher.FetchAllTimeNews();
            return Content("Historical News have been fetched");
        }

        [HttpGet]
        public async Task<IActionResult> GetNewsByCategory(string category)
        {
            var news = await _dbContext.News
                .Where(n => n.Category == category)
                .OrderByDescending(n => n.PublishedAt)
                .Take(10)
                .ToListAsync();

            return Json(news);
        }
    }
}
