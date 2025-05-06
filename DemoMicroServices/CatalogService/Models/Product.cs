using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CatalogService.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [ScaffoldColumn(true)]
        public Guid Id { get; set; }
    
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

    }
}
