from unittest import TestCase
from rnz_news import RnzNews


class RnzNewsTest(TestCase):
    def test_all_categories(self):

        rnz_news = RnzNews()

        for category in rnz_news.categories():

            print('path: ', category.path)
            print('address: ', category.address)
            print('description: ', category.description)

            print('\n')

    def test_all_articles(self):

        sport_category = RnzNews()['news/sport']

        for articles, page in sport_category:

            print('page: ', page)

            for article in articles:

                print('category: ', article.category)
                print('path: ', article.path)
                print('address: ', article.address)
                print('title: ', article.title)
                print('summary: ', article.summary)
                print('time: ', article.time)
                print('content: ', article.content)
                print('html: ', article.html)

                print('\n')

                break
            break
