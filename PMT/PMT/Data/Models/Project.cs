using System.ComponentModel.DataAnnotations;

namespace PMT.Data.Models;

public class Project
{
  [Key]
  public int Id { get; set; }

  [Required]
  [StringLength(30)]
  public string Name { get; set; }

  public DateTime StartDate { get; set; }

  public DateTime DueDate { get; set; }

  public string JoinCode { get; set; }
}
