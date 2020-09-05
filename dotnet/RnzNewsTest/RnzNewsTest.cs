using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using RnzNews;

namespace RnzNewsTest
{
    [TestClass]
    public class RnzNewsTest
    {
        [TestMethod]
        public void TestCategories()
        {
            var client = new RnzNewsClient();

            Assert.IsTrue(client.Categories.Count() > 0);

            foreach (var category in client.Categories)
            {

                Console.WriteLine("Path: {0}", category.Path);
                Console.WriteLine("Address: {0}", category.Address);
                Console.WriteLine("Description: {0}", category.Description);
            }
        }

        [TestMethod]
        public void TestAllArticles()
        {
            var client = new RnzNewsClient();
            var sportCategory = client["news/sport"];

            Assert.IsNotNull(sportCategory != null);

            foreach (var chunk in sportCategory)
            {
                var articles = chunk.Item1;
                var page = chunk.Item2;

                Console.WriteLine("Page: {0}", page);

                foreach (var article in articles)
                {
                    Console.WriteLine("Category: {0}", article.Category);
                    Console.WriteLine("Path: {0}", article.Path);
                    Console.WriteLine("Address: {0}", article.Address);
                    Console.WriteLine("Title: {0}", article.Title);
                    Console.WriteLine("Summary: {0}", article.Summary);
                    Console.WriteLine("Time: {0}", article.Time);
                    Console.WriteLine("Content: {0}", article.Content);
                    Console.WriteLine("Html: {0}", article.Html);

                    break;
                }

                break;
            }
        }
    }
} // namespace RnzNewsTest
