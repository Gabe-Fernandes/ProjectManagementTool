using System.ComponentModel.DataAnnotations;

namespace PMT.Data.Models;

public class FileStructure
{
  [Key]
  public int Id { get; set; }

  public int ProjId { get; set; }

  public string FileStructureData { get; set; }
}
