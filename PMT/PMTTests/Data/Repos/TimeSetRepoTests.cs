using Microsoft.EntityFrameworkCore; 
using PMT.Data.Models; 
using PMT.Data;
namespace PMTTests.Data.Repos;
public class TimeSetRepoTests
{ 
	private readonly AppDbContext _dbContext; 
	private readonly TimeSetRepo _timeSetRepo; 
 
	public TimeSetRepoTests() 
	{ 
		// Dependencies 
		_dbContext = GetDbContext(); 
		// SUT 
		_timeSetRepo = new TimeSetRepo(_dbContext); 
	} 
 
	private AppDbContext GetDbContext() 
	{ 
		var options = new DbContextOptionsBuilder<AppDbContext>() 
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options; 
		var dbContext = new AppDbContext(options); 
		dbContext.Database.EnsureCreated(); 
		if (dbContext.TimeSets.Count() < 0) 
		{ 
			for (int i = 0; i < 10; i++) 
			{ 
				dbContext.TimeSets.Add(new TimeSet() 
				{ 
					Id = (i + 1), 
					ProjId = 2, 
					AppUserId = "test string", 
					StopwatchId = 2,
					Milliseconds = 2, 
				}); 
				dbContext.SaveChangesAsync(); 
			} 
		} 
		return dbContext; 
	}

	[Fact]
	public async void GetAllFromStopwatch_ReturnsListTimeSetTask()
	{
		// Arrange
		int stopwatchId = 2;
		// Act
		var result = _timeSetRepo.GetAllFromStopwatch(stopwatchId);
		//Assert
		await Assert.IsType<Task<List<TimeSet>>>(result);
	}

	[Fact] 
	public void Add_ReturnsTrue() 
	{ 
		// Arrange 
		var timeSet = new TimeSet() 
		{ 
		Id = 2, 
		ProjId = 2, 
		AppUserId = "test string", 
		StopwatchId = 2,
		Milliseconds = 2, 
		}; 
		// Act 
		var result = _timeSetRepo.Add(timeSet); 
		// Assert 
		Assert.True(result); 
	} 
 
	[Fact] 
	public void Delete_ReturnsTrue() 
	{ 
		// Arrange 
		var timeSet = new TimeSet() 
		{ 
		Id = 2, 
		ProjId = 2, 
		AppUserId = "test string", 
		StopwatchId = 2,
		Milliseconds = 2, 
		}; 
		// Act 
		var result = _timeSetRepo.Delete(timeSet); 
		// Assert 
		Assert.True(result); 
	} 
 
	[Fact] 
	public void Update_ReturnsTrue() 
	{ 
		// Arrange 
		var timeSet = new TimeSet() 
		{ 
		Id = 2, 
		ProjId = 2, 
		AppUserId = "test string", 
		StopwatchId = 2,
		Milliseconds = 2, 
		}; 
		// Act 
		var result = _timeSetRepo.Update(timeSet); 
		// Assert 
		Assert.True(result); 
	} 
 
	[Fact] 
	public void Save_ReturnsBool() 
	{ 
		// Arrange (empty) 
		// Act 
		var result = _timeSetRepo.Save(); 
		// Assert 
		Assert.IsType<bool>(result); 
	} 
 
	[Fact] 
	public async void GetByIdAsync_ReturnsTimeSetTask() 
	{ 
		// Arrange 
		var id = 2; 
		// Act 
		var result = await _timeSetRepo.GetByIdAsync(id); 
		// Assert 
		await Assert.IsType<Task<TimeSet>>(result); 
	} 
} 
