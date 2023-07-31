using System.ComponentModel.DataAnnotations;

namespace PMT.Data.Models;

public class ColorPalette
{
  [Key]
  public int Id { get; set; }

  public int ProjId { get; set; }

  [Required]
  public string Colors { get; set; }
}
