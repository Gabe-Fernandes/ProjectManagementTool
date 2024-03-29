using Microsoft.EntityFrameworkCore; 
using PMT.Data.RepoInterfaces; 
 
namespace PMT.Data.Models; 
 
public class TimeSetRepo(AppDbContext db) : ITimeSetRepo 
{ 
	private readonly AppDbContext _db = db;

	public async Task<List<TimeSet>> GetAllFromStopwatch(int stopwatchId)
	{
		return await _db.TimeSets.Where(t => t.StopwatchId == stopwatchId).ToListAsync();
	}

	public bool Add(TimeSet timeSet) 
	{ 
		_db.TimeSets.Add(timeSet); 
		return Save(); 
	} 
 
	public bool Update(TimeSet timeSet) 
	{ 
		_db.TimeSets.Update(timeSet); 
		return Save(); 
	} 
 
	public bool Delete(TimeSet timeSet) 
	{ 
		_db.TimeSets.Remove(timeSet); 
		return Save(); 
	} 
 
	public bool Save() 
	{ 
		int numSaved = _db.SaveChanges(); // returns the number of entries written to the database 
		return numSaved > 0; 
	} 
 
	public async Task<TimeSet> GetByIdAsync(int id) 
	{ 
		return await _db.TimeSets.FindAsync(id); 
	} 
} 
