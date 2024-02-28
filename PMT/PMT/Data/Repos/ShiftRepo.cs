using Microsoft.EntityFrameworkCore; 
using PMT.Data.RepoInterfaces;

namespace PMT.Data.Models; 
 
public class ShiftRepo(AppDbContext db) : IShiftRepo 
{ 
	private readonly AppDbContext _db = db;

	public async Task<List<Shift>> GetAllFromStopwatch(int stopwatchId)
	{
		return await _db.Shifts.Where(s => s.StopwatchId == stopwatchId).ToListAsync();
	}

	public bool Add(Shift shift) 
	{ 
		_db.Shifts.Add(shift); 
		return Save(); 
	} 
 
	public bool Update(Shift shift) 
	{ 
		_db.Shifts.Update(shift); 
		return Save(); 
	} 
 
	public bool Delete(Shift shift) 
	{ 
		_db.Shifts.Remove(shift); 
		return Save(); 
	} 
 
	public bool Save() 
	{ 
		int numSaved = _db.SaveChanges(); // returns the number of entries written to the database 
		return numSaved > 0; 
	} 
 
	public async Task<Shift> GetByIdAsync(int id) 
	{ 
		return await _db.Shifts.FindAsync(id); 
	}
} 
