using System.ComponentModel.DataAnnotations;
using TecCore.Utilities;

namespace TecCore.Models.InfoBoard;

public class AdminUser : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = "admin";

    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; set; } =
        EncryptionHelpers.HashString("password"); // Store hashed password, not plaintext
}