using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchEngine
{
    public static class HtmlParserUtil
    {
        public static void ParseSearchResult(string html, Func<HtmlDocument, int?> parseRank, TaskCompletionSource<int?> taskCompletionSource)
        {
            var staThread = new Thread(() => {
                var br = new WebBrowser();
                br.ScriptErrorsSuppressed = true;
                br.DocumentText = html;
                br.DocumentCompleted += (sender, args) =>
                {
                    var br = sender as WebBrowser;
                    var rank = parseRank(br.Document);
                    taskCompletionSource.SetResult(rank);
                    br.Dispose();
                    Application.Exit();
                };
                Application.Run();
            });
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
        }
    }
}