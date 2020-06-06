# rnz-news

[![](https://img.shields.io/pypi/v/rnz-news)](https://badge.fury.io/py/rnz-news)
![](https://img.shields.io/pypi/l/rnz-news)

Retrieve RNZ news.

## Install.

```bash
pip install rnz-news
```

## Usage.

### 1. Retrieve all categories.

```python
rnz_news = RnzNews()

for category in rnz_news.categories():

    print(category.path)
    print(category.address)
    print(category.description)
```

### 2. Get access to a category with a given path.

```python
sport_category = RnzNews()['news/sport']
```

### 3. Retrive articles through a category.

```python
sport_category = RnzNews()['news/sport']

for articles, page in sport_category:

    print('page: {}'.format(page))

    for article in articles:

        print(article.category)
        print(article.path)
        print(article.address)
        print(article.title)
        print(article.summary)
        print(article.time)
        print(article.content)
        print(article.html)
```
