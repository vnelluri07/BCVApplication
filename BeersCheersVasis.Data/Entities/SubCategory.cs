using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeersCheersVasis.Data.Entities;

public sealed class SubCategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [Column("CATEGORY_ID")]
    public int CategoryId { get; set; }

    [Required]
    [Column("NAME")]
    [MaxLength(100)]
    public string Name { get; set; }

    [Column("DESCRIPTION")]
    [MaxLength(500)]
    public string? Description { get; set; }

    [Column("ICON")]
    [MaxLength(50)]
    public string? Icon { get; set; }

    [Required]
    [Column("SORT_ORDER")]
    public int SortOrder { get; set; }

    [Required]
    [Column("IS_ACTIVE")]
    public bool IsActive { get; set; }

    [Required]
    [Column("CREATED_DATE", TypeName = "DATETIME")]
    public DateTime? CreatedDate { get; set; }

    [Required]
    [Column("MODIFIED_DATE", TypeName = "DATETIME")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; }

    public ICollection<Script> Scripts { get; set; } = new List<Script>();
}
