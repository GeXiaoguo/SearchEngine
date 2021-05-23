using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchEngine
{
    public static class SearchEngineUtil 
    {
        public static async Task<int?> RankUrlAsync(string url, string searchEngineUrl, Func<HtmlDocument, string, int?> parser)
        {
            var html = await SearchResultCache.GetAsync(searchEngineUrl, () => GetAsync(searchEngineUrl));
            if (html == null)
            {
                return null;
            }
            var taskCompletionSource = new TaskCompletionSource<int?>();
            HtmlParserUtil.ParseSearchResult(html, doc => parser(doc, url), taskCompletionSource);
            return await taskCompletionSource.Task;
        }

        private static HttpClient _httpClient = new HttpClient();

        private static async Task<string?> GetAsync(string searchEngineUrl)
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Dotnet HttpClient");
            var response = await _httpClient.GetAsync(searchEngineUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string htmlText = await response.Content.ReadAsStringAsync();
                return htmlText;
            }
            return null;
        }
    }
}