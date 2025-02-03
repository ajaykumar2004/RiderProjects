using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DynamoDB_MovieAPI.Models;
using DynamoDB_MovieAPI.Repositry;
namespace DynamoDB_MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;

        public MoviesController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovie(int id)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);
            if (movie == null) return NotFound();

            return Ok(movie);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _movieRepository.GetAllMoviesAsync();
            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAndAddMovie([FromBody] Movie movie)
        {
            if (await _movieRepository.MovieExistsAsync(movie.Id, movie.Name))
                return BadRequest($"Movie with ID {movie.Id} and name '{movie.Name}' already exists.");

            await _movieRepository.AddMovieAsync(movie);
            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] Movie movie)
        {
            if (id != movie.Id)
                return BadRequest("ID in the URL must match the movie's ID.");

            var existingMovie = await _movieRepository.GetMovieByIdAsync(id);
            if (existingMovie == null) return NotFound();

            await _movieRepository.UpdateMovieAsync(movie);
            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            bool deleted = await _movieRepository.DeleteMovieAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
