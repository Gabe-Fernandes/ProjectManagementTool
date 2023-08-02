using System.ComponentModel.DataAnnotations;

namespace PMT.Data.Models;

public class ModelPlanning
{
  [Key]
  public int Id { get; set; }

  public int ProjId { get; set; }

  public string Models { get; set; }

  public string Properties { get; set; }

  public string Validations { get; set; }
}
