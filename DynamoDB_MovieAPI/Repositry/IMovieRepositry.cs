using System.Collections.Generic;
using System.Threading.Tasks;
using DynamoDB_MovieAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamoDB_MovieAPI.Repositry;

public interface IMovieRepository
{
    public Task<Movie> GetMovieByIdAsync(int id);
    public Task<IEnumerable<Movie>> GetAllMoviesAsync();
    public Task<bool> MovieExistsAsync(int id, string name);
    public Task<IActionResult> AddMovieAsync(Movie movie);
    public Task UpdateMovieAsync(Movie movie);
    public Task<bool> DeleteMovieAsync(int id);
}
