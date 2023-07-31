using System.ComponentModel.DataAnnotations;

namespace PMT.Data.Models;

public class TechStack
{
  [Key]
  public int Id { get; set; }

  public int ProjId { get; set; }

  [StringLength(30)]
  public string SourceControl { get; set; }

  [StringLength(30)]
  public string BackendFramework { get; set; }

  [StringLength(30)]
  public string BackendLanguage { get; set; }

  [StringLength(30)]
  public string FrontendFramework { get; set; }

  [StringLength(30)]
  public string FrontendLanguage { get; set; }

  [StringLength(30)]
  public string Styling { get; set; }

  [StringLength(30)]
  public string Database { get; set; }

  [StringLength(30)]
  public string ORM { get; set; }

  [StringLength(30)]
  public string UIDesign { get; set; }

  [StringLength(30)]
  public string Deployment { get; set; }
}
