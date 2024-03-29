using PMT.Data.Models; 
 
namespace PMT.Data.RepoInterfaces; 
 
public interface ITimeSetRepo 
{
	Task<List<TimeSet>> GetAllFromStopwatch(int stopwatchId);
	Task<TimeSet> GetByIdAsync(int id); 
	bool Add(TimeSet timeSet); 
	bool Update(TimeSet timeSet); 
	bool Delete(TimeSet timeSet); 
	bool Save(); 
} 
