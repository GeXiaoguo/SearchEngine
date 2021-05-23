using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchEngine.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
        public Task OnPostAsync()
        {
            var keyWordValues = Request.Form["keyWordsInput"];
            var url = Request.Form["urlInput"];

            var keyWords = keyWordValues
                .ToString()
                .Split(",")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            var googleTasks = keyWords.
                Select(x=> GoogleSearch.RankUrlAsync(x, url))
                .ToList();

            var binTasks = keyWords
                .Select(x => BingSearch.RankUrlAsync(x, url))
                .ToList();

            var googleRanks = Task.WhenAll(googleTasks).Result ?? new int?[0];
            var bingRanks = Task.WhenAll(binTasks).Result ?? new int?[0];

            var googleRankString = string.Join(",", googleRanks.Select(x => x.HasValue ? x + 1 : 0));
            var bingRankString = string.Join(",", bingRanks.Select(x => x.HasValue ? x + 1 : 0));

            ViewData["GoogleResult"] = $"Google Ranks: {string.Join(",", keyWords)}  - {url} - {googleRankString}";
            ViewData["BingResult"] = $"Bing Ranks: {string.Join(",", keyWords)} - {url} - {bingRankString}";
            return Task.CompletedTask;
        }
    }
}
