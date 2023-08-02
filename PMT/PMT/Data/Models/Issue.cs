using System.ComponentModel.DataAnnotations;

namespace PMT.Data.Models;

public class Issue
{
  [Display(Name = "Date Created")]
  public DateTime? DateCreated { get; set; }

  [Display(Name = "Date Resolved")]
  public DateTime? DateResolved { get; set; }

  [Display(Name = "Due Date")]
  public DateTime? DueDate { get; set; }

  [Required]
  [StringLength(600)]
  public string Description { get; set; }
  
  public string Status { get; set; }

  public int ProjId { get; set; }
}
