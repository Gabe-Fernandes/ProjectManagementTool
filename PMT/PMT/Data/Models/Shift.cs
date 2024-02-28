using System.ComponentModel.DataAnnotations; 
 
namespace PMT.Data.Models; 
 
public class Shift 
{ 
	[Key] 
	public int Id { get; set; } 
	public int ProjId { get; set; } 
	public int StopwatchId { get; set; } 
	public string AppUserId { get; set; } 
	public DateTime Date { get; set; } 
	public DateTime ClockIn { get; set; } 
	public DateTime ClockOut { get; set; } 
	public int Hours { get; set; } 
}
