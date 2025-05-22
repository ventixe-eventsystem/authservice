using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;
public class UserEntity : IdentityUser
{
  [ProtectedPersonalData]
  [Required]
  [Column(TypeName = "nvarchar(50)")]
  public string FirstName { get; set; } = null!;

  [ProtectedPersonalData]
  [Required]
  [Column(TypeName = "nvarchar(50)")]
  public string LastName { get; set; } = null!;
}
