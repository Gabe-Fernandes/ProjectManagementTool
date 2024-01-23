using CliWrap;
using CliWrap.Buffered;

namespace PMT.Services.PowerShell;

public static class BackendScaffold
{
  private static readonly List<string> TestNames = ["test0", "test1", "test2", "test3"];
  private static readonly string ProjectName = "SeparationTool";
  private static readonly string testFilePath = "C:\\dev\\separationtoolrepo\\separation\\separation\\data\\repos";

  public async static Task RunBatch()
  {
    for (int i = 0; i < TestNames.Count; i++)
    {
      string output =
        $"using Microsoft.EntityFrameworkCore;\r\n" +
        $"using {ProjectName}.Data.RepoInterfaces;\r\n\r\n" +
        $"namespace {ProjectName}.Data.Models;\r\n\r\n" +
        $"public class{TestNames[i]}Repo : I{TestNames[i]}Repo(AppDbContext db)\r\n" +
        "{\r\n" +
        "private readonly AppDbContext _db = db;\r\n\r\n" +
        $"public bool Add({TestNames[i]} {TestNames[i]})\r\n " +
        "{\r\n" +
        $"_db.{TestNames[i]}s.Add({TestNames[i]});\r\n" +
        "return Save();\r\n" +
        "}\r\n\r\n" +
        $"public bool Delete({TestNames[i]} {TestNames[i]})\r\n" +
        "{\r\n" +
        $"_db.{TestNames[i]}s.Remove({TestNames[i]});\r\n" +
        "return Save();\r\n" +
        "}\r\n\r\n" +
        $"public bool Update({TestNames[i]} {TestNames[i]})\r\n" +
        "{\r\n" +
        $"_db.{TestNames[i]}s.Update({TestNames[i]});\r\n" +
        "return Save();\r\n" +
        "}";
      await UseWrap($"Write-Output '{output}' > {TestNames[i]}.cs", testFilePath);
    }
  }

  public async static Task UseWrap(string cmd, string filePath)
  {
    try
    {
      var powerShellResults = await Cli.Wrap("powershell")
      .WithWorkingDirectory(filePath)
      .WithArguments(cmd)
      .ExecuteBufferedAsync();
    }
    catch
    {

    }
  }
}
