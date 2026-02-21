using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeersCheersVasis.Data.Entities;

public sealed class Reaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID")]
    public int Id { get; set; }

    [Column("SCRIPT_ID")]
    public int? ScriptId { get; set; }

    [Column("COMMENT_ID")]
    public int? CommentId { get; set; }

    [Required]
    [Column("VOTER_KEY")]
    [MaxLength(200)]
    public string VoterKey { get; set; } = string.Empty;

    [Required]
    [Column("REACTION_TYPE")]
    public int ReactionType { get; set; }

    [Required]
    [Column("CREATED_DATE", TypeName = "DATETIME")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey(nameof(ScriptId))]
    public Script? Script { get; set; }

    [ForeignKey(nameof(CommentId))]
    public Comment? Comment { get; set; }
}
