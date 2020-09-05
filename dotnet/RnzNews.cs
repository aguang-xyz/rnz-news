using System;
using System.Web;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace RnzNews
{
    public static class RnzNewsGlobalVar
    {
        public static readonly Uri BASE_URI = new Uri("https://www.rnz.co.nz/");

        public static readonly (string, string)[] CATEGORIES = {
            ("news/national", "New Zealand"),
            ("news/world", "World"),
            ("news/political", "Politics"),
            ("international/pacific-news", "Pacific News"),
            ("news/te-manu-korihi", "Te Ao Māori"),
            ("news/sport", "Sport"),
            ("news/business", "Business"),
            ("news/country", "Country"),
            ("news/ldr", "Local Democracy Reporting"),
            ("news/on-the-inside", "Comment & Analysis"),
            ("news/in-depth", "In Depth"),
        };
    }


    public class RnzNewsArticle
    {

        private RnzNewsCategory category;

        private string path;

        private string title;

        private string summary;

        private HtmlDocument document = null;

        public RnzNewsArticle(RnzNewsCategory _category, string _path, string _title, string _summary)
        {
            category = _category;
            path = _path;
            title = _title;
            summary = _summary;
        }

        public RnzNewsCategory Category
        {
            get
            {
                return category;
            }
        }

        public string Path
        {
            get
            {
                return path;
            }
        }

        public Uri Address
        {
            get
            {
                return new Uri(RnzNewsGlobalVar.BASE_URI, Path);
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
        }

        public string Summary
        {
            get
            {
                return HttpUtility.HtmlDecode(summary);
            }
        }

        protected HtmlNode DocumentNode
        {
            get
            {
                if (document == null)
                {

                    document = new HtmlWeb().Load(Address);
                }

                return document.DocumentNode;
            }
        }

        public string Content
        {
            get
            {
                return HttpUtility.HtmlDecode(DocumentNode.SelectSingleNode("//div[contains(@class, 'article__body')]")
                  .InnerText.Trim());
            }
        }

        public string Html
        {
            get
            {
                return DocumentNode.SelectSingleNode("//div[contains(@class, 'article__body')]")
                  .InnerHtml.Trim();
            }
        }

        public string Time
        {
            get
            {
                return HttpUtility.HtmlDecode(DocumentNode.SelectSingleNode("//header[contains(@class, 'article__header')]//span[contains(@class, 'updated')]").InnerText.Trim());
            }
        }

    }

    public class RnzNewsCategory : IEnumerable<(RnzNewsArticle[], int)>
    {
        private string path;

        private string description;

        public RnzNewsCategory(string _path, string _description)
        {
            this.path = _path;
            this.description = _description;
        }

        public string Path
        {
            get
            {
                return path;
            }
        }

        public Uri Address
        {
            get
            {
                return new Uri(RnzNewsGlobalVar.BASE_URI, this.path);
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
        }

        public IEnumerator<(RnzNewsArticle[], int)> GetEnumerator()
        {
            var lastPage = 0;
            var hasNext = true;

            while (hasNext)
            {
                var document = new HtmlWeb().Load(new Uri(this.Address, String.Format("?page={0}", ++lastPage)));

                document.OptionEmptyCollection = true;

                var articles = document.DocumentNode
                  .SelectNodes("//div[contains(@class, 'o-digest__detail')]")
                  .Select(detail =>
                {
                    var node = HtmlNode.CreateNode(detail.OuterHtml);

                    var link = node.SelectSingleNode("//a[contains(@class, 'faux-link')]");

                    var path = link.Attributes["href"].Value;
                    var title = HttpUtility.HtmlDecode(link.InnerText.Trim());

                    var summary = HttpUtility.HtmlDecode(node.SelectSingleNode("//div[contains(@class, 'o-digest__summary')]")
                      .InnerText.Trim());

                    return new RnzNewsArticle(this, path, title, summary);
                }).ToArray();

                hasNext = document.DocumentNode.SelectNodes("//a[@rel='next']").Count() > 0;

                yield return (articles, lastPage);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class RnzNewsClient
    {
        public RnzNewsCategory[] Categories
        {
            get
            {
                return RnzNewsGlobalVar.CATEGORIES.Select(category =>
                  new RnzNewsCategory(category.Item1, category.Item2)).ToArray();
            }
        }

        public RnzNewsCategory this[string path]
        {
            get
            {
                return RnzNewsGlobalVar.CATEGORIES.Where(category => category.Item1 == path)
                  .Select(category =>
                      new RnzNewsCategory(category.Item1, category.Item2)).First();
            }
        }
    }

} // namespace RnzNews
