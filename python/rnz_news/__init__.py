from pyquery import PyQuery as pq
from urllib.parse import urljoin

base_url = 'https://www.rnz.co.nz/'

categories = [
    ('news/national', 'New Zealand'),
    ('news/world', 'World'),
    ('news/political', 'Politics'),
    ('international/pacific-news', 'Pacific News'),
    ('news/te-manu-korihi', 'Te Ao Māori'),
    ('news/sport', 'Sport'),
    ('news/business', 'Business'),
    ('news/country', 'Country'),
    ('news/ldr', 'Local Democracy Reporting'),
    ('news/on-the-inside', 'Comment & Analysis'),
    ('news/in-depth', 'In Depth'),
]


class RnzNewsArticle:
    def __init__(self, category, path, title, summary):

        self._category = category
        self._path = path
        self._title = title
        self._summary = summary
        self._body = None
        self._html = None
        self._time = None

    @property
    def category(self):
        return self._category

    @property
    def path(self):
        return self._path

    @property
    def address(self):
        return urljoin(base_url, self.path)

    @property
    def title(self):
        return self._title

    @property
    def summary(self):
        return self._summary

    def _fetch(self):
        doc = pq(url=self.address)

        self._body = doc('.article__body').text()
        self._html = doc('.article__body').html()
        self._time = doc('.article__header .updated').text()

    @property
    def content(self):
        if self._body is None:
            self._fetch()

        return self._body

    @property
    def html(self):
        if self._html is None:
            self._fetch()

        return self._html

    @property
    def time(self):
        if self._time is None:
            self._fetch()

        return self._time


class RnzNewsCategory:
    def __init__(self, path, description):
        self._path = path
        self._description = description

        self._last_page = 0
        self._has_next = True

    @property
    def path(self):
        return self._path

    @property
    def address(self):
        return urljoin(base_url, self.path)

    @property
    def description(self):
        return self._description

    def __getitem__(self, page):

        doc = pq('{}?page={}'.format(self.address, page))

        news_list = []

        for news in doc('.o-digest__detail').items():

            link = news('a.faux-link')

            path = link.attr('href')
            title = link.text()

            summary = news('.o-digest__summary').text()

            news_list.append(RnzNewsArticle(self, path, title, summary))

        has_next = doc('a[rel=next]').length > 0

        return news_list, has_next

    def __iter__(self):
        return self

    def __next__(self):
        if not self._has_next:
            raise StopIteration

        self._last_page += 1

        news_list, self._has_next = self[self._last_page]

        return news_list, self._last_page


class RnzNews:
    def categories(self):
        '''Retrieve all categories.

        Returns: a list of RnzNewsCategory.
        '''
        return [
            RnzNewsCategory(path, description)
            for path, description in categories
        ]

    def __getitem__(self, path):

        for _path, description in categories:
            if _path == path:
                return RnzNewsCategory(path, description)

        raise Exception('No such category')
