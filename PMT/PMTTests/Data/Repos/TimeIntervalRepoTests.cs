using Microsoft.EntityFrameworkCore; 
using PMT.Data.Models; 
using PMT.Data;
namespace PMTTests.Data.Repos;
public class TimeIntervalRepoTests
{ 
	private readonly AppDbContext _dbContext; 
	private readonly TimeIntervalRepo _timeIntervalRepo; 
 
	public TimeIntervalRepoTests() 
	{ 
		// Dependencies 
		_dbContext = GetDbContext(); 
		// SUT 
		_timeIntervalRepo = new TimeIntervalRepo(_dbContext); 
	} 
 
	private AppDbContext GetDbContext() 
	{ 
		var options = new DbContextOptionsBuilder<AppDbContext>() 
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options; 
		var dbContext = new AppDbContext(options); 
		dbContext.Database.EnsureCreated(); 
		if (dbContext.TimeIntervals.Count() < 0) 
		{ 
			for (int i = 0; i < 10; i++) 
			{ 
				dbContext.TimeIntervals.Add(new TimeInterval() 
				{ 
					Id = (i + 1), 
					ProjId = 2, 
					StopwatchId = 2, 
					TimeSetId = 2, 
					AppUserId = "test string", 
					StartDate = DateTime.Now, 
					EndDate = DateTime.Now,
					Milliseconds = 2, 
				}); 
				dbContext.SaveChangesAsync(); 
			} 
		} 
		return dbContext; 
	}

	[Fact]
	public async void GetAllFromStopwatch_ReturnsListTimeIntervalTask()
	{
		// Arrange
		int stopwatchId = 2;
		// Act
		var result = _timeIntervalRepo.GetAllFromStopwatch(stopwatchId);
		//Assert
		await Assert.IsType<Task<List<TimeInterval>>>(result);
	}

	[Fact]
	public async void GetAllFromTimeSet_ReturnsListTimeIntervalTask()
	{
		// Arrange
		int timeSetId = 2;
		// Act
		var result = _timeIntervalRepo.GetAllFromTimeSet(timeSetId);
		//Assert
		await Assert.IsType<Task<List<TimeInterval>>>(result);
	}

	[Fact] 
	public void Add_ReturnsTrue() 
	{ 
		// Arrange 
		var timeInterval = new TimeInterval() 
		{ 
		Id = 2, 
		ProjId = 2, 
		StopwatchId = 2, 
		TimeSetId = 2, 
		AppUserId = "test string", 
		StartDate = DateTime.Now, 
		EndDate = DateTime.Now,
		Milliseconds = 2, 
		}; 
		// Act 
		var result = _timeIntervalRepo.Add(timeInterval); 
		// Assert 
		Assert.True(result); 
	} 
 
	[Fact] 
	public void Delete_ReturnsTrue() 
	{ 
		// Arrange 
		var timeInterval = new TimeInterval() 
		{ 
		Id = 2, 
		ProjId = 2, 
		StopwatchId = 2, 
		TimeSetId = 2, 
		AppUserId = "test string", 
		StartDate = DateTime.Now, 
		EndDate = DateTime.Now,
		Milliseconds = 2, 
		}; 
		// Act 
		var result = _timeIntervalRepo.Delete(timeInterval); 
		// Assert 
		Assert.True(result); 
	} 
 
	[Fact] 
	public void Update_ReturnsTrue() 
	{ 
		// Arrange 
		var timeInterval = new TimeInterval() 
		{ 
		Id = 2, 
		ProjId = 2, 
		StopwatchId = 2, 
		TimeSetId = 2, 
		AppUserId = "test string", 
		StartDate = DateTime.Now, 
		EndDate = DateTime.Now,
		Milliseconds = 2, 
		}; 
		// Act 
		var result = _timeIntervalRepo.Update(timeInterval); 
		// Assert 
		Assert.True(result); 
	} 
 
	[Fact] 
	public void Save_ReturnsBool() 
	{ 
		// Arrange (empty) 
		// Act 
		var result = _timeIntervalRepo.Save(); 
		// Assert 
		Assert.IsType<bool>(result); 
	} 
 
	[Fact] 
	public async void GetByIdAsync_ReturnsTimeIntervalTask() 
	{ 
		// Arrange 
		var id = 2; 
		// Act 
		var result = await _timeIntervalRepo.GetByIdAsync(id); 
		// Assert 
		await Assert.IsType<Task<TimeInterval>>(result); 
	} 
} 
