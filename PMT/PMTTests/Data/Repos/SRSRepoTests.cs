using Microsoft.EntityFrameworkCore;
using PMT.Data.Models;
using PMT.Data;

namespace PMTTests.Data.Repos;

public class SRSRepoTests
{
  private readonly AppDbContext _dbContext;
  private readonly SRSRepo _SRSRepo;

  public SRSRepoTests()
  {
    // Dependencies
    _dbContext = GetDbContext();
    // SUT
    _SRSRepo = new SRSRepo(_dbContext);
  }

  private AppDbContext GetDbContext()
  {
    var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
      .Options;
    var dbContext = new AppDbContext(options);
    dbContext.Database.EnsureCreated();
    if (dbContext.SRSs.Count() < 0)
    {
      for (int i = 0; i < 10; i++)
      {
        dbContext.SRSs.Add(new SRS()
        {
          Id = (i + 1),
          ProjId = i + 2,
          DomainName = "test string",
          TargetDemographic = "test string",
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
    var SRS = new SRS()
    {
      Id = 2,
      ProjId = 3,
      DomainName = "test string",
      TargetDemographic = "test string",
      Description = "test string"
    };
    // Act
    var result = _SRSRepo.Add(SRS);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Delete_ReturnsTrue()
  {
    // Arrange
    var SRS = new SRS()
    {
      Id = 2,
      ProjId = 3,
      DomainName = "test string",
      TargetDemographic = "test string",
      Description = "test string"
    };
    _SRSRepo.Add(SRS);
    // Act
    var result = _SRSRepo.Delete(SRS);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Update_ReturnsTrue()
  {
    // Arrange
    var SRS = new SRS()
    {
      Id = 2,
      ProjId = 3,
      DomainName = "test string",
      TargetDemographic = "test string",
      Description = "test string"
    };
    _SRSRepo.Add(SRS);
    // Act
    var result = _SRSRepo.Update(SRS);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Save_ReturnsBool()
  {
    // Arrange (empty)
    // Act
    var result = _SRSRepo.Save();
    // Assert
    Assert.IsType<bool>(result);
  }

  [Fact]
  public async void GetByProjectIdAsync_ReturnsSRSTask()
  {
    // Arrange
    int projId = 3;
    // Act
    var result = _SRSRepo.GetByProjectIdAsync(projId);
    // Assert
    await Assert.IsType<Task<SRS>>(result);
  }
}
