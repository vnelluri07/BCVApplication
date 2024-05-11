using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BeersCheersVasis.Data.Entities;

public sealed class Script
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [Column("TITLE")]
    public string Title { get; set; }

    [Required]
    [Column("CONTENT")]
    public string Content { get; set; }

    [Required]
    [Column("IS_ACTIVE")]
    public bool IsActive { get; set; }

    [Required]
    [Column("CREATED_BY_USER_ID")]
    public int CreatedByUserId { get; set; }

    [Required]
    [Column("CREATED_DATE", TypeName = "DATETIME")]
    public DateTime? CreatedDate { get; set; }

    [Required]
    [Column("MODIFIED_BY_USER_ID")]
    public int ModifiedByUserId { get; set; }

    [Required]
    [Column("MODIFIED_DATE", TypeName = "DATETIME")]
    public DateTime? ModifiedDate { get; set; }
}
