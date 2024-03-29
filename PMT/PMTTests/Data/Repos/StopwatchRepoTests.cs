using Microsoft.EntityFrameworkCore; 
using PMT.Data.Models; 
using PMT.Data; 
 
namespace PMTTests.Data.Repos; 
 
public class StopwatchRepoTests 
{ 
	private readonly AppDbContext _dbContext; 
	private readonly StopwatchRepo _stopwatchRepo; 
 
	public StopwatchRepoTests() 
	{ 
		// Dependencies 
		_dbContext = GetDbContext(); 
		// SUT 
		_stopwatchRepo = new StopwatchRepo(_dbContext); 
	} 
 
	private AppDbContext GetDbContext() 
	{ 
		var options = new DbContextOptionsBuilder<AppDbContext>() 
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options; 
		var dbContext = new AppDbContext(options); 
		dbContext.Database.EnsureCreated(); 
		if (dbContext.Stopwatches.Count() < 0) 
		{ 
			for (int i = 0; i < 10; i++) 
			{ 
				dbContext.Stopwatches.Add(new Stopwatch() 
				{ 
					Id = (i + 1), 
					ProjId = 2, 
					AppUserId = "test string", 
					Name = "test string",
					Milliseconds = 2
				}); 
				dbContext.SaveChangesAsync(); 
			} 
		} 
		return dbContext; 
	}

	[Fact]
	public async void GetAllFromUser_ReturnsListStopwatchTask()
	{
		// Arrange
		string appUserId = "test";
		int projId = 2;
		// Act
		var result = _stopwatchRepo.GetAllFromUser(appUserId, projId);
		//Assert
		await Assert.IsType<Task<List<Stopwatch>>>(result);
	}

	[Fact] 
	public void Add_ReturnsTrue() 
	{ 
		// Arrange 
		var stopwatch = new Stopwatch() 
		{ 
		Id = 2, 
		ProjId = 2, 
		AppUserId = "test string", 
		Name = "test string",
		Milliseconds = 2
		}; 
		// Act 
		var result = _stopwatchRepo.Add(stopwatch); 
		// Assert 
		Assert.True(result); 
	} 
 
	[Fact] 
	public void Delete_ReturnsTrue() 
	{ 
		// Arrange 
		var stopwatch = new Stopwatch() 
		{ 
		Id = 2, 
		ProjId = 2, 
		AppUserId = "test string", 
		Name = "test string",
		Milliseconds = 2
		}; 
		// Act 
		var result = _stopwatchRepo.Delete(stopwatch); 
		// Assert 
		Assert.True(result); 
	} 
 
	[Fact] 
	public void Update_ReturnsTrue() 
	{ 
		// Arrange 
		var stopwatch = new Stopwatch() 
		{ 
		Id = 2, 
		ProjId = 2, 
		AppUserId = "test string", 
		Name = "test string",
		Milliseconds = 2
		}; 
		// Act 
		var result = _stopwatchRepo.Update(stopwatch); 
		// Assert 
		Assert.True(result); 
	} 
 
	[Fact] 
	public void Save_ReturnsBool() 
	{ 
		// Arrange (empty) 
		// Act 
		var result = _stopwatchRepo.Save(); 
		// Assert 
		Assert.IsType<bool>(result); 
	} 
 
	[Fact] 
	public async void GetByIdAsync_ReturnsStopwatchTask() 
	{ 
		// Arrange 
		var id = 2; 
		// Act 
		var result = await _stopwatchRepo.GetByIdAsync(id); 
		// Assert 
		await Assert.IsType<Task<Stopwatch>>(result); 
	} 
} 
