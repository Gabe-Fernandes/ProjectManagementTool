using PMT.Data.Models;
using PMT.Data;
using Microsoft.EntityFrameworkCore;

namespace PMTTests.Data.Repos;

public class AppUserRepoTests
{
  private readonly AppDbContext _dbContext;
  private readonly AppUserRepo _appUserRepo;

  public AppUserRepoTests()
  {
    // Dependencies
    _dbContext = GetDbContext();
    // SUT
    _appUserRepo = new AppUserRepo(_dbContext);
  }

  private AppDbContext GetDbContext()
  {
    var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
      .Options;
    var dbContext = new AppDbContext(options);
    dbContext.Database.EnsureCreated();
    if (dbContext.Users.Count() < 0)
    {
      for (int i = 0; i < 10; i++)
      {
        dbContext.Users.Add(new AppUser()
        {
          Id = (i + 1).ToString(),
          Email = "test@email.com",
          UserName = "myUsername",
          Pfp = "testPFP",
          DefaultProjId = i + 1,
          CurrentProjId = i + 1,
          Firstname = "Bob",
          Lastname = "Smith",
          StreetAddress = "test",
          City = "test",
          State = "test",
          PostalCode = "test",
          Dob = DateTime.Now,
          EmailConfirmed = true
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
    var appUser = new AppUser()
    {
      Id = "2",
      Email = "test@email.com",
      UserName = "myUsername",
      Pfp = "testPFP",
      DefaultProjId = 2,
      CurrentProjId = 2,
      Firstname = "Bob",
      Lastname = "Smith",
      StreetAddress = "test",
      City = "test",
      State = "test",
      PostalCode = "test",
      Dob = DateTime.Now,
      EmailConfirmed = true
    };
    // Act
    var result = _appUserRepo.Add(appUser);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Delete_ReturnsTrue()
  {
    // Arrange
    var appUser = new AppUser()
    {
      Id = "2",
      Email = "test@email.com",
      UserName = "myUsername",
      Pfp = "testPFP",
      DefaultProjId = 2,
      CurrentProjId = 2,
      Firstname = "Bob",
      Lastname = "Smith",
      StreetAddress = "test",
      City = "test",
      State = "test",
      PostalCode = "test",
      Dob = DateTime.Now,
      EmailConfirmed = true
    };
    _appUserRepo.Add(appUser);
    // Act
    var result = _appUserRepo.Delete(appUser);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Update_ReturnsTrue()
  {
    // Arrange
    var appUser = new AppUser()
    {
      Id = "2",
      Email = "test@email.com",
      UserName = "myUsername",
      Pfp = "testPFP",
      DefaultProjId = 2,
      CurrentProjId = 2,
      Firstname = "Bob",
      Lastname = "Smith",
      StreetAddress = "test",
      City = "test",
      State = "test",
      PostalCode = "test",
      Dob = DateTime.Now,
      EmailConfirmed = true
    };
    _appUserRepo.Add(appUser);
    // Act
    var result = _appUserRepo.Update(appUser);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Save_ReturnsBool()
  {
    // Arrange (empty)
    // Act
    var result = _appUserRepo.Save();
    // Assert
    Assert.IsType<bool>(result);
  }

  [Fact]
  public async void GetByIdAsync_ReturnsAppUserTask()
  {
    // Arrange
    var id = "1";
    // Act
    var result = _appUserRepo.GetByIdAsync(id);
    // Assert
    await Assert.IsType<Task<AppUser>>(result);
  }

  [Fact]
  public async void GetAllWithSearchFilterAsync_ReturnsIEnumerableAppUserTask()
  {
    // Arrange
    string filter = "search filter";
    // Act
    var result = _appUserRepo.GetAllWithSearchFilterAsync(filter);
    // Assert
    await Assert.IsType<Task<IEnumerable<AppUser>>>(result);
  }
}
