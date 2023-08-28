using System.ComponentModel.DataAnnotations;

namespace PMT.Views.Account;

public class AccountVM
{
  [Required]
  public string Email { get; set; }
	public string ConfirmPassword { get; set; }
	[Required]
	public string Password { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string Code { get; set; }
}
