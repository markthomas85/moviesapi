using Azure;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;

namespace MoviesApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MoviesController : Controller
    {
        private readonly MovieContext _movieContext;

        public MoviesController(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        // GET: api/<Movies>
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery(Name = "page")] int? page = -1,
            [FromQuery(Name = "limit")] int? limit = 0
        )
        {
            limit = (page >= 0) ? 10 : limit;
            int rows = _movieContext.Movies.Count();
            int maxrows = (!limit.HasValue || limit <= 0) ? rows : limit.Value;
            page = (page < 0) ? 0 : page;
            List<Movie> movieList = await _movieContext.Movies
                .OrderBy(m => m.Title)
                .Skip((int)page * 10)
                .Take(maxrows)
                .ToListAsync();
            return Ok(movieList);
        }

        //GET api/<Movies>/filterField/filterText
        [HttpGet("{field}/{searchText}")]
        public async Task<IActionResult> Get(
            string field,
            string searchText,
            [FromQuery(Name = "page")] int? page = -1,
            [FromQuery(Name = "limit")] int? limit = 0,
            [FromQuery(Name = "sort")] string? sort = "title"
        )
        {
            limit = (page >= 0) ? 10 : limit;
            int rows = _movieContext.Movies.Count();
            int maxrows = (!limit.HasValue || limit <= 0) ? rows : limit.Value;
            page = (page < 0) ? 0 : page;
            field = field.ToLower();
            sort = sort.ToLower();
            bool noFilter = !(field == "genre" || field == "title");
            List<Movie> movieList = await _movieContext.Movies
                .Where(m => (m.Genre.Contains(searchText) && field == "genre") ||
                            (m.Title.Contains(searchText) && field == "title") ||
                            noFilter
                      )
                .OrderBy(m => m.Title)
                .Skip((int)page * 10)
                .Take(maxrows)
                .ToListAsync();
            List<Movie> sorted = movieList;
            switch (sort)
            {
                case "title":
                    return Ok(movieList.OrderBy(m => m.Title));
                case "title_desc":
                    return Ok(movieList.OrderByDescending(m => m.Title));
                case "releasedate":
                    return Ok(movieList.OrderBy(m => m.Release_Date));
                case "releasedate_desc":
                    return Ok(movieList.OrderByDescending(m => m.Release_Date));
                 default:
                    return Ok(movieList);
            }
        }
    }
}
