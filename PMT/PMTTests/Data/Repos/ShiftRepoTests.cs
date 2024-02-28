using Microsoft.EntityFrameworkCore; 
using PMT.Data.Models; 
using PMT.Data; 
 
namespace PMTTests.Data.Repos; 
 
public class ShiftRepoTests 
{ 
	private readonly AppDbContext _dbContext; 
	private readonly ShiftRepo _shiftRepo; 
 
	public ShiftRepoTests() 
	{ 
		// Dependencies 
		_dbContext = GetDbContext(); 
		// SUT 
		_shiftRepo = new ShiftRepo(_dbContext); 
	} 
 
	private AppDbContext GetDbContext() 
	{ 
		var options = new DbContextOptionsBuilder<AppDbContext>() 
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options; 
		var dbContext = new AppDbContext(options); 
		dbContext.Database.EnsureCreated(); 
		if (dbContext.Shifts.Count() < 0) 
		{ 
			for (int i = 0; i < 10; i++) 
			{ 
				dbContext.Shifts.Add(new Shift() 
				{ 
					Id = (i + 1), 
					ProjId = 2, 
					StopwatchId = 2, 
					AppUserId = "test string", 
					Date = DateTime.Now, 
					ClockIn = DateTime.Now, 
					ClockOut = DateTime.Now, 
					Hours = 2, 
				}); 
				dbContext.SaveChangesAsync(); 
			} 
		} 
		return dbContext; 
	} 
 
	[Fact] 
	public void Add_ReturnsTrue() 
	{ 
		// Arrange 
		var shift = new Shift() 
		{ 
		Id = 2, 
		ProjId = 2, 
		StopwatchId = 2, 
		AppUserId = "test string", 
		Date = DateTime.Now, 
		ClockIn = DateTime.Now, 
		ClockOut = DateTime.Now, 
		Hours = 2, 
		}; 
		// Act 
		var result = _shiftRepo.Add(shift); 
		// Assert 
		Assert.True(result); 
	} 
 
	[Fact] 
	public void Delete_ReturnsTrue() 
	{ 
		// Arrange 
		var shift = new Shift() 
		{ 
		Id = 2, 
		ProjId = 2, 
		StopwatchId = 2, 
		AppUserId = "test string", 
		Date = DateTime.Now, 
		ClockIn = DateTime.Now, 
		ClockOut = DateTime.Now, 
		Hours = 2, 
		}; 
		// Act 
		var result = _shiftRepo.Delete(shift); 
		// Assert 
		Assert.True(result); 
	} 
 
	[Fact] 
	public void Update_ReturnsTrue() 
	{ 
		// Arrange 
		var shift = new Shift() 
		{ 
		Id = 2, 
		ProjId = 2, 
		StopwatchId = 2, 
		AppUserId = "test string", 
		Date = DateTime.Now, 
		ClockIn = DateTime.Now, 
		ClockOut = DateTime.Now, 
		Hours = 2, 
		}; 
		// Act 
		var result = _shiftRepo.Update(shift); 
		// Assert 
		Assert.True(result); 
	} 
 
	[Fact] 
	public void Save_ReturnsBool() 
	{ 
		// Arrange (empty) 
		// Act 
		var result = _shiftRepo.Save(); 
		// Assert 
		Assert.IsType<bool>(result); 
	} 
 
	[Fact] 
	public async void GetByIdAsync_ReturnsShiftTask() 
	{ 
		// Arrange 
		var id = 2; 
		// Act 
		var result = await _shiftRepo.GetByIdAsync(id); 
		// Assert 
		await Assert.IsType<Task<Shift>>(result); 
	} 
} 
