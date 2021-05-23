using MSHTML;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SearchEngine
{
    public static class HtmlElementExtension
    {
        static public IEnumerable<HtmlElement> ToEnumerable(this HtmlElementCollection collection)
        {
            foreach (HtmlElement elm in collection)
            {
                yield return elm;
            }
        }
    }
}
