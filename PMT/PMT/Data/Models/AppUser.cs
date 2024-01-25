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

  [StringLength(40)]
  public string StreetAddress { get; set; }

  [StringLength(40)]
  public string City { get; set; }

  [StringLength(40)]
  public string State { get; set; }

  [StringLength(40)]
  [DataType(DataType.PostalCode)]
  public string PostalCode { get; set; }

  [DataType(DataType.Date)]
  public DateTime Dob { get; set; }

  public string Pfp { get; set; }

  public int DefaultProjId { get; set; }
}
