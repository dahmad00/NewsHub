# NewsHub
A dynamic news website integrating LLM technology to enable users to summarize, analyze, and interact with news content seamlessly.

## Technologies Used
- **ASP.NET MVC**: Built with ASP.NET MVC using .NET 8.0.
- **NewsAPI Library**: Fetches news articles using the NewsAPI NuGet package.

## Features
- **News Categories**: 
  - The website covers 7 categories of news:
    - Business
    - Entertainment
    - General
    - Health
    - Science
    - Sports
    - Technology
  - Each category is fetched using the NewsAPI NuGet package.

- **LLM Integration**:
  - The Large Language Model (LLM) is integrated via API.
  - The LLM allows users to:
    - Summarize news articles.
    - Analyze news content.
    - Ask custom questions related to the news.

- **Recommendation System**:
  - A recommendation system is implemented using Cosine Similarity.
  - It finds the best match news articles for a given user based on their interests.

- **Automated News Fetching**:
  - ASP.NET Jobs are used to fetch news regularly, ensuring that the website is always up-to-date with the latest articles.

## Architecture Diagram
*Include an architecture diagram here to visualize the system design.*

## How It Works
- Users can browse through various news categories.
- The LLM provides interactive features for deeper engagement with the news content.
- The recommendation system enhances user experience by suggesting relevant articles.
