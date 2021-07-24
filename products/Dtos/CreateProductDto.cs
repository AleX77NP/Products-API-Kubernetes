using System;
using System.ComponentModel.DataAnnotations;

namespace products.Dtos
{
    public class CreateProductDto
    {
        [Required]
        public string Title { get; init; }
        [Required]
        [Range(1, 1000)]
        public decimal Price { get; init; }
    }
}
