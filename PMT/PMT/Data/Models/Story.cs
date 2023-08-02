using System.ComponentModel.DataAnnotations;

namespace PMT.Data.Models;

public class Story : Issue
{
  [Key]
  public int Id { get; set; }

  [Required]
  [StringLength(30)]
  public string Title { get; set; }
}
