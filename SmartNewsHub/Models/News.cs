using System;
using System.ComponentModel.DataAnnotations;

namespace Tech360.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Url { get; set; }

        public string? Author { get; set; }

        public DateTime? PublishedAt { get; set; }

        public string? Source { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public string? Category { get; set; }
    }
}
