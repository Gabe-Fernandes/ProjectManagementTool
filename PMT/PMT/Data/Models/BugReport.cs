using System.ComponentModel.DataAnnotations;

namespace PMT.Data.Models;

public class BugReport : Issue
{
  [Key]
  public int Id { get; set; }

  [Required]
  [StringLength(350)]
  public string RecreateIssue { get; set; }

  [StringLength(1000)]
  public string AttemptedSolutions { get; set; }

  [StringLength(1000)]
  public string SuccessfulSolution { get; set; }

  public string Priority { get; set; }
}
