# rnz-news

[![](https://img.shields.io/nuget/v/RnzNews)](https://www.nuget.org/packages/RnzNews/)
![](https://img.shields.io/pypi/l/rnz-news)
![](https://img.shields.io/nuget/dt/RnzNews)

## Install.

```bash
dotnet add package RnzNews
```

## Usage.

### 1. Retrive all categories.

```cs
using System;
using RnzNews;

var client = new RnzNewsClient();

foreach (var category in client.Categories)
{
  Console.WriteLine("Path: {0}", category.Path);
  Console.WriteLine("Address: {0}", category.Address);
  Console.WriteLine("Description: {0}", category.Description);
}
```

## 2. Get access to a category with a given path.

```cs
using System;
using RnzNews;

var client = new RnzNewsClient();
var sportCategory = client["news/sport"];
```

## 3. Retrive articles through a category.

```cs
using System;
using RnzNews;

var client = new RnzNewsClient();
var sportCategory = client["news/sport"];

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
  }
}
```
