using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeersCheersVasis.Data.Entities;

public sealed class Comment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [Column("SCRIPT_ID")]
    public int ScriptId { get; set; }

    [Required]
    [Column("APP_USER_ID")]
    public int AppUserId { get; set; }

    [Column("PARENT_COMMENT_ID")]
    public int? ParentCommentId { get; set; }

    [Required]
    [Column("CONTENT")]
    [MaxLength(2000)]
    public string Content { get; set; }

    [Required]
    [Column("IS_DELETED")]
    public bool IsDeleted { get; set; }

    [Required]
    [Column("CREATED_DATE", TypeName = "DATETIME")]
    public DateTime? CreatedDate { get; set; }

    [Required]
    [Column("MODIFIED_DATE", TypeName = "DATETIME")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey(nameof(ScriptId))]
    public Script Script { get; set; }

    [ForeignKey(nameof(AppUserId))]
    public AppUser AppUser { get; set; }

    [ForeignKey(nameof(ParentCommentId))]
    public Comment? ParentComment { get; set; }

    public ICollection<Comment> Replies { get; set; } = new List<Comment>();
}
