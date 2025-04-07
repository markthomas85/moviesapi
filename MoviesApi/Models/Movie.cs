using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Models
{
    [Keyless]
    public class Movie
    {
        public DateOnly Release_Date { get; set; }
        public string? Title { get; set; } = string.Empty;
        public string? Overview { get; set; } = string.Empty;
        public decimal? Popularity { get; set; } = 0;
        public int? Vote_Count { get; set; } = 0;
        public decimal? Vote_Average { get; set; } = 0;
        public string? Original_Language {  get; set; } = string.Empty;
        public string? Genre { get; set; } = string.Empty;
        public string? Poster_Url { get; set; } = string.Empty;
    }
}
