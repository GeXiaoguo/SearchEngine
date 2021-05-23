using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchEngine
{
    public static class BingSearch
    {
        public static Task<int?> RankUrlAsync(string keyword, string url)
        {
            var encodedKeyword = HttpUtility.UrlEncode(keyword);
            var searchUrl = $"https://www.bing.com/search?q={encodedKeyword}&safeSearch=Strict&count=100";
            return SearchEngineUtil.RankUrlAsync(url, searchUrl, DoParse);
        }

        private static int? DoParse(HtmlDocument document, string url)
        {
            var main = document.GetElementById("b_results");
            var lis = main.GetElementsByTagName("li");

            var searchresuls = lis
                .ToEnumerable()
                .Where(x => x.GetAttribute("className") == "b_algo")
                .SelectMany(x => x.GetElementsByTagName("a").ToEnumerable())
                .Select((x, index) => (index, x.GetAttribute("href")))
                .Where(x => x.Item2.Contains(url))
                .FirstOrDefault();

            return searchresuls.Item2 != null ? searchresuls.index : null as int?;
        }
    }
}