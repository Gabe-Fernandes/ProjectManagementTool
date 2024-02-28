using PMT.Data.Models; 
 
namespace PMT.Data.RepoInterfaces; 
 
public interface IShiftRepo 
{ 
	Task<Shift> GetByIdAsync(int id); 
	bool Add(Shift shift); 
	bool Update(Shift shift); 
	bool Delete(Shift shift); 
	bool Save(); 
} 
