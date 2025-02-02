using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DynamoDB_MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        public MoviesController(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovie(int id)
        {
            var movie = await _dynamoDbContext.LoadAsync<Movie>(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _dynamoDbContext.ScanAsync<Movie>(default).GetRemainingAsync();
            return Ok(movies);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAndAddMovie([FromBody] Movie movie)
        {
            var movieExists = await _dynamoDbContext.LoadAsync<Movie>(movie.Id,movie.Name);
            if(movieExists!=null) return BadRequest($"Movie with id :{movie.Id} and name : {movie.Name} already exists");
            await _dynamoDbContext.SaveAsync(movie);
            return Ok(movie);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _dynamoDbContext.LoadAsync<Movie>(id);
            if (movie == null)
            {
                return NotFound();
            }
            await _dynamoDbContext.DeleteAsync<Movie>(id);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateMovie([FromBody] Movie movie)
        {
            var movieExists = await _dynamoDbContext.LoadAsync<Movie>(movie.Id);
            if (movieExists == null)
            {
                return NotFound();
            }
            await _dynamoDbContext.SaveAsync(movie);
            return Ok(movie);
        }
    }
}
