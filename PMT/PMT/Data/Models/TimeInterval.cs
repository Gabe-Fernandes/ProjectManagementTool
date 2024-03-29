using System.ComponentModel.DataAnnotations; 
 
namespace PMT.Data.Models; 
 
public class TimeInterval 
{ 
	[Key] 
	public int Id { get; set; } 
	public int ProjId { get; set; } 
	public int StopwatchId { get; set; } 
	public int TimeSetId { get; set; }
	public string AppUserId { get; set; } 
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public double Milliseconds { get; set; }
}
