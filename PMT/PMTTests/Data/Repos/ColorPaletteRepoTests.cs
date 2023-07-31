using Microsoft.EntityFrameworkCore;
using PMT.Data.Models;
using PMT.Data;

namespace PMTTests.Data.Repos;

public class ColorPaletteRepoTests
{
  private readonly AppDbContext _dbContext;
  private readonly ColorPaletteRepo _colorPaletteRepo;

  public ColorPaletteRepoTests()
  {
    // Dependencies
    _dbContext = GetDbContext();
    // SUT
    _colorPaletteRepo = new ColorPaletteRepo(_dbContext);
  }

  private AppDbContext GetDbContext()
  {
    var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
      .Options;
    var dbContext = new AppDbContext(options);
    dbContext.Database.EnsureCreated();
    if (dbContext.ColorPalettes.Count() < 0)
    {
      for (int i = 0; i < 10; i++)
      {
        dbContext.ColorPalettes.Add(new ColorPalette()
        {
          Id = (i + 1),
          ProjId = i + 2,
          Colors = "test string"
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
    var colorPallete = new ColorPalette()
    {
      Id = 2,
      ProjId= 3,
      Colors = "test string"
    };
    // Act
    var result = _colorPaletteRepo.Add(colorPallete);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Delete_ReturnsTrue()
  {
    // Arrange
    var colorPalette = new ColorPalette()
    {
      Id = 2,
      ProjId = 3,
      Colors = "test string"
    };
    _colorPaletteRepo.Add(colorPalette);
    // Act
    var result = _colorPaletteRepo.Delete(colorPalette);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Update_ReturnsTrue()
  {
    // Arrange
    var colorPalette = new ColorPalette()
    {
      Id = 2,
      ProjId = 3,
      Colors = "test string"
    };
    _colorPaletteRepo.Add(colorPalette);
    // Act
    var result = _colorPaletteRepo.Update(colorPalette);
    // Assert
    Assert.True(result);
  }

  [Fact]
  public void Save_ReturnsBool()
  {
    // Arrange (empty)
    // Act
    var result = _colorPaletteRepo.Save();
    // Assert
    Assert.IsType<bool>(result);
  }

  [Fact]
  public async void GetByProjectIdAsync_ReturnsStoryTask()
  {
    // Arrange
    int projId = 3;
    // Act
    var result = _colorPaletteRepo.GetByProjectIdAsync(projId);
    // Assert
    await Assert.IsType<Task<ColorPalette>>(result);
  }
}
