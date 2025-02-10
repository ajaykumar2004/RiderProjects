using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using DynamoDB_MovieAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DynamoDB_MovieAPI.Repositry;

public class MovieRepository : IMovieRepository
{
    private readonly IDynamoDBContext _dynamoDbContext;

    public MovieRepository(IDynamoDBContext dynamoDbContext)
    {
        _dynamoDbContext = dynamoDbContext;
    }

    public async Task<Movie> GetMovieByIdAsync(int id)
    {
        return await _dynamoDbContext.LoadAsync<Movie>(id);
    }

    public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
    {
        return await _dynamoDbContext.ScanAsync<Movie>(default).GetRemainingAsync();
    }

    public async Task<bool> MovieExistsAsync(int id, string name)
    {
        var movie = await _dynamoDbContext.LoadAsync<Movie>(id, name);
        return movie != null;
    }

    public async Task<IActionResult> AddMovieAsync(Movie movie)
    {
        await _dynamoDbContext.SaveAsync(movie);
        return new CreatedResult($"/api/movies/{movie.Id}", movie);
    }

    public async Task UpdateMovieAsync(Movie movie)
    {
        await _dynamoDbContext.SaveAsync(movie);
        
    }

    public async Task<bool> DeleteMovieAsync(int id)
    {
        var movie = await _dynamoDbContext.LoadAsync<Movie>(id);
        if (movie == null) return false;

        await _dynamoDbContext.DeleteAsync<Movie>(id);
        return true;
    }
}
