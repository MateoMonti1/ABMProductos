using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABMProductos.Entities;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }
    public int? CategoryId { get; set; }
}
