using PMT.Data.Models;

namespace PMT.Services.ProjectMetrics;

public class PieChartData
{
  public PieChartData(List<Story> stories, List<BugReport> bugReports)
  {
    TotalStoryWeight = GetTotalStoryWeight(stories);
    TotalBugReportWeight = GetTotalBugReportWeight(bugReports);
    TotalIssueWeight = TotalStoryWeight + TotalBugReportWeight;
    StoryPercentage = RoundToNearestInt(TotalStoryWeight / TotalIssueWeight * 100);
    BugReportPercentage = RoundToNearestInt(TotalBugReportWeight / TotalIssueWeight * 100);
  }

  public int TotalStoryWeight { get; set; }

  public int TotalBugReportWeight { get; set; }

  public double TotalIssueWeight { get; set; }
  
  public int StoryPercentage { get; set; }
  
  public int BugReportPercentage { get; set; }

  //// If method calls would except stories/bugreports as arguments, this single method could work better
  //public static int GetTotalIssueWeight(List<Issue> issues)
  //{
  //  int totalIssueWeight = 0;

  //  for (int i = 0; i < issues.Count; i++)
  //  {
  //    int weight = DetermineStoryWeight(issues[i].Points);
  //    totalIssueWeight += weight;
  //  }

  //  return totalIssueWeight;
  //}

  private static int GetTotalStoryWeight(List<Story> stories)
  {
    int totalStoryWeight = 0;

    for (int i = 0; i < stories.Count; i++)
    {
      int weight = DetermineIssueWeight(stories[i].Points);
      totalStoryWeight += weight;
    }

    return totalStoryWeight;
  }

  private static int GetTotalBugReportWeight(List<BugReport> bugReports)
  {
    int totalBugReportWeight = 0;

    for (int i = 0; i < bugReports.Count; i++)
    {
      int weight = DetermineIssueWeight(bugReports[i].Points);
      totalBugReportWeight += weight;
    }

    return totalBugReportWeight;
  }

  private static int RoundToNearestInt(double rawNumber)
  {
    // casting to an int always rounds down (if the decimal part of the original value was >= 0.5, it'll round down from the next integer, which is effectively rounding up)
    rawNumber += 0.5;
    return (int)rawNumber;
  }

  private static int DetermineIssueWeight(int fibNum)
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
