using BookStoreApp.Business.Abstract;
using BookStoreApp.Entities.ComplexTypes;
using Microsoft.Extensions.Options;
using Elastic.Clients.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStoreApp.Business.DTOs;
using BookStoreApp.Core.Utilities.Config;
using BookStoreApp.Entities.Concrete;
using Elastic.Clients.Elasticsearch.QueryDsl;

public class ElasticSearchManager : IElasticsearchService
{
    private readonly ElasticsearchClient _elasticClient;
    private readonly ElasticsearchConfig _config;
    private readonly Fuzziness _fuzziness;

    public ElasticSearchManager(ElasticsearchClient elasticClient,IOptions<ElasticsearchConfig >config)
    {
        _elasticClient = elasticClient;
        _config = config.Value;
        
    }

    public async Task IndexBookAsync(BookDetails book)
    {
        var response = await _elasticClient.IndexAsync(new BookDetails {BookId = book.BookId,BookReviews = book.BookReviews,BookImage = book.BookImage,BookPrice = book.BookPrice,BookRate = book.BookRate,AuthorName = book.AuthorName,BookDescription = book.BookDescription,BookTitle = book.BookTitle}, idx => idx.Index("books").Id(book.BookId.ToString()) );
        if (!response.IsValidResponse)
        {
            throw new Exception("Failed to index document");
        }
    }

    public async Task<IEnumerable<BookDetails>> SearchBooksAsync(string query)
    {
        var searchResponse = await _elasticClient.SearchAsync<BookDetails>(s => s
            .Index(_config.IndexName)
            .Query(q => q
                .Bool(b => b
                    .Should(
                        sh => sh
                            .MultiMatch(m => m
                                .Fields(new[] { "bookTitle", "bookDescription", "authorName" })
                                .Query(query)
                                .Fuzziness(new Fuzziness("AUTO"))
                                .PrefixLength(1)
                            ),
                        sh => sh
                            .Wildcard(w => w
                                .Field("bookTitle")
                                .Value($"*{query}*")
                            ),
                        sh => sh
                            .Wildcard(w => w
                                .Field("bookDescription")
                                .Value($"*{query}*")
                            ),
                        sh => sh
                            .Wildcard(w => w
                                .Field("authorName")
                                .Value($"*{query}*")
                            )
                    )
                )
            )
        );

        if (!searchResponse.IsValidResponse)
        {
            throw new Exception($"Failed to search documents: {searchResponse.DebugInformation}");
        }

        return searchResponse.Documents.Select(book => new BookDetails { BookId = book.BookId, BookReviews = book.BookReviews, BookImage = book.BookImage, BookPrice = book.BookPrice, BookRate = book.BookRate, AuthorName = book.AuthorName, BookDescription = book.BookDescription, BookTitle = book.BookTitle });
    }





}