using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchEngine
{
    public static class GoogleSearch
    {
        public static Task<int?> RankUrlAsync(string keyword, string url)
        {
            var encodedKeyword = HttpUtility.UrlEncode(keyword);
            var searchUrl = $"https://www.google.com/search?q={encodedKeyword}&safe=strict&start=0&num=100";
            return SearchEngineUtil.RankUrlAsync(url, searchUrl, DoParse);
        }
        private static int? DoParse(HtmlDocument document, string url)
        {
            var main = document.GetElementById("main");
            var divs = main.GetElementsByTagName("div");

            var searchresuls = divs
                .ToEnumerable()
                .Where(x => x.GetAttribute("className") == "kCrYT")
                .SelectMany(x => x.GetElementsByTagName("a").ToEnumerable())
                .Select((x, index) => (index, x.GetAttribute("href")))
                .Where(x => x.Item2.Contains(url))
                .FirstOrDefault();

            return searchresuls.Item2 != null ? searchresuls.index : null as int?;
        }
    }
}