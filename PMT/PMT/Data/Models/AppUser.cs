using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PMT.Data.Models;

public class AppUser : IdentityUser
{
  [Required]
  [StringLength(20)]
  [Display(Name = "First Name")]
  public string Firstname { get; set; }

  [Required]
  [StringLength(20)]
  [Display(Name = "Last Name")]
  public string Lastname { get; set; }

  public string Pfp { get; set; }

  public int DefaultProjId { get; set; }
}
