using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeersCheersVasis.Data.Entities;

public sealed class AppUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID")]
    public int Id { get; set; }

    [Column("GOOGLE_ID")]
    [MaxLength(200)]
    public string? GoogleId { get; set; }

    [Required]
    [Column("DISPLAY_NAME")]
    [MaxLength(200)]
    public string DisplayName { get; set; }

    [Column("EMAIL")]
    [MaxLength(300)]
    public string? Email { get; set; }

    [Column("AVATAR_URL")]
    [MaxLength(500)]
    public string? AvatarUrl { get; set; }

    [Required]
    [Column("ROLE")]
    [MaxLength(20)]
    public string Role { get; set; } = "User";

    [Required]
    [Column("IS_ANONYMOUS")]
    public bool IsAnonymous { get; set; }

    [Required]
    [Column("IS_ACTIVE")]
    public bool IsActive { get; set; } = true;

    [Required]
    [Column("CREATED_DATE", TypeName = "DATETIME")]
    public DateTime? CreatedDate { get; set; }

    [Required]
    [Column("MODIFIED_DATE", TypeName = "DATETIME")]
    public DateTime? ModifiedDate { get; set; }

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
