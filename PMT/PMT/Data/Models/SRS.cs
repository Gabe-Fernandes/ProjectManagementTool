using System.ComponentModel.DataAnnotations;

namespace PMT.Data.Models;

public class SRS
{
  [Key]
  public int Id { get; set; }

  public int ProjId { get; set; }

  [StringLength(35)]
  public string DomainName { get; set; }

  [StringLength(100)]
  public string TargetDemographic { get; set; }

  [StringLength(600)]
  public string Description { get; set; }
}
