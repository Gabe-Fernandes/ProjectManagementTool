using PMT.Data.Models; 
 
namespace PMT.Data.RepoInterfaces; 
 
public interface ITimeIntervalRepo 
{
	Task<List<TimeInterval>> GetAllFromStopwatch(int stopwatchId);
	Task<List<TimeInterval>> GetAllFromTimeSet(int timeSetId);
	Task<TimeInterval> GetByIdAsync(int id); 
	bool Add(TimeInterval timeInterval); 
	bool Update(TimeInterval timeInterval); 
	bool Delete(TimeInterval timeInterval); 
	bool Save(); 
} 
