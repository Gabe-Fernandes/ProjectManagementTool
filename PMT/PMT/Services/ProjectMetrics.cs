using PMT.Data.Models;

namespace PMT.Services;

public static class ProjectMetrics
{
  public static List<string> TeamProdSprintData()
  {
    List<string> result = new();

    return result;
  }

  public static int GetTotalStoryWeight(List<Story> stories)
  {
    int totalStoryWeight = 0;

    for (int i = 0; i < stories.Count; i++)
    {
      int weight = DetermineStoryWeight(stories[i].Points);
      totalStoryWeight += weight;
    }

    return totalStoryWeight;
  }

  public static int GetTotalBugReportWeight(List<BugReport> bugReports)
  {
    int totalBugReportWeight = 0;

    for (int i = 0; i < bugReports.Count; i++)
    {
      int weight = DetermineStoryWeight(bugReports[i].Points);
      totalBugReportWeight += weight;
    }

    return totalBugReportWeight;
  }

  private static int DetermineStoryWeight(int fibNum)
  {
    return fibNum switch
    {
      1 => 1,
      2 => 2,
      3 => 3,
      5 => 4,
      8 => 5,
      13 => 6,
      _ => 0
    };
  }
}
