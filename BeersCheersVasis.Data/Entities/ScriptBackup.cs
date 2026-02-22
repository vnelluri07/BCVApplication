using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeersCheersVasis.Data.Entities;

public sealed class ScriptBackup
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [Column("SCRIPT_ID")]
    public int ScriptId { get; set; }

    [Required]
    [Column("PROVIDER")]
    [MaxLength(50)]
    public string Provider { get; set; } = string.Empty;

    [Column("EXTERNAL_ID")]
    [MaxLength(500)]
    public string? ExternalId { get; set; }

    [Column("EXTERNAL_URL")]
    [MaxLength(1000)]
    public string? ExternalUrl { get; set; }

    [Required]
    [Column("BACKED_UP_AT", TypeName = "DATETIME")]
    public DateTime BackedUpAt { get; set; }

    [Required]
    [Column("STATUS")]
    [MaxLength(20)]
    public string Status { get; set; } = "Pending";

    [Column("ERROR_MESSAGE")]
    [MaxLength(2000)]
    public string? ErrorMessage { get; set; }

    [ForeignKey(nameof(ScriptId))]
    public Script Script { get; set; } = null!;
}
