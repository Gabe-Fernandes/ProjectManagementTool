using PMT.Data.Models; 
 
namespace PMT.Data.RepoInterfaces; 
 
public interface IStopwatchRepo 
{
	Task<List<Stopwatch>> GetAllFromUser(string appUserId, int projId);
	Task<Stopwatch> GetByIdAsync(int id); 
	bool Add(Stopwatch stopwatch); 
	bool Update(Stopwatch stopwatch); 
	bool Delete(Stopwatch stopwatch); 
	bool Save(); 
} 
