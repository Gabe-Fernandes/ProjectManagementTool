using System.ComponentModel.DataAnnotations;

namespace PMT.Data.Models;
    
public class Project_AppUser
{
  [Key]
  public int Id { get; set; }

  public int ProjId { get; set; }
 
  public string AppUserId { get; set; }
  
  public bool Approved { get; set; }
  
  public string Role { get; set; }
}
