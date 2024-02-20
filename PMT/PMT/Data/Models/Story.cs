using System.ComponentModel.DataAnnotations;

namespace PMT.Data.Models;

public class Story : Issue
{
  [Key]
  public int Id { get; set; }

  [Required]
  [StringLength(45)]
  public string Title { get; set; }
}
