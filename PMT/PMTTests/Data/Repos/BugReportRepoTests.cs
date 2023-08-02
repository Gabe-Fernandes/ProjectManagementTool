using Microsoft.EntityFrameworkCore;
using PMT.Data.Models;
using PMT.Data;

namespace PMTTests.Data.Repos;

public class BugReportRepoTests
{
  private readonly AppDbContext _dbContext;
  private readonly BugReportRepo _bugReportRepo;

  public BugReportRepoTests()
  {
    // Dependencies
    _dbContext = GetDbContext();
    // SUT
    _bugReportRepo = new BugReportRepo(_dbContext);
  }

  private AppDbContext GetDbContext()
  {
    var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
      .Options;
    var dbContext = new AppDbContext(options);
    dbContext.Database.EnsureCreated();
    if (dbContext.BugReports.Count() < 0)
    {
      for (int i = 0; i < 10; i++)
      {
        dbContext.BugReports.Add(new BugReport()
        {
          Id = (i + 1),
          RecreateIssue = "test string",
          AttemptedSolutions = "test string",
          SuccessfulSolution = "test string",
          DateCreated = DateTime.Now,
          DateResolved = DateTime.Now,
          DueDate = DateTime.Now,
          Status = "test string",
          Priority = "test string",
          Description = "test string"
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
    var bugReport = new BugReport()
    {
      Id = 2,
      RecreateIssue = "test string",
      AttemptedSolutions = "test string",
      SuccessfulSolution = "test string",
      DateCreated = DateTime.Now,
      DateResolved = DateTime.Now,
      DueDate = DateTime.Now,
      Status = "test string",
      Priority = "test string",
      Description = "test string"
    };
    // Act
    var result = _bugReportRepo.Add(bugReport);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Delete_ReturnsTrue()
  {
    // Arrange
    var bugReport = new BugReport()
    {
      Id = 2,
      RecreateIssue = "test string",
      AttemptedSolutions = "test string",
      SuccessfulSolution = "test string",
      DateCreated = DateTime.Now,
      DateResolved = DateTime.Now,
      DueDate = DateTime.Now,
      Status = "test string",
      Priority = "test string",
      Description = "test string"
    };
    _bugReportRepo.Add(bugReport);
    // Act
    var result = _bugReportRepo.Delete(bugReport);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Update_ReturnsTrue()
  {
    // Arrange
    var bugReport = new BugReport()
    {
      Id = 2,
      RecreateIssue = "test string",
      AttemptedSolutions = "test string",
      SuccessfulSolution = "test string",
      DateCreated = DateTime.Now,
      DateResolved = DateTime.Now,
      DueDate = DateTime.Now,
      Status = "test string",
      Priority = "test string",
      Description = "test string"
    };
    _bugReportRepo.Add(bugReport);
    // Act
    var result = _bugReportRepo.Update(bugReport);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Save_ReturnsBool()
  {
    // Arrange (empty)
    // Act
    var result = _bugReportRepo.Save();
    // Assert
    Assert.IsType<bool>(result);
  }

  [Fact]
  public async void GetByIdAsync_ReturnsBugReportTask()
  {
    // Arrange
    var id = 2;
    // Act
    var result = _bugReportRepo.GetByIdAsync(id);
    // Assert
    await Assert.IsType<Task<BugReport>>(result);
  }

  [Fact]
  public async void GetAllAsync_ReturnsIEnumerableBugReportTask()
  {
    // Arrange
    int projId = 3;
    // Act
    var result = _bugReportRepo.GetAllAsync(projId);
    // Assert
    await Assert.IsType<Task<IEnumerable<BugReport>>>(result);
  }

  [Fact]
  public async void GetAllUnresolvedReportsAsync_ReturnsIEnumerableBugReportTask()
  {
    // Arrange
    int projId = 3;
    // Act
    var result = _bugReportRepo.GetAllUnresolvedReportsAsync(projId);
    // Assert
    await Assert.IsType<Task<IEnumerable<BugReport>>>(result);
  }

  [Fact]
  public async void GetAllFromUserAsync_ReturnsIEnumerableBugReportTask()
  {
    // Arrange
    int projId = 3;
    string appUserId = "abc";
    // Act
    var result = _bugReportRepo.GetAllFromUserAsync(projId, appUserId);
    // Assert
    await Assert.IsType<Task<IEnumerable<BugReport>>>(result);
  }

  [Fact]
  public async void GetAllUnresolvedReportsFromUserAsync_ReturnsIEnumerableBugReportTask()
  {
    // Arrange
    int projId = 3;
    string appUserId = "abc";
    // Act
    var result = _bugReportRepo.GetAllUnresolvedReportsFromUserAsync(projId, appUserId);
    // Assert
    await Assert.IsType<Task<IEnumerable<BugReport>>>(result);
  }
}
