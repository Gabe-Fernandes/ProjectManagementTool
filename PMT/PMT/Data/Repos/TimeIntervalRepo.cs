using Microsoft.EntityFrameworkCore; 
using PMT.Data.RepoInterfaces; 
 
namespace PMT.Data.Models; 
 
public class TimeIntervalRepo(AppDbContext db) : ITimeIntervalRepo 
{ 
	private readonly AppDbContext _db = db;

	public async Task<List<TimeInterval>> GetAllFromStopwatch(int stopwatchId)
	{
		return await _db.TimeIntervals.Where(t => t.StopwatchId == stopwatchId).ToListAsync();
	}

	public async Task<List<TimeInterval>> GetAllFromTimeSet(int timeSetId)
	{
		return await _db.TimeIntervals.Where(t => t.TimeSetId == timeSetId).ToListAsync();
	}

	public bool Add(TimeInterval timeInterval) 
	{ 
		_db.TimeIntervals.Add(timeInterval); 
		return Save(); 
	} 
 
	public bool Update(TimeInterval timeInterval) 
	{ 
		_db.TimeIntervals.Update(timeInterval); 
		return Save(); 
	} 
 
	public bool Delete(TimeInterval timeInterval) 
	{ 
		_db.TimeIntervals.Remove(timeInterval); 
		return Save(); 
	} 
 
	public bool Save() 
	{ 
		int numSaved = _db.SaveChanges(); // returns the number of entries written to the database 
		return numSaved > 0; 
	} 
 
	public async Task<TimeInterval> GetByIdAsync(int id) 
	{ 
		return await _db.TimeIntervals.FindAsync(id); 
	} 
} 
