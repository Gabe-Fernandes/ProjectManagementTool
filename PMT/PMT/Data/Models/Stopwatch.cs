using System.ComponentModel.DataAnnotations; 
 
namespace PMT.Data.Models; 
 
public class Stopwatch 
{ 
	[Key] 
	public int Id { get; set; } 
	public int ProjId { get; set; } 
	public string AppUserId { get; set; } 
	public string Name { get; set; } 
	public double Milliseconds { get; set; }
}
