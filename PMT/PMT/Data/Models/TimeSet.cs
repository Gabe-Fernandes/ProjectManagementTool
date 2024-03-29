using System.ComponentModel.DataAnnotations; 
 
namespace PMT.Data.Models; 
 
public class TimeSet 
{ 
	[Key] 
	public int Id { get; set; } 
	public int ProjId { get; set; } 
	public string AppUserId { get; set; } 
	public int StopwatchId { get; set; } 
	public double Milliseconds { get; set; } 
}
