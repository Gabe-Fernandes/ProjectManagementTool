using Microsoft.EntityFrameworkCore; 
using PMT.Data.RepoInterfaces;

namespace PMT.Data.Models; 
 
public class StopwatchRepo(AppDbContext db) : IStopwatchRepo 
{ 
	private readonly AppDbContext _db = db;

	public async Task<List<Stopwatch>> GetAllFromUser(string appUserId, int projId)
	{
		return await _db.Stopwatches.Where(s => s.AppUserId == appUserId && s.ProjId == projId).ToListAsync();
	}

	public bool Add(Stopwatch stopwatch) 
	{ 
		_db.Stopwatches.Add(stopwatch); 
		return Save(); 
	} 
 
	public bool Update(Stopwatch stopwatch) 
	{ 
		_db.Stopwatches.Update(stopwatch); 
		return Save(); 
	} 
 
	public bool Delete(Stopwatch stopwatch) 
	{ 
		_db.Stopwatches.Remove(stopwatch); 
		return Save(); 
	} 
 
	public bool Save() 
	{ 
		int numSaved = _db.SaveChanges(); // returns the number of entries written to the database 
		return numSaved > 0; 
	} 
 
	public async Task<Stopwatch> GetByIdAsync(int id) 
	{ 
		return await _db.Stopwatches.FindAsync(id); 
	}
} 
