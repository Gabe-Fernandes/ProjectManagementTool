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
  public string StreetAddress { get; set; }
  public string City { get; set; }
  public string State { get; set; }
  [DataType(DataType.PostalCode)]
  public string PostalCode { get; set; }
  [DataType(DataType.Date)]
  public DateTime Dob { get; set; }
  [DataType(DataType.PhoneNumber)]
  public string CellPhone { get; set; }
}
