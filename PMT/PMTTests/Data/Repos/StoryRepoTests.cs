using Microsoft.EntityFrameworkCore;
using PMT.Data;
using PMT.Data.Models;

namespace PMTTests.Data.Repos;

public class StoryRepoTests
{
  private readonly AppDbContext _dbContext;
  private readonly StoryRepo _storyRepo;

  public StoryRepoTests()
  {
    // Dependencies
    _dbContext = GetDbContext();
    // SUT
    _storyRepo = new StoryRepo(_dbContext);
  }

  private AppDbContext GetDbContext()
  {
    var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
      .Options;
    var dbContext = new AppDbContext(options);
    dbContext.Database.EnsureCreated();
    if (dbContext.Stories.Count() < 0)
    {
      for (int i = 0; i < 10; i++)
      {
        dbContext.Stories.Add(new Story()
        {
          Id = (i + 1),
          DateCreated = DateTime.Now,
          DateResolved = DateTime.Now,
          DueDate = DateTime.Now,
          Status = "test string",
          Description = "test string",
          Title = "test string"
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
    var story = new Story()
    {
      Id = 2,
      DateCreated = DateTime.Now,
      DateResolved = DateTime.Now,
      DueDate = DateTime.Now,
      Status = "test string",
      Description = "test string",
      Title = "test string"
    };
    // Act
    var result = _storyRepo.Add(story);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Delete_ReturnsTrue()
  {
    // Arrange
    var story = new Story()
    {
      Id = 2,
      DateCreated = DateTime.Now,
      DateResolved = DateTime.Now,
      DueDate = DateTime.Now,
      Status = "test string",
      Description = "test string",
      Title = "test string"
    };
    _storyRepo.Add(story);
    // Act
    var result = _storyRepo.Delete(story);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Update_ReturnsTrue()
  {
    // Arrange
    var story = new Story()
    {
      Id = 2,
      DateCreated = DateTime.Now,
      DateResolved = DateTime.Now,
      DueDate = DateTime.Now,
      Status = "test string",
      Description = "test string",
      Title = "test string"
    };
    _storyRepo.Add(story);
    // Act
    var result = _storyRepo.Update(story);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Save_ReturnsBool()
  {
    // Arrange (empty)
    // Act
    var result = _storyRepo.Save();
    // Assert
    Assert.IsType<bool>(result);
  }

  [Fact]
  public async void GetByIdAsync_ReturnsStoryTask()
  {
    // Arrange
    var id = 2;
    // Act
    var result = _storyRepo.GetByIdAsync(id);
    // Assert
    await Assert.IsType<Task<Story>>(result);
  }

  [Fact]
  public async void GetAllWithSearchFilterAsync_ReturnsIEnumerableStoryTask()
  {
    // Arrange
    int projId = 3;
    string filterString = "abc";
    // Act
    var result = _storyRepo.GetAllWithSearchFilterAsync(projId, filterString);
    // Assert
    await Assert.IsType<Task<IEnumerable<Story>>>(result);
  }

  [Fact]
  public async void GetAllUnresolvedStoriesWithSearchFilterAsync_ReturnsIEnumerableStoryTask()
  {
    // Arrange
    int projId = 3;
    string filterString = "abc";
    // Act
    var result = _storyRepo.GetAllUnresolvedStoriesWithSearchFilterAsync(projId, filterString);
    // Assert
    await Assert.IsType<Task<IEnumerable<Story>>>(result);
  }
  
  [Fact]
  public async void GetAllFromUserAsync_ReturnsIEnumerableStoryTask()
  {
    // Arrange
    int projId = 3;
    string appUserId = "abc";
    // Act
    var result = _storyRepo.GetAllFromUserAsync(projId, appUserId);
    // Assert
    await Assert.IsType<Task<IEnumerable<Story>>>(result);
  }

  [Fact]
  public async void GetAllUnresolvedStoriesFromUserAsync_ReturnsIEnumerableStoryTask()
  {
    // Arrange
    int projId = 3;
    string appUserId = "abc";
    // Act
    var result = _storyRepo.GetAllUnresolvedStoriesFromUserAsync(projId, appUserId);
    // Assert
    await Assert.IsType<Task<IEnumerable<Story>>>(result);
  }
}
