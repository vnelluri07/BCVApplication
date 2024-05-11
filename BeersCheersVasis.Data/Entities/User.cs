using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BeersCheersVasis.Data.Entities;

//[Table("dbo.USERS", Schema = "tempdb")]
public sealed class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [Column("NAME")]
    public string Name { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    [Column("EMAIL")]
    public string Email { get; set; }

    [Required]
    [Column("PASSWORD_SALT")]
    public byte[] PasswordSalt { get; set; }

    [Required]
    [Column("PASSWORD_HASH")]
    public byte[] PasswordHash { get; set; }

    [Required]
    [Column("IS_ACTIVE")]
    public int IsActive { get; set; }

    [Required]
    [Column("LAST_PASSWORD_CHANGE", TypeName = "DATETIME")]
    public DateTime? LastPasswordChanged { get; set; }

    //[Required]
    //[Column("PASSWORD_RESET_REQUIRED")]
    //public bool? PasswordResetRequired { get; set; }

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

